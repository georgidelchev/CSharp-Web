using SUS.HTTP;
using Panda.Services;
using SUS.MvcFramework;
using Panda.ViewModels.Packages;

namespace Panda.Controllers
{
    public class PackagesController : Controller
    {
        private readonly IPackagesService packagesService;
        private readonly IUsersService usersService;

        public PackagesController(IPackagesService packagesService, IUsersService usersService)
        {
            this.packagesService = packagesService;
            this.usersService = usersService;
        }

        public HttpResponse Create()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var viewModel = this.usersService.GetAll();

            return this.View(viewModel);
        }

        [HttpPost]
        public HttpResponse Create(CreatePackageInputModel input)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (string.IsNullOrEmpty(input.Description) ||
                input.Description.Length < 5 ||
                input.Description.Length > 20)
            {
                //return this.Error("Description should be between 5 and 20 characters.");

                return this.Redirect("/Packages/Create");
            }

            this.packagesService.Create(input, input.RecipientName);

            return this.Redirect("/");
        }

        public HttpResponse Pending()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var viewModel = this.packagesService.GetPendingPackages();

            return this.View(viewModel);
        }

        public HttpResponse Deliver(string id)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            this.packagesService.DeliverPackage(id);

            return this.Redirect("/Packages/Pending");
        }

        public HttpResponse Delivered()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var viewModel = this.packagesService.GetDeliveredPackages();

            return this.View(viewModel);
        }
    }
}