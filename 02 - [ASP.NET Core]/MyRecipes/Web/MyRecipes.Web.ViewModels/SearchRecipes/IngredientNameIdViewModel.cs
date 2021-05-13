using MyRecipes.Data.Models;
using MyRecipes.Services.Mapping;

namespace MyRecipes.Web.ViewModels.SearchRecipes
{
    public class IngredientNameIdViewModel : IMapFrom<Ingredient>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}