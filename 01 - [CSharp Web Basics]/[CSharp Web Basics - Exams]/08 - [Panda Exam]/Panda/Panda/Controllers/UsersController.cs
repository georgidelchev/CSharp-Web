﻿using SUS.HTTP;
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
        public HttpResponse Register(RegisterUserInputModel input)
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }

            if (string.IsNullOrEmpty(input.Username) ||
                input.Username.Length < 5 ||
                input.Username.Length > 20)
            {
                //return this.Error("Username should be between 5 and 20 characters.");

                return this.Redirect("/Users/Register");
            }

            if (!Regex.IsMatch(input.Username, @"^[a-zA-Z0-9\.]+$"))
            {
                //return this.Error("Invalid username.");

                return this.Redirect("/Users/Register");
            }

            if (string.IsNullOrEmpty(input.Email) ||
                !new EmailAddressAttribute().IsValid(input.Email) ||
                input.Email.Length < 5 ||
                input.Email.Length > 20)
            {
                //return this.Error("Email is not valid.");

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
                //return this.Error("Passwords do not match.");

                return this.Redirect("/Users/Register");
            }

            if (!this.usersService.IsEmailAvailable(input.Email))
            {
                //return this.Error("Email already taken");

                return this.Redirect("/Users/Register");
            }

            if (!this.usersService.IsUsernameAvailable(input.Username))
            {
                //return this.Error("Username already taken");

                return this.Redirect("/Users/Register");
            }

            this.usersService.Create(input);

            return this.Redirect("/Users/Login");
        }

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