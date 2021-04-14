using System.Collections.Generic;
using System.Threading.Tasks;

using MyRecipes.Web.ViewModels.Recipes;

namespace MyRecipes.Services.Data
{
    public interface IRecipesService
    {
        Task CreateAsync(CreateRecipeInputModel input, string userId, string imagePath);

        IEnumerable<T> GetAll<T>(int page, int itemsPerPage = 12);

        int GetCount();
    }
}
