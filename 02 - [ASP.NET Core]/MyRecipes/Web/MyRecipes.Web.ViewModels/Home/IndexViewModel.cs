using System.Collections.Generic;

namespace MyRecipes.Web.ViewModels.Home
{
    public class IndexViewModel
    {
        public IEnumerable<IndexPageRecipeViewModel> RandomRecipes { get; set; }

        public int RecipesCount { get; set; }

        public int CategoriesCount { get; set; }

        public int IngredientsCount { get; set; }

        public int ImagesCount { get; set; }
    }
}
