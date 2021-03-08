using SUS.HTTP;
using System.Linq;
using SUS.MvcFramework;
using BattleCards.Data;
using BattleCards.ViewModels;
using BattleCards.ViewModels.Cards;

namespace BattleCards.Controllers
{
    public class CardsController : Controller
    {
        private readonly ApplicationDbContext db;

        public CardsController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public HttpResponse Add()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            return this.View();
        }

        [HttpPost("/Cards/Add")]
        public HttpResponse DoAdd(int attack, string description, int health, string image, string name, string keyword)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }

            if (this.Request.FormData["name"].Length < 5)
            {
                return this.Error("Name should be at least 5 characters long.");
            }

            var card = new Card()
            {
                Attack = attack,
                Description = description,
                Health = health,
                ImageUrl = image,
                Keyword = keyword,
                Name = name
            };

            this.db.Cards.Add(card);

            this.db.SaveChanges();

            return this.Redirect("/Cards/All");
        }

        public HttpResponse All()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var viewModel = this.db
                .Cards
                .Select(c => new CardViewModel()
                {
                    Attack = c.Attack,
                    Health = c.Health,
                    Name = c.Name,
                    ImageUrl = c.ImageUrl,
                    Type = c.Keyword,
                    Description = c.Description
                })
                .ToList();

            return this.View(viewModel);
        }

        public HttpResponse Collection()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var list = this.db.Cards.ToList();

            return this.View(list);
        }
    }
}