using System.Diagnostics;
using System.Linq;

using Microsoft.AspNetCore.Mvc;
using MyRecipes.Data;
using MyRecipes.Web.ViewModels;
using MyRecipes.Web.ViewModels.Home;

namespace MyRecipes.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ApplicationDbContext dbContext;

        public HomeController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var viewModel = new IndexViewModel()
            {
                CategoriesCount = this.dbContext.Categories.Count(),
                ImagesCount = this.dbContext.Images.Count(),
                IngredientsCount = this.dbContext.Ingredients.Count(),
                RecipesCount = this.dbContext.Recipes.Count(),
            };

            return this.View(viewModel);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
