using System.Collections.Generic;
using BattleCards.ViewModels.Cards;

namespace BattleCards.Services
{
    public interface ICardsService
    {
        void Add(AddCardInputModel input, string userId);

        bool AddCardToUser(int cardId, string userId);

        bool RemoveCardFromUser(int cardId, string userId);

        IEnumerable<DisplayAllCardsViewModel> GetAll();

        IEnumerable<GetUserCardCollectionViewModel> GetUserCollection(string userId);
    }
}