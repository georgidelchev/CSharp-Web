using System;
using System.Collections.Generic;
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
        private readonly IDeletableEntityRepository<Recipe> recipeRepository;

        private readonly IDeletableEntityRepository<Ingredient> ingredientsRepository;

        public RecipesService(IDeletableEntityRepository<Recipe> recipeRepository, IDeletableEntityRepository<Ingredient> ingredientsRepository)
        {
            this.recipeRepository = recipeRepository;
            this.ingredientsRepository = ingredientsRepository;
        }

        public async Task CreateAsync(CreateRecipeInputModel input, string userId)
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
                    .FirstOrDefault(i => i.Name == ingredient.Name);

                if (currentIngredient == null)
                {
                    currentIngredient = new Ingredient()
                    {
                        Name = ingredient.Name,
                    };
                }

                recipe.Ingredients.Add(new RecipeIngredient()
                {
                    Ingredient = currentIngredient,
                    Quantity = ingredient.Quantity,
                });
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
            return this.recipeRepository.All().Count();
        }
    }
}
