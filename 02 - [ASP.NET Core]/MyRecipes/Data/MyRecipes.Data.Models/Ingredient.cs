using System.Collections.Generic;

using MyRecipes.Data.Common.Models;

namespace MyRecipes.Data.Models
{
    public class Ingredient : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public ICollection<RecipeIngredient> Recipes { get; set; }
            = new HashSet<RecipeIngredient>();
    }
}
