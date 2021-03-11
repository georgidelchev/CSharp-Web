using System;
using SUS.HTTP;
using SUS.MvcFramework;
using Andreys.Services;
using Andreys.Data.Enums;
using Andreys.ViewModels.Products;

namespace Andreys.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductsService productsService;

        public ProductsController(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        public HttpResponse Add()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Add(AddProductInputModel input)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (string.IsNullOrEmpty(input.Name) ||
                input.Name.Length < 4 ||
                input.Name.Length > 20)
            {
                //return this.Error("Name should be between 4 and 20 characters.");

                return this.Redirect("/Products/Add");
            }

            if (input.Description.Length > 10)
            {
                //return this.Error("Description cannot be more than 10 characters.");

                return this.Redirect("/Products/Add");
            }

            if (!Enum.IsDefined(typeof(Category), input.Category))
            {
                //return this.Error("Category is invalid.");

                return this.Redirect("/Products/Add");
            }

            if (!Enum.IsDefined(typeof(Gender), input.Gender))
            {
                //return this.Error("Gender is invalid.");

                return this.Redirect("/Products/Add");
            }

            this.productsService.Add(input);

            return this.Redirect("/");
        }

        public HttpResponse Details(int id)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            // made this because of f12 id changes
            if (!this.productsService.IsProductExisting(id))
            {
                return this.Error("Product is not existing.");
            }

            var viewModel = this.productsService.GetDetail(id);

            return this.View(viewModel);
        }

        public HttpResponse Delete(int id)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            // made this because of f12 id changes
            if (!this.productsService.IsProductExisting(id))
            {
                return this.Error("Product is not existing.");
            }

            this.productsService.Delete(id);

            return this.Redirect("/");
        }
    }
}