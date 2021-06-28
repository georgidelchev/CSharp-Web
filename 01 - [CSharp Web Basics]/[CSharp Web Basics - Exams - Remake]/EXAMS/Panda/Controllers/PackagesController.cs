using SUS.HTTP;
using Panda.Services;
using SUS.MvcFramework;
using Panda.ViewModels.Packages;

namespace Panda.Controllers
{
    public class PackagesController : Controller
    {
        private readonly IUsersService usersService;
        private readonly IPackagesService packagesService;

        public PackagesController(
            IUsersService usersService,
            IPackagesService packagesService)
        {
            this.usersService = usersService;
            this.packagesService = packagesService;
        }

        [HttpGet]
        public HttpResponse Create()
            => !this.IsUserSignedIn() ? this.Redirect("/Users/Login") : this.View(this.usersService.GetAllUsernames());

        [HttpPost]
        public HttpResponse Create(CreatePackageInputModel input)
        {
            if (string.IsNullOrEmpty(input.Description) ||
                input.Description.Length < 5 ||
                input.Description.Length > 20 ||
                input.ShippingAddress == null)
            {
                return this.Redirect("/Packages/Create");
            }

            this.packagesService.Create(input);

            return this.Redirect("/");
        }

        [HttpGet]
        public HttpResponse Pending()
            => !this.IsUserSignedIn() ? this.Redirect("/Users/Login") : this.View(this.packagesService.GetPendings());

        [HttpGet]
        public HttpResponse Delivered()
            => !this.IsUserSignedIn() ? this.Redirect("/Users/Login") : this.View(this.packagesService.GetDelivered());

        [HttpGet]
        public HttpResponse Deliver(string id)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            this.packagesService.Deliver(id);

            return this.Redirect("/");
        }
    }
}
