using System.Collections.Generic;
using System.Linq;
using BattleCards.Data;
using BattleCards.Services;

namespace BattleCards.ViewModels.Cards
{
    public class CardsService : ICardsService
    {
        private readonly ApplicationDbContext dbContext;

        public CardsService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Create(CreateCardInputModel input, string userId)
        {
            var card = new Card()
            {
                Attack = input.Attack,
                Description = input.Description,
                Health = input.Health,
                ImageUrl = input.Image,
                Keyword = input.Keyword,
                Name = input.Name,
            };

            this.dbContext.Add(card);
            this.dbContext.SaveChanges();

            this.dbContext
                .UserCards
                .Add(new UserCard()
                {
                    UserId = userId,
                    CardId = card.Id,
                });

            this.dbContext.SaveChanges();
        }

        public IEnumerable<GetAllCardsViewModel> GetAll()
            => this.dbContext
                .Cards
                .Select(c => new GetAllCardsViewModel()
                {
                    Attack = c.Attack,
                    Keyword = c.Keyword,
                    Health = c.Health,
                    Id = c.Id,
                    Name = c.Name,
                    ImageUrl = c.ImageUrl,
                    Description = c.Description
                })
                .ToList();

        public IEnumerable<GetAllCardsViewModel> GetUserCollection(string id)
            => this.dbContext
                .UserCards
                .Where(uc => uc.UserId == id)
                .Select(uc => new GetAllCardsViewModel()
                {
                    Attack = uc.Card.Attack,
                    Keyword = uc.Card.Description,
                    Health = uc.Card.Health,
                    Id = uc.Card.Id,
                    ImageUrl = uc.Card.ImageUrl,
                    Name = uc.Card.Name,
                    Description = uc.Card.Description
                })
                .ToList();

        public void AddCardToUserCollection(int cardId, string userId)
        {
            var userCard = new UserCard()
            {
                CardId = cardId,
                UserId = userId,
            };

            this.dbContext.Add(userCard);
            this.dbContext.SaveChanges();
        }

        public void RemoveCardFromUserCollection(int cardId, string userId)
        {
            var user = this.dbContext
                .Users
                .FirstOrDefault(u => u.Id == userId);

            var card = this.dbContext
                .UserCards
                .FirstOrDefault(uc => uc.UserId == userId &&
                                    uc.CardId == cardId);

            this.dbContext.UserCards.Remove(card);
            this.dbContext.SaveChanges();
        }

        public bool IsUserAlreadyOwnsCard(string userId, int cardId)
            => this.dbContext
                .UserCards
                .Any(uc => uc.CardId == cardId &&
                           uc.UserId == userId);
    }
}
