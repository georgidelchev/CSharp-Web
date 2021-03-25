using System.Collections.Generic;

using MyRecipes.Data.Common.Models;

namespace MyRecipes.Data.Models
{
    public class Category : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public ICollection<Recipe> Recipes { get; set; }
            = new HashSet<Recipe>();
    }
}
