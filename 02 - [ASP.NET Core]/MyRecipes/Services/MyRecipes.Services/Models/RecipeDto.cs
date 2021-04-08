using System;
using System.Collections.Generic;

using MyRecipes.Data.Models;

namespace MyRecipes.Services.Models
{
    public class RecipeDto
    {
        public string CategoryName { get; set; }

        public string RecipeName { get; set; }

        public string Instructions { get; set; }

        public TimeSpan PreparationTime { get; set; }

        public TimeSpan CookingTime { get; set; }

        public int PortionsCount { get; set; }

        public int OriginalRecipeId { get; set; }

        public string OriginalUrl { get; set; }

        public ICollection<string> Ingredients { get; set; }
            = new List<string>();

        public ICollection<string> Images { get; set; }
            = new HashSet<string>();
    }
}
