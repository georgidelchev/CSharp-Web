using System;
using System.Linq;
using System.Threading.Tasks;

using MyRecipes.Data.Common.Repositories;
using MyRecipes.Data.Models;
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

        public async Task CreateAsync(CreateRecipeInputModel input)
        {
            var recipe = new Recipe()
            {
                PortionsCount = input.PortionsCount,
                CategoryId = int.Parse(input.CategoryId),
                CookingTime = TimeSpan.FromMinutes(input.CookingTime),
                PreparationTime = TimeSpan.FromMinutes(input.PreparationTime),
                Instructions = input.Instructions,
                Name = input.Name,
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
    }
}
