using System;
using SUS.HTTP;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstMvcApp
{
    public class StartUp
    {
        public static async Task Main(string[] args)
        {
            IHttpServer server = new HttpServer();

            server.AddRoute("/", HomePage);
            server.AddRoute("/favicon.ico", Favicon);
            server.AddRoute("/about", About);
            server.AddRoute("/users/login", Login);

            await server.StartAsync(80);
        }

        private static HttpResponse HomePage(HttpRequest request)
        {
            var responseHtml = $"<h1>Welcome! {DateTime.Now}</h1>";

            var responseBodyBytes = Encoding.UTF8.GetBytes(responseHtml);

            var response = new HttpResponse("text/html", responseBodyBytes);

            return response;
        }

        private static HttpResponse Favicon(HttpRequest request)
        {
            var fileBytes = File.ReadAllBytes("wwwroot/favicon.ico");

            var response = new HttpResponse("image/vnd.microsoft.icon", fileBytes);

            return response;
        }

        private static HttpResponse About(HttpRequest request)
        {
            var responseHtml = $"<h1>About! {DateTime.Now}</h1>";

            var responseBodyBytes = Encoding.UTF8.GetBytes(responseHtml);

            var response = new HttpResponse("text/html", responseBodyBytes);

            return response;
        }

        private static HttpResponse Login(HttpRequest request)
        {
            var responseHtml = $"<h1>Login! {DateTime.Now}</h1>";

            var responseBodyBytes = Encoding.UTF8.GetBytes(responseHtml);

            var response = new HttpResponse("text/html", responseBodyBytes);

            return response;
        }
    }
}
