using BattleCards.Data;
using BattleCards.ViewModels;
using BattleCards2.Data;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace BattleCards.Services.Cards
{
    public class CardService : ICardService
    {
        private readonly ApplicationDbContext db;

        public CardService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void AddCardToCollection(int cardId, string userId)
        {
            if (this.db.UsersCards.Any(x => x.CardId == cardId && x.UserId == userId))
            {
                return;
            }

            this.db.UsersCards.Add(new UserCard
            {
                CardId = cardId,
                UserId = userId
            });

            this.db.SaveChanges();
        }

        public int Create(CardInputModel model)
        {
            var card = new Card
            {
                Name = model.Name,
                ImageUrl = model.Image,
                Keyword = model.Keyword,
                Attack = model.Attack,
                Health = model.Health,
                Description = model.Description
            };

            this.db.Cards.Add(card);
            this.db.SaveChanges();
            return card.Id;
        }

        public IEnumerable<CardViewModel> GetCollection(string userId)
        {
            return this.db.UsersCards.Where(x => x.UserId == userId)
                .Select(x => new CardViewModel
                {
                    Id = x.CardId,
                    Name = x.Card.Name,
                    Image = x.Card.ImageUrl,
                    Keyword = x.Card.Keyword,
                    Attack = x.Card.Attack,
                    Health = x.Card.Health,
                    Description = x.Card.Description
                }).ToList();
        }

        public IEnumerable<CardViewModel> ListAll()
        {
            var cards = this.db.Cards.Select(x => new CardViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Image = x.ImageUrl,
                Keyword = x.Keyword,
                Attack = x.Attack,
                Health = x.Health,
                Description = x.Description
            }).ToList();

            return cards;
        }

        public void RemoveCard(int cardId, string userId)
        {
            var userCard = this.db.UsersCards.FirstOrDefault(x => x.CardId == cardId && x.UserId == userId);
            if (userCard == null)
            {
                return;
            }

            this.db.Remove(userCard);
            this.db.SaveChanges();
        }
    }
}
