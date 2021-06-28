using System.Collections.Generic;
using BattleCards.ViewModels.Cards;

namespace BattleCards.Services
{
    public interface ICardsService
    {
        void Create(CreateCardInputModel input,string userId);

        IEnumerable<GetAllCardsViewModel> GetAll();

        IEnumerable<GetAllCardsViewModel> GetUserCollection(string id);

        void AddCardToUserCollection(int cardId, string userId);

        void RemoveCardFromUserCollection(int cardId, string userId);

        bool IsUserAlreadyOwnsCard(string userId, int cardId);
    }
}
