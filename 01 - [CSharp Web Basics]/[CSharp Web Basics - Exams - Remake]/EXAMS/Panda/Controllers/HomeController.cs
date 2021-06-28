using SUS.HTTP;
using Panda.Services;
using SUS.MvcFramework;

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
            => this.IsUserSignedIn()
                ? this.View(this.usersService.GetUsername(this.GetUserId()), "IndexLoggedIn")
                : this.View();
    }
}
