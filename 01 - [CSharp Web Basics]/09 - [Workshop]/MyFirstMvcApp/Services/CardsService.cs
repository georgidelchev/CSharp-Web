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

        public int AddCard(AddCardInputModel model)
        {
            var card = new Card()
            {
                Attack = model.Attack,
                Description = model.Description,
                Health = model.Health,
                ImageUrl = model.Image,
                Keyword = model.Keyword,
                Name = model.Name
            };

            this.db.Cards.Add(card);

            this.db.SaveChanges();

            return card.Id;
        }

        public IEnumerable<CardViewModel> GetAll()
        {
            return this.db
                .Cards
                .Select(c => new CardViewModel()
                {
                    Id = c.Id,
                    Attack = c.Attack,
                    Health = c.Health,
                    Name = c.Name,
                    ImageUrl = c.ImageUrl,
                    Type = c.Keyword,
                    Description = c.Description
                })
                .ToList();
        }

        public IEnumerable<CardViewModel> GetByUserId(string userId)
        {
            return this.db
                .UserCards
                .Where(uc => uc.UserId == userId)
                .Select(c => new CardViewModel()
                {
                    Id = c.CardId,
                    Name = c.Card.Name,
                    Description = c.Card.Description,
                    Health = c.Card.Health,
                    Attack = c.Card.Attack,
                    ImageUrl = c.Card.ImageUrl,
                    Type = c.Card.Keyword
                })
                .ToList();
        }

        public void AddCardToUserCollection(string userId, int cardId)
        {
            if (this.db.UserCards.Any(uc => uc.UserId == userId &&
                                          uc.CardId == cardId))
            {
                return;
            }

            this.db.UserCards.Add(new UserCard()
            {
                CardId = cardId,
                UserId = userId
            });

            this.db.SaveChanges();
        }

        public void RemoveCardFromUserCollection(string userId, int cardId)
        {
            var cardToRemove = this.db
                .UserCards
                .FirstOrDefault(uc => uc.UserId == userId &&
                                      uc.CardId == cardId);

            if (cardToRemove == null)
            {
                return;
            }

            this.db
                .UserCards
                .Remove(cardToRemove);

            db.SaveChanges();
        }
    }
}