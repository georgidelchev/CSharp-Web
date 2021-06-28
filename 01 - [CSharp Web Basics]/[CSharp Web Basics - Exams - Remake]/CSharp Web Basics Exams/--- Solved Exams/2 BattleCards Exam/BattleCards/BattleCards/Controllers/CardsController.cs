using SUS.HTTP;
using SUS.MvcFramework;
using BattleCards.Services;
using BattleCards.ViewModels.Cards;

namespace BattleCards.Controllers
{
    public class CardsController : Controller
    {
        private readonly ICardsService cardsService;

        public CardsController(ICardsService cardsService)
        {
            this.cardsService = cardsService;
        }

        public HttpResponse All()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var viewModel = this.cardsService.GetAll();

            return this.View(viewModel);
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
        public HttpResponse Add(AddCardInputModel input)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (string.IsNullOrEmpty(input.Name) ||
                input.Name.Length < 5 ||
                input.Name.Length > 15)
            {
                return this.Error("Name should be between 5 and 15 characters.");
            }

            if (string.IsNullOrEmpty(input.Image))
            {
                return this.Error("Image is required.");
            }

            if (string.IsNullOrEmpty(input.Keyword))
            {
                return this.Error("Keyword is required.");
            }

            if (input.Attack < 0)
            {
                return this.Error("Attack cannot be less than 0.");
            }

            if (input.Health < 0)
            {
                return this.Error("Health cannot be less than 0.");
            }

            if (string.IsNullOrEmpty(input.Description) ||
                input.Description.Length > 200)
            {
                return this.Error("Max size of the description is 200 characters and its required.");
            }

            var userId = this.GetUserId();

            this.cardsService.Add(input, userId);

            return this.Redirect("/Cards/All");
        }

        public HttpResponse AddToCollection(int cardId)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var userId = this.GetUserId();

            var result = this.cardsService.AddCardToUser(cardId, userId);

            if (result == false)
            {
                return this.Error("You already have this card.");
            }

            return this.Redirect("/Cards/All");
        }

        public HttpResponse RemoveFromCollection(int cardId)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var userId = this.GetUserId();

            var result = this.cardsService.RemoveCardFromUser(cardId, userId);

            if (result == false)
            {
                return this.Error("You don't have this card in your collection");
            }

            return this.Redirect("/Cards/Collection");
        }

        public HttpResponse Collection()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var userId = this.GetUserId();

            var viewModel = this.cardsService.GetUserCollection(userId);

            return this.View(viewModel);
        }
    }
}