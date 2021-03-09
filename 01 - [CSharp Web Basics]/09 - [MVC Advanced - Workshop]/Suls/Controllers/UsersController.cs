using SUS.HTTP;
using Suls.Services;
using SUS.MvcFramework;
using Suls.ViewModels.Users;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace Suls.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public HttpResponse Login()
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Login(string username, string password)
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }

            var userId = this.usersService.GetUserId(username, password);

            if (userId == null)
            {
                return this.Error("Invalid username or password");
            }

            this.SignIn(userId);

            return this.Redirect("/");
        }

        public HttpResponse Register()
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Register(RegisterInputModel model)
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }

            if (string.IsNullOrWhiteSpace(model.Username) ||
                model.Username.Length < 5 ||
                model.Username.Length > 20)
            {
                return this.Error("Username should be between 5 and 20 characters.");
            }

            if (!this.usersService.IsUsernameAvailable(model.Username))
            {
                return this.Error("Username already taken.");
            }

            if (string.IsNullOrWhiteSpace(model.Email) ||
                !new EmailAddressAttribute().IsValid(model.Email))
            {
                return this.Error("Email should be valid.");
            }

            if (!this.usersService.IsEmailAvailable(model.Email))
            {
                return this.Error("Email already taken.");
            }

            if (!Regex.IsMatch(model.Username, @"^[a-zA-Z0-9\.]+$"))
            {
                return this.Error("The username is not valid.");
            }

            if (string.IsNullOrEmpty(model.Password) ||
                model.Password.Length < 6 ||
                model.Password.Length > 20)
            {
                return this.Error("Password should be between 6 and 20 characters");
            }

            if (model.Password != model.ConfirmPassword)
            {
                return this.Error("Passwords should be the same.");
            }

            this.usersService.CreateUser(model.Username, model.Email, model.Password);

            return this.Redirect("/Users/Login");
        }

        public HttpResponse Logout()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }

            this.SignOut();

            return this.Redirect("/");
        }
    }
}