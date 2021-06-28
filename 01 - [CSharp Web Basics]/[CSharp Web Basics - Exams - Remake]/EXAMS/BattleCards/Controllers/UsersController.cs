using SUS.HTTP;
using SUS.MvcFramework;
using BattleCards.Services;
using BattleCards.ViewModels.Users;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace BattleCards.Controllers
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
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Login(LoginUserInputModel input)
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }

            var userId = this.usersService.GetUserId(input.Username, input.Password);

            if (userId == null)
            {
                return this.Redirect("/Users/Login");
            }

            this.SignIn(userId);

            return this.Redirect("/Cards/All");
        }

        [HttpGet]
        public HttpResponse Register()
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Register(RegisterUserInputModel input)
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }

            if (string.IsNullOrEmpty(input.Username) ||
                input.Username.Length < 5 ||
                input.Username.Length > 20 ||
                !Regex.IsMatch(input.Username, @"^[a-zA-Z0-9\.]+$") ||
                this.usersService.IsUsernameTaken(input.Username))
            {
                return this.Redirect("/Users/Register");
            }

            if (string.IsNullOrEmpty(input.Password) ||
                input.Password.Length < 6 ||
                input.Password.Length > 20 ||
                input.Password != input.ConfirmPassword)
            {
                return this.Redirect("/Users/Register");
            }

            if (string.IsNullOrEmpty(input.Email) ||
                !new EmailAddressAttribute().IsValid(input.Email) ||
                this.usersService.IsEmailTaken(input.Email))
            {
                return this.Redirect("/Users/Register");
            }

            this.usersService.Register(input);

            return this.Redirect("/Users/Login");
        }

        [HttpGet("/Logout")]
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
