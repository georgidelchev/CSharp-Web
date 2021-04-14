using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyRecipes.Data.Models;
using MyRecipes.Services.Data;
using MyRecipes.Web.ViewModels.Recipes;

namespace MyRecipes.Web.Controllers
{
    public class RecipesController : Controller
    {
        private const int ItemsPerPage = 12;

        private readonly ICategoriesService categoriesService;
        private readonly IRecipesService recipesService;
        private readonly UserManager<ApplicationUser> userManager;

        public RecipesController(ICategoriesService categoriesService, IRecipesService recipesService, UserManager<ApplicationUser> userManager)
        {
            this.categoriesService = categoriesService;
            this.recipesService = recipesService;

            this.userManager = userManager;
        }

        public IActionResult All(int id = 1)
        {
            if (id <= 0)
            {
                return this.NotFound();
            }

            var viewModel = new RecipesListViewModel()
            {
                PageNumber = id,
                Recipes = this.recipesService.GetAll<RecipeInListViewModel>(id, 12),
                RecipesCount = this.recipesService.GetCount(),
                ItemsPerPage = ItemsPerPage,
            };

            return this.View(viewModel);
        }

        [Authorize]
        public IActionResult Create()
        {
            var viewModel = new CreateRecipeInputModel();

            viewModel.CategoriesItems = this.categoriesService.GetAllAsKeyValuePairs();

            return this.View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateRecipeInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.CategoriesItems = this.categoriesService.GetAllAsKeyValuePairs();

                return this.View(input);
            }

            // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await this.userManager.GetUserAsync(this.User);

            await this.recipesService.CreateAsync(input, user.Id);

            // TODO: redirect to recipe info page.
            return this.Redirect("/");
        }
    }
}
