using AutoMapper;
using MyRecipes.Data.Models;
using MyRecipes.Services.Mapping;

namespace MyRecipes.Web.ViewModels.Recipes
{
    public class EditRecipeInputModel : BaseRecipeInputModel, IMapFrom<Recipe>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<Recipe, EditRecipeInputModel>()
                .ForMember(r => r.CookingTime, opt => opt
                    .MapFrom(o => (int)o.CookingTime.TotalMinutes))
                .ForMember(r => r.PreparationTime, opt => opt
                    .MapFrom(o => (int)o.PreparationTime.TotalMinutes));
        }
    }
}
