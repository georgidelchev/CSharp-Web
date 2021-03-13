using SUS.HTTP;
using Suls.Services;
using SUS.MvcFramework;

namespace Suls.Controllers
{
    public class ProblemsController : Controller
    {
        private readonly IProblemsService problemsService;

        public ProblemsController(IProblemsService problemsService)
        {
            this.problemsService = problemsService;
        }

        public HttpResponse Create()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Create(string name, int points)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (string.IsNullOrEmpty(name) ||
                name.Length < 5 ||
                name.Length > 20)
            {
                //return this.Error("Name should be between 5 and 20 characters.");

                return this.Redirect("/Problems/Create");
            }

            if (points < 50 ||
                points > 300)
            {
                //return this.Error("Points should be between 50 and 300.");

                return this.Redirect("/Problems/Create");
            }

            this.problemsService.Create(name, points);

            return this.Redirect("/");
        }

        public HttpResponse Details(string id)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var viewModel = this.problemsService.GetDetails(id);

            return this.View(viewModel);
        }
    }
}