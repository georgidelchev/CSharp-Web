using System.Diagnostics;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyRecipes.Services.Data;
using MyRecipes.Web.ViewModels;
using MyRecipes.Web.ViewModels.Home;

namespace MyRecipes.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IGetCountsService countsService;
        private readonly IRecipesService recipesService;

        public HomeController(
            IGetCountsService countsService,
            IRecipesService recipesService)
        {
            this.countsService = countsService;
            this.recipesService = recipesService;
        }

        public IActionResult Index()
        {
            var countsDto = this.countsService
                .GetCounts();

            var viewModel = new IndexViewModel()
            {
                CategoriesCount = countsDto.CategoriesCount,
                IngredientsCount = countsDto.IngredientsCount,
                ImagesCount = countsDto.ImagesCount,
                RecipesCount = countsDto.RecipesCount,
                RandomRecipes = this.recipesService.GetRandom<IndexPageRecipeViewModel>(10),
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
