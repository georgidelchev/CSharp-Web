using SUS.HTTP;
using SUS.MvcFramework;

namespace Panda.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        [HttpGet("/")]
        public HttpResponse Index()
            => this.IsUserSignedIn() ? this.Redirect("/") : this.View();
    }
}
