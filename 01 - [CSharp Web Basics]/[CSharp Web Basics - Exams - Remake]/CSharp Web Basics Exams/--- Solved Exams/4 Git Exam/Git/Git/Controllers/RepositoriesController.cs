using SUS.HTTP;
using Git.Services;
using SUS.MvcFramework;

namespace Git.Controllers
{
    public class RepositoriesController : Controller
    {
        private readonly IRepositoriesService repositoriesService;

        public RepositoriesController(IRepositoriesService repositoriesService)
        {
            this.repositoriesService = repositoriesService;
        }

        public HttpResponse All()
        {
            var viewModel = this.repositoriesService.GetAll();

            return this.View(viewModel);
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
        public HttpResponse Create(string name, string repositoryType)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (string.IsNullOrEmpty(name) ||
                name.Length < 3 ||
                name.Length > 10)
            {
                return this.Error("Name should be between 3 and 10 characters.");
            }

            if (repositoryType != "Public" && repositoryType != "Private")
            {
                return this.Error("Invalid repository type.");
            }

            var userId = this.GetUserId();

            if (userId == null)
            {
                return this.Error("Error occurred.");
            }

            this.repositoriesService.Create(name, repositoryType, userId);

            return this.Redirect("/Repositories/All");
        }
    }
}