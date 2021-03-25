using Microsoft.AspNetCore.Mvc;
using MyRecipes.Web.ViewModels.Administration.Dashboard;

namespace MyRecipes.Web.Areas.Administration.Controllers
{
    public class DashboardController : AdministrationController
    {
        public DashboardController()
        {
        }

        public IActionResult Index()
        {
            return this.View();
        }
    }
}
