using BattleCards.Services;
using BattleCards.ViewModels.Cards;
using SUS.HTTP;
using SUS.MvcFramework;

namespace BattleCards.Controllers
{
    public class CardsController : Controller
    {
        private readonly ICardsService cardsService;

        public CardsController(ICardsService cardsService)
        {
            this.cardsService = cardsService;
        }

        [HttpGet]
        public HttpResponse Add()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Add(CreateCardInputModel input)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (string.IsNullOrEmpty(input.Name) ||
                input.Name.Length < 5 ||
                input.Name.Length > 15)
            {
                return this.Redirect("/Cards/Add");
            }

            if (string.IsNullOrEmpty(input.Image) ||
                string.IsNullOrEmpty(input.Keyword) ||
                input.Attack < 0 ||
                input.Health < 0)
            {
                return this.Redirect("/Cards/Add");
            }

            if (string.IsNullOrEmpty(input.Description) ||
                input.Description.Length > 200)
            {
                return this.Redirect("/Cards/Add");
            }

            this.cardsService.Create(input, this.GetUserId());

            return this.Redirect("/Cards/All");
        }

        [HttpGet]
        public HttpResponse All()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var viewModel = this.cardsService.GetAll();

            return this.View(viewModel);
        }

        [HttpGet]
        public HttpResponse Collection()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var viewModel = this.cardsService.GetUserCollection(this.GetUserId());

            return this.View(viewModel);
        }

        [HttpGet]
        public HttpResponse AddToCollection(int cardId)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var userId = this.GetUserId();

            if (this.cardsService.IsUserAlreadyOwnsCard(userId, cardId))
            {
                return this.Redirect("/Cards/All");
            }

            this.cardsService.AddCardToUserCollection(cardId, userId);

            return this.Redirect("/Cards/All");
        }

        [HttpGet]
        public HttpResponse RemoveFromCollection(int cardId)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var userId = this.GetUserId();

            this.cardsService.RemoveCardFromUserCollection(cardId, userId);

            return this.Redirect($"/Cards/Collection");
        }
    }
}
