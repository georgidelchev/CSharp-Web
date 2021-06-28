using System.Linq;
using BattleCards.Data;
using System.Collections.Generic;
using BattleCards.ViewModels.Cards;

namespace BattleCards.Services
{
    public class CardsService : ICardsService
    {
        private readonly ApplicationDbContext db;

        public CardsService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void Add(AddCardInputModel input, string userId)
        {
            var card = new Card()
            {
                Description = input.Description,
                Attack = input.Attack,
                Health = input.Health,
                ImageUrl = input.Image,
                Keyword = input.Keyword,
                Name = input.Name
            };

            this.db.Cards.Add(card);

            this.db.SaveChanges();

            this.db.UserCards.Add(new UserCard()
            {
                CardId = card.Id,
                UserId = userId
            });

            this.db.SaveChanges();
        }

        public bool AddCardToUser(int cardId, string userId)
        {
            if (this.db.UserCards.Any(uc => uc.UserId == userId &&
                                          uc.CardId == cardId))
            {
                return false;
            }

            this.db.UserCards.Add(new UserCard()
            {
                CardId = cardId,
                UserId = userId
            });

            this.db.SaveChanges();

            return true;
        }

        public bool RemoveCardFromUser(int cardId, string userId)
        {
            var card = this.db.UserCards
                .FirstOrDefault(uc => uc.UserId == userId &&
                                      uc.CardId == cardId);

            if (card == null)
            {
                return false;
            }

            this.db.UserCards.Remove(card);

            this.db.SaveChanges();

            return true;
        }

        public IEnumerable<DisplayAllCardsViewModel> GetAll()
        {
            var cards = this.db
                .Cards
                .Select(c => new DisplayAllCardsViewModel()
                {
                    Id = c.Id,
                    Attack = c.Attack,
                    Health = c.Health,
                    ImageUrl = c.ImageUrl,
                    Description = c.Description,
                    Keyword = c.Keyword,
                    Name = c.Name
                })
                .ToList();

            return cards;
        }

        public IEnumerable<GetUserCardCollectionViewModel> GetUserCollection(string userId)
        {
            var collection = this.db
                .UserCards
                .Where(uc => uc.UserId == userId)
                .Select(uc => new GetUserCardCollectionViewModel()
                {
                    Attack = uc.Card.Attack,
                    Health = uc.Card.Health,
                    Id = uc.Card.Id,
                    Image = uc.Card.ImageUrl,
                    Description = uc.Card.Description,
                    Keyword = uc.Card.Keyword,
                    Name = uc.Card.Name
                })
                .ToList();

            return collection;
        }
    }
}