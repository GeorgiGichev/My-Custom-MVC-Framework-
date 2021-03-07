using BattleCards.Services.Cards;
using BattleCards.ViewModels;
using SUS.HTTP;
using SUS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BattleCards.Controllers
{
    public class CardsController : Controller
    {
        private readonly ICardService cardService;

        public CardsController(ICardService cardService)
        {
            this.cardService = cardService;
        }

        public HttpResponse Add()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Home/Index");
            }
            return this.View();
        }
        [HttpPost]
        public HttpResponse Add(CardInputModel model)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Home/Index");
            }

            if (model.Name.Length < 5 || model.Name.Length > 15 || string.IsNullOrWhiteSpace(model.Name))
            {
                return this.Error("Name lenght should be between 5 and 15 characters.");
            }

            if (string.IsNullOrWhiteSpace(model.Image))
            {
                return this.Error("Url is required.");
            }

            if (string.IsNullOrWhiteSpace(model.Keyword))
            {
                return this.Error("Keyword is required.");
            }

            if (model.Attack < 0)
            {
                return this.Error("Attack can't bew negative number.");
            }

            if (string.IsNullOrWhiteSpace(model.Attack.ToString()))
            {
                return this.Error("Attack is required");
            }

            if (model.Health < 0)
            {
                return this.Error("Health can't bew negative number.");
            }

            if (string.IsNullOrWhiteSpace(model.Health.ToString()))
            {
                return this.Error("Healt is required");
            }

            if (string.IsNullOrWhiteSpace(model.Description) || model.Description.Length > 200)
            {
                return this.Error("Description is required and has max lenght 200 characters.");
            }
            var cardId = this.cardService.Create(model);
            this.cardService.AddCardToCollection(cardId, this.GetUserId());
            return this.Redirect("/Cards/All");
        }

        public HttpResponse All()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Home/Index");
            }

            var model = this.cardService.ListAll();
            return this.View(model);
        }

        public HttpResponse Collection()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Home/Index");
            }

            var cards = this.cardService.GetCollection(this.GetUserId());
            return this.View(cards);
        }

        public HttpResponse AddToCollection(int cardId)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Home/Index");
            }

            this.cardService.AddCardToCollection(cardId, this.GetUserId());
            return this.Redirect("/Cards/All");
        }

        public HttpResponse RemoveFromCollection(int cardId)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Home/Index");
            }

            this.cardService.RemoveCard(cardId, this.GetUserId());
            return this.Redirect("/Cards/Collection");
        }
    }
}
