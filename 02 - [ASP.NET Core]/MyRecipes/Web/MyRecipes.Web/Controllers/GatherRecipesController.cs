using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using MyRecipes.Services;

namespace MyRecipes.Web.Controllers
{
    public class GatherRecipesController : Controller
    {
        private readonly IGotvachBgScraperService gotvachBgScraperService;

        public GatherRecipesController(IGotvachBgScraperService gotvachBgScraperService)
        {
            this.gotvachBgScraperService = gotvachBgScraperService;
        }

        public IActionResult RecipeIndex()
        {
            return this.View();
        }

        public async Task<IActionResult> Add()
        {
            await this.gotvachBgScraperService.PopulateDbWithRecipesAsync();

            return this.Redirect("/");
        }
    }
}
