using System;
using System.Collections.Generic;

using MyRecipes.Data.Common.Models;

namespace MyRecipes.Data.Models
{
    public class Recipe : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public string Instructions { get; set; }

        public TimeSpan PreparationTime { get; set; }

        public TimeSpan CookingTime { get; set; }

        public string OriginalUrl { get; set; }

        public int PortionsCount { get; set; }

        public int OriginalId { get; set; }

        public string AddedByUserId { get; set; }

        public virtual ApplicationUser AddedByUser { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<RecipeIngredient> Ingredients { get; set; }
            = new HashSet<RecipeIngredient>();

        public virtual ICollection<Image> Images { get; set; }
            = new HashSet<Image>();
    }
}
