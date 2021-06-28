using SUS.HTTP;
using Andreys.Services;
using SUS.MvcFramework;

namespace Andreys.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductsService productsService;

        public HomeController(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        [HttpGet]
        public HttpResponse Index()
            => this.IsUserSignedIn() ? this.Redirect("/") : this.View();

        [HttpGet("/")]
        public HttpResponse Home()
            => !this.IsUserSignedIn() ? this.Redirect("/Home/Index") : this.View(this.productsService.GetAll());
    }
}
