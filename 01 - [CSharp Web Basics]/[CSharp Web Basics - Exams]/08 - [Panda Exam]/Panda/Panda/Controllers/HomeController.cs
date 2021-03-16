using SUS.HTTP;
using Panda.Services;
using SUS.MvcFramework;
using Panda.ViewModels.Home;

namespace Panda.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUsersService usersService;

        public HomeController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        [HttpGet("/")]
        public HttpResponse Index()
        {
            if (this.IsUserSignedIn())
            {
                var userId = this.GetUserId();

                var viewModel = new IndexLoggedInViewModel()
                {
                    Username = this.usersService.GetUsernameById(userId)
                };

                return this.View(viewModel, "IndexLoggedIn");
            }

            return this.View();
        }
    }
}