using System.Linq;

using AutoMapper;
using MyRecipes.Data.Models;
using MyRecipes.Services.Mapping;

namespace MyRecipes.Web.ViewModels.Recipes
{
    public class RecipeInListViewModel : IMapFrom<Recipe>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        public string Name { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Recipe, RecipeInListViewModel>()
                .ForMember(r => r.ImageUrl, opt => opt.MapFrom(r => r.Images.FirstOrDefault().Url != null ?
                    r.Images.FirstOrDefault().Url :
                    "/images/recipes/" + r.Images.FirstOrDefault().Id + "." +
                                                    r.Images.FirstOrDefault().Extension));
        }
    }
}
