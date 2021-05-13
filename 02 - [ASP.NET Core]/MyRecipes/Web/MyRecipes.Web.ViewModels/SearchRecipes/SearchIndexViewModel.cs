using System.Collections.Generic;

namespace MyRecipes.Web.ViewModels.SearchRecipes
{
    public class SearchIndexViewModel
    {
        public IEnumerable<IngredientNameIdViewModel> Ingredients { get; set; }
    }
}