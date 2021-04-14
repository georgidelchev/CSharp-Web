using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using MyRecipes.Data.Common.Repositories;
using MyRecipes.Data.Models;
using MyRecipes.Services.Mapping;
using MyRecipes.Web.ViewModels.Recipes;

namespace MyRecipes.Services.Data
{
    public class RecipesService : IRecipesService
    {
        private readonly string[] allowedExtensions = { "jpg", "png", "gif" };
        private readonly IDeletableEntityRepository<Recipe> recipeRepository;
        private readonly IDeletableEntityRepository<Ingredient> ingredientsRepository;

        public RecipesService(
            IDeletableEntityRepository<Recipe> recipeRepository,
            IDeletableEntityRepository<Ingredient> ingredientsRepository)
        {
            this.recipeRepository = recipeRepository;
            this.ingredientsRepository = ingredientsRepository;
        }

        public async Task CreateAsync(CreateRecipeInputModel input, string userId, string imagePath)
        {
            var recipe = new Recipe()
            {
                PortionsCount = input.PortionsCount,
                CategoryId = int.Parse(input.CategoryId),
                CookingTime = TimeSpan.FromMinutes(input.CookingTime),
                PreparationTime = TimeSpan.FromMinutes(input.PreparationTime),
                Instructions = input.Instructions,
                Name = input.Name,
                AddedByUserId = userId,
            };

            foreach (var ingredient in input.Ingredients)
            {
                var currentIngredient = this.ingredientsRepository
                    .All()
                    .FirstOrDefault(i => i.Name == ingredient.Name)
                ?? new Ingredient()
                {
                    Name = ingredient.Name,
                };

                recipe.Ingredients.Add(new RecipeIngredient()
                {
                    Ingredient = currentIngredient,
                    Quantity = ingredient.Quantity,
                });
            }

            Directory.CreateDirectory($"{imagePath}/recipes/");
            foreach (var image in input.Images)
            {
                var extension = Path
                    .GetExtension(image.FileName)
                    .TrimStart('.');

                if (!this.allowedExtensions.Any(e => e.EndsWith(e)))
                {
                    throw new Exception($"Invalid image extension {extension}");
                }

                var dbImage = new Image()
                {
                    AddedByUserId = userId,
                    Recipe = recipe,
                    Extension = extension,
                };

                recipe.Images.Add(dbImage);

                var physicalPath = $"{imagePath}/recipes/{dbImage.Id}.{dbImage.Extension}";

                using var fileStream = new FileStream(physicalPath, FileMode.Create);
                await image.CopyToAsync(fileStream);
            }

            await this.recipeRepository.AddAsync(recipe);
            await this.recipeRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll<T>(int page, int itemsPerPage = 12)
        {
            var recipes = this.recipeRepository
                .AllAsNoTracking()
                .OrderByDescending(r => r.Id)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .To<T>()
                .ToList();

            return recipes;
        }

        public int GetCount()
        {
            return this.recipeRepository
                .All()
                .Count();
        }
    }
}
