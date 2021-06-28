﻿using SUS.HTTP;
using Andreys.Services;
using SUS.MvcFramework;
using Andreys.ViewModels.Users;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace Andreys.Controllers
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
                //return this.Error("Invalid username or password.");

                return this.Redirect("/Users/Login");
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
        public HttpResponse Register(RegisterInputModel input)
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }

            if (string.IsNullOrEmpty(input.Username) ||
                input.Username.Length < 4 ||
                input.Username.Length > 10)
            {
                //return this.Error("Username should be between 4 and 10 characters.");

                return this.Redirect("/Users/Register");
            }

            if (!Regex.IsMatch(input.Username, @"^[a-zA-Z0-9\.]+$"))
            {
                //return this.Error("Invalid username.");

                return this.Redirect("/Users/Register");
            }

            if (!this.usersService.IsUsernameAvailable(input.Username))
            {
                //return this.Error("Username already taken.");

                return this.Redirect("/Users/Register");
            }

            if (string.IsNullOrEmpty(input.Password) ||
                input.Password.Length < 6 ||
                input.Password.Length > 20)
            {
                //return this.Error("Password should be between 6 and 20 characters");

                return this.Redirect("/Users/Register");
            }

            if (input.Password != input.ConfirmPassword)
            {
                //return this.Error("Passwords doesn't match.");

                return this.Redirect("/Users/Register");
            }

            if (string.IsNullOrEmpty(input.Email) ||
                !new EmailAddressAttribute().IsValid(input.Email))
            {
                //return this.Error("Email is invalid.");

                return this.Redirect("/Users/Register");
            }

            if (!this.usersService.IsEmailAvailable(input.Email))
            {
                //return this.Error("Email already taken.");

                return this.Redirect("/Users/Register");
            }

            this.usersService.Create(input.Username, input.Email, input.Password);

            return this.Redirect("/Users/Login");
        }

        public HttpResponse Logout()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }

            this.SignOut();

            return this.Redirect("/Home/Index");
        }
    }
}