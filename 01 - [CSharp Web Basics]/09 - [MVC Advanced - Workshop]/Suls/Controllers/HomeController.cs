using SUS.HTTP;
using SUS.MvcFramework;
using Suls.ViewModels.Problems;
using System.Collections.Generic;
using Suls.Services;

namespace Suls.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProblemsService problemsService;

        public HomeController(IProblemsService problemsService)
        {
            this.problemsService = problemsService;
        }

        [HttpGet("/")]
        public HttpResponse Index()
        {
            if (this.IsUserSignedIn())
            {
                var allProblems = this.problemsService.GetAll();

                return this.View(allProblems, "IndexLoggedIn");
            }

            return this.View();
        }
    }
}