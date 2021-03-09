using System;
using SUS.HTTP;
using SUS.MvcFramework;
using BattleCards.Services;
using BattleCards.ViewModels.Cards;

namespace BattleCards.Controllers
{
    public class CardsController : Controller
    {
        private readonly ICardsService cardService;

        public CardsController(ICardsService cardService)
        {
            this.cardService = cardService;
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
        public HttpResponse Add(AddCardInputModel model)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }

            if (string.IsNullOrEmpty(model.Name) ||
                model.Name.Length < 5 ||
                model.Name.Length > 15)
            {
                return this.Error("Name should be between 5 and 15 characters long.");
            }

            if (string.IsNullOrWhiteSpace(model.Image))
            {
                return this.Error("The image is required.");
            }

            if (!Uri.TryCreate(model.Image, UriKind.Absolute, out _))
            {
                return this.Error("Image url should be valid.");
            }

            if (string.IsNullOrWhiteSpace(model.Keyword))
            {
                return this.Error("Keyword is required");
            }

            if (model.Attack < 0)
            {
                return this.Error("Attack should be non-negative integer.");
            }

            if (model.Health < 0)
            {
                return this.Error("Health should be non-negative integer.");
            }

            if (string.IsNullOrWhiteSpace(model.Description) || model.Description.Length > 200)
            {
                return this.Error("Description is required and its length should be at most 200 characters");
            }

            var cardId = this.cardService.AddCard(model);

            var userId = this.GetUserId();

            this.cardService.AddCardToUserCollection(userId, cardId);

            return this.Redirect("/Cards/All");
        }

        public HttpResponse All()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            return this.View(this.cardService.GetAll());
        }

        public HttpResponse Collection()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var userId = this.GetUserId();

            return this.View(this.cardService.GetByUserId(userId));
        }

        public HttpResponse AddToCollection(int cardId)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var userId = this.GetUserId();

            this.cardService.AddCardToUserCollection(userId, cardId);

            return this.Redirect("/Cards/All");
        }

        public HttpResponse RemoveFromCollection(int cardId)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var userId = this.GetUserId();

            this.cardService.RemoveCardFromUserCollection(userId, cardId);

            return this.Redirect("/Cards/Collection");
        }
    }
}