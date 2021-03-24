using Microsoft.AspNetCore.Mvc;
using MyRecipes.Services.Data;
using MyRecipes.Web.ViewModels.Administration.Dashboard;

namespace MyRecipes.Web.Areas.Administration.Controllers
{
    public class DashboardController : AdministrationController
    {
        private readonly ISettingsService settingsService;

        public DashboardController(ISettingsService settingsService)
        {
            this.settingsService = settingsService;
        }

        public IActionResult Index()
        {
            var viewModel = new IndexViewModel { SettingsCount = this.settingsService.GetCount(), };
            return this.View(viewModel);
        }
    }
}
