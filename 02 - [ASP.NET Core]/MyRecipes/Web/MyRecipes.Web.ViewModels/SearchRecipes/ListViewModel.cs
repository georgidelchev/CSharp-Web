using System.Collections.Generic;

using MyRecipes.Web.ViewModels.Recipes;

namespace MyRecipes.Web.ViewModels.SearchRecipes
{
    public class ListViewModel
    {
        public IEnumerable<RecipeInListViewModel> Recipes { get; set; }
    }
}
