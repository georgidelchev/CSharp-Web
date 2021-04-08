using System.Threading.Tasks;

using MyRecipes.Web.ViewModels.Recipes;

namespace MyRecipes.Services.Data
{
    public interface IRecipesService
    {
        Task CreateAsync(CreateRecipeInputModel input);
    }
}
