using BattleCards.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BattleCards.Services.Cards
{
    public interface ICardService
    {
        int Create(CardInputModel model);

        IEnumerable<CardViewModel> ListAll();

        void AddCardToCollection(int id, string userId);

        IEnumerable<CardViewModel> GetCollection(string userId);

        void RemoveCard(int cardId, string userId);
    }
}
