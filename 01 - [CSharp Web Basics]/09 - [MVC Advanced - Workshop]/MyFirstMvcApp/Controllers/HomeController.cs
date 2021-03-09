using SUS.HTTP;
using SUS.MvcFramework;

namespace BattleCards.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public HttpResponse Index()
        {
            if (this.IsUserSignedIn())
            {
                return Redirect("/Cards/All");
            }

            return this.View();
        }
    }
}