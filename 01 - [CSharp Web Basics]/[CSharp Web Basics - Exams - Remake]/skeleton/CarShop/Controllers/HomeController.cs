using SUS.HTTP;
using SUS.MvcFramework;

namespace CarShop.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public HttpResponse Index()
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/Cars/All");
            }

            return this.View();
        }
    }
}
