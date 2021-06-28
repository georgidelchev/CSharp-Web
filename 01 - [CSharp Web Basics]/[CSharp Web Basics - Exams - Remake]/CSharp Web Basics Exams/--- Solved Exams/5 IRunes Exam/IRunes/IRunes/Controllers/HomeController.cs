using SUS.HTTP;
using IRunes.Services;
using SUS.MvcFramework;

namespace IRunes.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUsersService usersService;

        public HomeController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public HttpResponse Index()
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }

            return this.View();
        }

        [HttpGet("/")]
        public HttpResponse Home()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Home/Index");
            }

            var userId = this.GetUserId();

            var viewModel = this.usersService.GetUsername(userId);

            return this.View(viewModel);
        }
    }
}