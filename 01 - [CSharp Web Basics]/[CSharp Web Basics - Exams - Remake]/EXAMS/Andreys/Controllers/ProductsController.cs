using System;
using SUS.HTTP;
using Andreys.Services;
using SUS.MvcFramework;
using Andreys.Data.Enumerations;
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

        [HttpGet]
        public HttpResponse Add()
            => !this.IsUserSignedIn() ? this.Redirect("/Users/Login") : this.View();

        [HttpPost]
        public HttpResponse Add(AddProductInputModel input)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (input.Name.Length < 4 ||
                input.Name.Length > 20 ||
                string.IsNullOrEmpty(input.Name))
            {
                return this.Redirect("/Products/Add");
            }

            if (input.Description.Length > 10)
            {
                return this.Redirect("/Products/Add");
            }

            if (!Enum.IsDefined(typeof(Category), input.Category) ||
                !Enum.IsDefined(typeof(Gender), input.Gender))
            {
                return this.Redirect("/Products/Add");
            }

            this.productsService.Add(input);

            return this.Redirect("/");
        }

        [HttpGet]
        public HttpResponse Details(int id)
            => !this.IsUserSignedIn() ? this.Redirect("/Users/Login") : this.View(this.productsService.GetDetails(id));

        [HttpGet]
        public HttpResponse Delete(int id)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            this.productsService.Delete(id);

            return this.Redirect("/");
        }
    }
}
