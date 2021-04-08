using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using HtmlAgilityPack;
using MyRecipes.Data.Common.Repositories;
using MyRecipes.Data.Models;
using MyRecipes.Services.Models;

namespace MyRecipes.Services
{
    public class GotvachBgScraperService : IGotvachBgScraperService
    {
        private readonly object lockObj = new();

        private readonly HtmlWeb web;

        private readonly ConcurrentBag<RecipeDto> concurrentBag;

        private readonly IDeletableEntityRepository<Category> categoriesRepository;
        private readonly IDeletableEntityRepository<Ingredient> ingredientsRepository;
        private readonly IDeletableEntityRepository<Recipe> recipesRepository;
        private readonly IRepository<RecipeIngredient> recipeIngredientsRepository;
        private readonly IRepository<Image> imagesRepository;

        private HttpStatusCode statusCode;

        public GotvachBgScraperService(
            HtmlWeb web,
            IDeletableEntityRepository<Category> categoriesRepository,
            IDeletableEntityRepository<Ingredient> ingredientsRepository,
            IDeletableEntityRepository<Recipe> recipeRepository,
            IRepository<RecipeIngredient> recipeIngredientsRepository,
            IRepository<Image> imagesRepository)
        {
            this.web = web;
            this.web.PostResponse += (request, response) =>
            {
                if (response != null)
                {
                    this.statusCode = response.StatusCode;
                }
            };

            this.concurrentBag = new ConcurrentBag<RecipeDto>();

            this.categoriesRepository = categoriesRepository;
            this.ingredientsRepository = ingredientsRepository;
            this.recipesRepository = recipeRepository;
            this.recipeIngredientsRepository = recipeIngredientsRepository;
            this.imagesRepository = imagesRepository;

            this.statusCode = HttpStatusCode.OK;
        }

        public async Task PopulateDbWithRecipesAsync()
        {
            this.ScrapeRecipes();

            foreach (var recipeDto in this.concurrentBag)
            {
                var categoryId = await this.GetOrCreateCategoryAsync(recipeDto.CategoryName);

                if (recipeDto.CookingTime.Days >= 1)
                {
                    recipeDto.CookingTime = new TimeSpan(23, 59, 59);
                }

                if (recipeDto.PreparationTime.Days >= 1)
                {
                    recipeDto.PreparationTime = new TimeSpan(23, 59, 59);
                }

                Recipe recipe = null;
                if (!this.IsRecipeNameExisting(recipeDto.RecipeName))
                {
                    recipe = await this.CreateRecipeAsync(recipeDto, categoryId);
                }
                else
                {
                    recipe = this.GetRecipe(recipeDto.RecipeName);
                }

                foreach (var ingredient in recipeDto.Ingredients)
                {
                    var split = ingredient
                        .Split(" - ", 2)
                        .ToList();

                    var ingredientId = 0;
                    var quantity = string.Empty;

                    if (split.Count >= 2)
                    {
                        ingredientId = await this.GetOrCreateIngredientAsync(split[0].TrimEnd());

                        quantity = split[1];
                    }
                    else
                    {
                        ingredientId = await this.GetOrCreateIngredientAsync(split[0].TrimEnd());
                    }

                    await this.CreateRecipeIngredientAsync(recipe, ingredientId, quantity);
                }

                foreach (var imageUrl in recipeDto.Images)
                {
                    var imageExtension = Path
                        .GetExtension(imageUrl)
                        .Substring(1);

                    await this.CreateImageAsync(recipe, imageUrl, imageExtension);
                }
            }
        }

        private void ScrapeRecipes()
        {
            Parallel.For(1, 200000 + 1, i =>
            {
                try
                {
                    this.IsRecipeIdExisting(i);

                    this.concurrentBag.Add(this.GetRecipeDetails(i));

                    Console.WriteLine(this.concurrentBag.Count + " => " + i + " index");
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message + " => " + i + " index");
                }
                catch (Exception)
                {
                }
            });
        }

        private async Task<Recipe> CreateRecipeAsync(RecipeDto recipeDto, int categoryId)
        {
            var recipe = new Recipe()
            {
                PortionsCount = recipeDto.PortionsCount,
                CategoryId = categoryId,
                Instructions = recipeDto.Instructions,
                Name = recipeDto.RecipeName,
                CookingTime = recipeDto.CookingTime,
                OriginalUrl = recipeDto.OriginalUrl,
                PreparationTime = recipeDto.PreparationTime,
                OriginalId = recipeDto.OriginalRecipeId,
            };

            await this.recipesRepository.AddAsync(recipe);
            await this.recipesRepository.SaveChangesAsync();

            return recipe;
        }

        private Recipe GetRecipe(string recipeName)
        {
            return this.recipesRepository
                .AllAsNoTracking()
                .FirstOrDefault(r => r.Name == recipeName);
        }

        private async Task<int> GetOrCreateIngredientAsync(string name)
        {
            var ingredient = this.ingredientsRepository
                .AllAsNoTracking()
                .FirstOrDefault(i => i.Name == name);

            if (ingredient == null)
            {
                ingredient = new Ingredient()
                {
                    Name = name,
                };

                await this.ingredientsRepository.AddAsync(ingredient);
                await this.ingredientsRepository.SaveChangesAsync();
            }

            return ingredient.Id;
        }

        private async Task CreateRecipeIngredientAsync(Recipe recipe, int ingredientId, string quantity)
        {
            var recipeIngredient = new RecipeIngredient()
            {
                IngredientId = ingredientId,
                Quantity = quantity,
                RecipeId = recipe.Id,
            };

            await this.recipeIngredientsRepository.AddAsync(recipeIngredient);
            await this.recipeIngredientsRepository.SaveChangesAsync();
        }

        private async Task<int> GetOrCreateCategoryAsync(string categoryName)
        {
            var category = this.categoriesRepository
                                .AllAsNoTracking()
                                .FirstOrDefault(c => c.Name == categoryName);

            if (category == null)
            {
                category = new Category()
                {
                    Name = categoryName,
                };

                await this.categoriesRepository.AddAsync(category);
                await this.categoriesRepository.SaveChangesAsync();
            }

            return category.Id;
        }

        private async Task CreateImageAsync(Recipe recipe, string imageUrl, string imageExtension)
        {
            var image = new Image()
            {
                Url = imageUrl,
                RecipeId = recipe.Id,
                Extension = imageExtension,
            };

            await this.imagesRepository.AddAsync(image);
            await this.imagesRepository.SaveChangesAsync();
        }

        // Recipe Methods
        private RecipeDto GetRecipeDetails(int i)
        {
            var html = $"https://recepti.gotvach.bg/r-{i}";

            var htmlDoc = this.web.Load(html);

            if (this.web.StatusCode != HttpStatusCode.OK)
            {
                throw new InvalidOperationException();
            }

            var recipeDto = new RecipeDto();

            // Recipe Real Id
            this.GetRecipeOriginalId(recipeDto, html);

            // Recipe Original Url
            this.GetRecipeOriginalUrl(html, recipeDto);

            // Recipe Name
            this.GetRecipeName(htmlDoc, recipeDto);

            // Recipe Category
            this.GetRecipeCategory(htmlDoc, recipeDto);

            // Recipe Date
            // GetRecipeOriginalPublishDate(htmlDoc);

            // Recipe Pictures Urls
            var allPicturesUrlParse = htmlDoc.DocumentNode
                .SelectNodes(@"//a[@class='morebtn']");

            if (allPicturesUrlParse != null)
            {
                var allPicturesUrlParsed = allPicturesUrlParse
                    .FirstOrDefault()
                    ?.GetAttributeValue("href", "unknown");

                var link = this.web.Load(allPicturesUrlParsed);

                var allPicturesUrlsParse = link
                    .DocumentNode
                    .SelectNodes(@"//div[@class='main']/div/img");

                if (allPicturesUrlsParse != null)
                {
                    var allPicturesUrl = allPicturesUrlsParse.ToList();

                    if (allPicturesUrl[0].GetAttributeValue("src", "unknown") == "https://recepti.gotvach.bg/files/recipes/photos/")
                    {
                        allPicturesUrl.Clear();
                    }
                    else
                    {
                        foreach (var picture in allPicturesUrl)
                        {
                            recipeDto.Images.Add(picture.GetAttributeValue("src", "unknown"));
                        }
                    }
                }
            }

            // Preparation and Cooking times
            this.GetRecipeTimes(htmlDoc, recipeDto);

            // Recipe Portions Count
            this.GetRecipePortionsCount(htmlDoc, recipeDto);

            // Recipe Ingredient => Quantity
            this.GetRecipeIngredients(htmlDoc, recipeDto);

            // Recipe instructions
            this.GetRecipeInstructions(htmlDoc, recipeDto);

            return recipeDto;
        }

        private void GetRecipeOriginalId(RecipeDto recipeDto, string html)
        {
            recipeDto.OriginalRecipeId = int.Parse(html.Split("r-", 2)[1]);
        }

        private void GetRecipeOriginalUrl(string html, RecipeDto recipeDto)
        {
            recipeDto.OriginalUrl = html;
        }

        private void GetRecipeName(HtmlDocument htmlDoc, RecipeDto recipeDto)
        {
            var recipeParse = htmlDoc
                            .DocumentNode
                            .SelectNodes(@"//div[@class='combocolumn mr']/h1");

            if (recipeParse != null)
            {
                recipeDto.RecipeName = recipeParse
                    .Select(r => r.InnerText)
                    .FirstOrDefault();
            }
        }

        private void GetRecipeCategory(HtmlDocument htmlDoc, RecipeDto recipeDto)
        {
            var recipeCategoryParse = htmlDoc
                            .DocumentNode
                            .SelectNodes(@"//div[@class='breadcrumb']");

            if (recipeCategoryParse != null)
            {
                recipeDto.CategoryName = recipeCategoryParse
                    .Select(c => c.InnerText)
                    .FirstOrDefault()
                    .Split(" »")
                    .Reverse()
                    .ToList()[1];
            }
        }

        private void GetRecipeTimes(HtmlDocument htmlDoc, RecipeDto recipeDto)
        {
            var timesParse = htmlDoc
                            .DocumentNode
                            .SelectNodes(@"//div[@class='feat small']");

            if (timesParse == null)
            {
                return;
            }

            if (timesParse.Count == 2)
            {
                recipeDto.PreparationTime = TimeSpan.FromMinutes(int.Parse(this.ParseTime(timesParse, 0, "Приготвяне")));

                recipeDto.CookingTime = TimeSpan.FromMinutes(int.Parse(this.ParseTime(timesParse, 1, "Готвене")));
            }
            else if (timesParse.Count == 1)
            {
                if (timesParse[0].InnerText.Contains("Приготвяне"))
                {
                    recipeDto.PreparationTime = TimeSpan.FromMinutes(int.Parse(this.ParseTime(timesParse, 0, "Приготвяне")));
                }
                else
                {
                    recipeDto.CookingTime = TimeSpan.FromMinutes(int.Parse(this.ParseTime(timesParse, 1, "Готвене")));
                }
            }
        }

        private void GetRecipePortionsCount(HtmlDocument htmlDoc, RecipeDto recipeDto)
        {
            var portionsParse = htmlDoc
                            .DocumentNode
                            .SelectNodes(@"//div[@class='feat']")
                            .LastOrDefault()
                            ?.InnerText
                            .Split(new[] { "-", "Порции ", " " }, 2, StringSplitOptions.RemoveEmptyEntries)
                            .ToList();

            if (portionsParse != null &&
                portionsParse[0].Contains("Порции"))
            {
                recipeDto.PortionsCount = int.Parse(portionsParse[0].Replace("Порции", string.Empty)
                                                                    .Replace("бр", string.Empty)
                                                                    .Replace("бр.", string.Empty)
                                                                    .Replace("броя", string.Empty)
                                                                    .Replace("бройки", string.Empty));
            }
        }

        private void GetRecipeIngredients(HtmlDocument htmlDoc, RecipeDto recipeDto)
        {
            var ingredientsAndQuantityParse = htmlDoc
                            .DocumentNode
                            .SelectNodes(@"//section[@class='products new']/ul/li");

            if (ingredientsAndQuantityParse != null)
            {
                var ingredientsAndQuantity = ingredientsAndQuantityParse
                    .Select(li => li.InnerText);

                foreach (var ingredientQuantity in ingredientsAndQuantity)
                {
                    recipeDto.Ingredients.Add(ingredientQuantity);
                }
            }
        }

        private void GetRecipeInstructions(HtmlDocument htmlDoc, RecipeDto recipeDto)
        {
            var instructionsParse = htmlDoc
                            .DocumentNode
                            .SelectNodes(@"//p[@class='desc']");

            if (instructionsParse != null)
            {
                var description = instructionsParse
                    .Select(d => d.InnerText)
                    .ToList();

                var fullDescription = new StringBuilder();

                foreach (var desc in description)
                {
                    fullDescription.AppendLine(desc);
                }

                recipeDto.Instructions = fullDescription
                    .ToString()
                    .TrimEnd();
            }
        }

        // Helping method.
        private void IsRecipeIdExisting(int i)
        {
            lock (this.lockObj)
            {
                if (this.recipesRepository.AllAsNoTracking().Any(r => r.OriginalId == i))
                {
                    throw new InvalidOperationException();
                }
            }
        }

        private bool IsRecipeNameExisting(string recipeName)
        {
            return this.recipesRepository
                .AllAsNoTracking()
                .Any(r => r.Name == recipeName);
        }

        private string ParseTime(HtmlNodeCollection timesParse, int index, string timeType)
        {
            var time = timesParse[index]
                .InnerText
                .Replace(timeType, string.Empty)
                .Replace(" мин.", string.Empty);

            return time;
        }

        // Possible to use.
        private void GetRecipeOriginalPublishDate(HtmlDocument htmlDoc)
        {
            var recipeDateParse = htmlDoc.DocumentNode
                .SelectNodes(@"//span[@class='date']");

            if (recipeDateParse != null)
            {
                var recipeDate = recipeDateParse
                    .Select(r => r.InnerText)
                    .FirstOrDefault();
            }
        }
    }
}
