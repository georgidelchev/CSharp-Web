using System;
using SUS.HTTP;
using System.Text;
using SUS.MvcFramework;

namespace MyFirstMvcApp.Controllers
{
    public class HomeController : Controller
    {
        public HttpResponse Index(HttpRequest request)
        {
            return this.View();
        }
    }
}