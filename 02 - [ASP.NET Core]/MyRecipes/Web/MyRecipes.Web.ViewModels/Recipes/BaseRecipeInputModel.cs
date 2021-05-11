using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyRecipes.Web.ViewModels.Recipes
{
    public abstract class BaseRecipeInputModel
    {
        [Required]
        [MinLength(4)]
        public string Name { get; set; }

        [Required]
        [MinLength(100)]
        public string Instructions { get; set; }

        [Range(0, 24 * 60)]
        [DisplayName("Preparation time (in minutes)")]
        public int PreparationTime { get; set; }

        [Range(0, 24 * 60)]
        [DisplayName("Cooking time (in minutes)")]
        public int CookingTime { get; set; }

        [Range(1, 100)]
        public int PortionsCount { get; set; }

        public int CategoryId { get; set; }

        public IEnumerable<KeyValuePair<string, string>> CategoriesItems { get; set; }
    }
}
