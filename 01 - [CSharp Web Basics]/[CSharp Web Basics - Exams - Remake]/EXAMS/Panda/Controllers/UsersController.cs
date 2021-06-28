using SUS.HTTP;
using Panda.Services;
using SUS.MvcFramework;
using Panda.ViewModels.Users;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace Panda.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        [HttpGet]
        public HttpResponse Login()
            => this.IsUserSignedIn() ? this.Redirect("/") : this.View();

        [HttpPost]
        public HttpResponse Login(LoginUserInputModel input)
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }

            var userId = this.usersService
                .GetUserId(input.Username, input.Password);

            if (userId == null)
            {
                return this.Redirect("/Users/Login");
            }

            this.SignIn(userId);

            return this.Redirect("/");
        }

        [HttpGet]
        public HttpResponse Register()
            => this.IsUserSignedIn() ? this.Redirect("/") : this.View();

        [HttpPost]
        public HttpResponse Register(RegisterUserInputModel input)
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }

            if (input.Username.Length < 5 ||
                input.Username.Length > 20 ||
                string.IsNullOrEmpty(input.Username) ||
                !Regex.IsMatch(input.Username, @"^[a-zA-Z0-9\.]+$") ||
                this.usersService.IsUsernameTaken(input.Username))
            {
                return this.Redirect("/Users/Register");
            }

            if (string.IsNullOrEmpty(input.Email) ||
                !new EmailAddressAttribute().IsValid(input.Email) ||
                input.Email.Length < 5 ||
                input.Email.Length > 20 ||
                this.usersService.IsEmailTaken(input.Email))
            {
                return this.Redirect("/Users/Register");
            }

            if (string.IsNullOrEmpty(input.Password) ||
                input.Password != input.ConfirmPassword)
            {
                return this.Redirect("/Users/Register");
            }

            this.usersService.Register(input);

            return this.Redirect("/Users/Login");
        }

        [HttpGet]
        public HttpResponse Logout()
        {
            if (this.IsUserSignedIn())
            {
                this.SignOut();
            }

            return this.Redirect("/");
        }
    }
}
