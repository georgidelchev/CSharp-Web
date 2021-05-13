using System;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyRecipes.Common;
using MyRecipes.Data.Models;
using MyRecipes.Services.Data;
using MyRecipes.Services.Messaging;
using MyRecipes.Web.ViewModels.Recipes;

namespace MyRecipes.Web.Controllers
{
    public class RecipesController : Controller
    {
        private const int ItemsPerPage = 12;

        private readonly ICategoriesService categoriesService;
        private readonly IRecipesService recipesService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment environment;
        private readonly IEmailSender emailSender;

        public RecipesController(
            ICategoriesService categoriesService,
            IRecipesService recipesService,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment environment,
            IEmailSender emailSender)
        {
            this.categoriesService = categoriesService;
            this.recipesService = recipesService;

            this.userManager = userManager;
            this.environment = environment;
            this.emailSender = emailSender;
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

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult Edit(int id)
        {
            var inputModel = this.recipesService
                .GetById<EditRecipeInputModel>(id);

            inputModel.CategoriesItems = this.categoriesService
                .GetAllAsKeyValuePairs();

            return this.View(inputModel);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Edit(int id, EditRecipeInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.CategoriesItems = this.categoriesService
                    .GetAllAsKeyValuePairs();

                return this.View(input);
            }

            await this.recipesService
                .UpdateAsync(id, input);

            return this.RedirectToAction(nameof(this.ById), new { id });
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

            try
            {
                await this.recipesService.CreateAsync(input, user.Id, $"{this.environment.WebRootPath}/images");
            }
            catch (Exception e)
            {
                this.ModelState.AddModelError(string.Empty, e.Message);

                input.CategoriesItems = this.categoriesService.GetAllAsKeyValuePairs();

                return this.View(input);
            }

            this.TempData["Message"] = "Recipe added successfully.";

            // TODO: redirect to recipe info page.
            return this.RedirectToAction("All");
        }

        public IActionResult ById(int id)
        {
            var recipe = this.recipesService.GetById<SingleRecipeViewModel>(id);

            return this.View(recipe);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Delete(int id)
        {
            await this.recipesService.DeleteAsync(id);

            return this.RedirectToAction(nameof(this.All));
        }

        [HttpPost]
        public async Task<IActionResult> SendToEmail(int id)
        {
            var recipe = this.recipesService
                .GetById<RecipeInListViewModel>(id);

            var html = new StringBuilder();

            html.AppendLine($"<h1>{recipe.Name}</h1>")
                .AppendLine($"<h3>{recipe.CategoryName}</h3>");

            await this.emailSender
                .SendEmailAsync("delchev_1981@abv.bg", "MyRecipes", "alienguymc@gmail.com", recipe.Name, html.ToString());

            return this.RedirectToAction(nameof(this.ById), new { id });
        }
    }
}
