using System.Linq;

using AutoMapper;
using MyRecipes.Data.Models;
using MyRecipes.Services.Mapping;

namespace MyRecipes.Web.ViewModels.Home
{
    public class IndexPageRecipeViewModel : IMapFrom<Recipe>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string CategoryName { get; set; }

        public string ImageUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Recipe, IndexPageRecipeViewModel>()
                .ForMember(r => r.ImageUrl, opt => opt.MapFrom(r => r.Images.FirstOrDefault().Url != null ?
                    r.Images.FirstOrDefault().Url :
                    "/images/recipes/" + r.Images.FirstOrDefault().Id + "." +
                    r.Images.FirstOrDefault().Extension));
        }
    }
}
