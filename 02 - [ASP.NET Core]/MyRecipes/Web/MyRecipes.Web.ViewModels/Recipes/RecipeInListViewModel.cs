using System;
using System.Globalization;
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

        public string ShortName
            => this.Name.Substring(0, this.Name.Length >= 20 ? 20 : this.Name.Length);

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedOnAsString
            => this.CreatedOn.ToString("d", CultureInfo.InvariantCulture);

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
