using SUS.HTTP;
using SUS.MvcFramework;

namespace BattleCards.Controllers
{
    public class HomeController : Controller
    {
        public HttpResponse Index()
        {
            return this.View();
        }
    }
}
