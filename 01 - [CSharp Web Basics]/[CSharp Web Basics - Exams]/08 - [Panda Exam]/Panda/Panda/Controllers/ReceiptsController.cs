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

        public HttpResponse Index()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var userId = this.GetUserId();

            var viewModel = this.receiptsService.GetAll(userId);

            return this.View(viewModel);
        }
    }
}