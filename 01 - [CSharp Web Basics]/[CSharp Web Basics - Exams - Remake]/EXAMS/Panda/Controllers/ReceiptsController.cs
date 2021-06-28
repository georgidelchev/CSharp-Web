using SUS.HTTP;
using Panda.Services;
using SUS.MvcFramework;

namespace Panda.Controllers
{
    public class ReceiptsController : Controller
    {
        private readonly IReceiptsService receiptsService;

        public ReceiptsController(IReceiptsService receiptsService)
        {
            this.receiptsService = receiptsService;
        }

        [HttpGet]
        public HttpResponse Index()
            => !this.IsUserSignedIn()
                ? this.Redirect("/Users/Login")
                : this.View(this.receiptsService.GetAllForUser(this.GetUserId()));
    }
}
