using SUS.HTTP;
using Suls.Services;
using SUS.MvcFramework;
using Suls.ViewModels.Submissions;

namespace Suls.Controllers
{
    public class SubmissionsController : Controller
    {
        private readonly IProblemsService problemsService;

        private readonly ISubmissionsService submissionsService;

        public SubmissionsController(IProblemsService problemsService, ISubmissionsService submissionsService)
        {
            this.problemsService = problemsService;
            this.submissionsService = submissionsService;
        }

        public HttpResponse Create(string id)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var viewModel = new CreateViewModel()
            {
                Name = this.problemsService.GetNameById(id),
                ProblemId = id
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public HttpResponse Create(string problemId, string code)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var userId = this.GetUserId();

            if (string.IsNullOrEmpty(code) ||
                code.Length < 30 ||
                code.Length > 300)
            {
                return this.Error("Submission must be between 30 and 300 characters.");
            }

            this.submissionsService.Create(userId, problemId, code);

            return this.Redirect("/");
        }

        public HttpResponse Delete(string id)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            this.submissionsService.Delete(id);

            return this.Redirect("/");
        }
    }
}