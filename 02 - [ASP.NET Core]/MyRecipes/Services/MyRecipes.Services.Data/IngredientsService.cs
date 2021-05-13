using System.Collections.Generic;
using System.Linq;

using MyRecipes.Data.Common.Repositories;
using MyRecipes.Data.Models;
using MyRecipes.Services.Mapping;

namespace MyRecipes.Services.Data
{
    public class IngredientsService : IIngredientsService
    {
        private readonly IDeletableEntityRepository<Ingredient> ingredientsRepository;

        public IngredientsService(IDeletableEntityRepository<Ingredient> ingredientsRepository)
        {
            this.ingredientsRepository = ingredientsRepository;
        }

        public IEnumerable<T> GetAllPopular<T>()
        {
            return this.ingredientsRepository
                .All()
                .Where(i => i.Recipes.Count >= 20)
                .OrderByDescending(i => i.Recipes.Count)
                .To<T>()
                .ToList();
        }
    }
}