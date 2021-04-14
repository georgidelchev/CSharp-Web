using System.Collections.Generic;

namespace MyRecipes.Web.ViewModels.Recipes
{
    public class RecipesListViewModel : PagingViewModel
    {
        public IEnumerable<RecipeInListViewModel> Recipes { get; set; }
    }
}
