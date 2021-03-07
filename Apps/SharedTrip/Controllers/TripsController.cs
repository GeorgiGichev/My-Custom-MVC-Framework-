using Microsoft.EntityFrameworkCore.Query.Internal;
using SharedTrip.Services.Trips;
using SharedTrip.ViewModels.Trips;
using SUS.HTTP;
using SUS.MvcFramework;
using System;
using System.Globalization;

namespace SharedTrip.Controllers
{
    public class TripsController : Controller
    {
        private readonly ITripService tripService;

        public TripsController(ITripService tripService)
        {
            this.tripService = tripService;
        }

        public HttpResponse All()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var trips = this.tripService.GetAll();
            return this.View(trips);
        }

        public HttpResponse Add()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Add(TripInputModel model)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (string.IsNullOrWhiteSpace(model.StartPoint))
            {
                return this.Error("Starting point is required.");
            }

            if (string.IsNullOrWhiteSpace(model.EndPoint))
            {
                return this.Error("Ending point is required.");
            }

            if (!DateTime.TryParseExact(
                model.DepartureTime,
                "dd.MM.yyyy HH:mm",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out _))
            {
                return this.Error("Departure time should be in format \"dd.MM.yyyy HH:mm\"");
            }

            if (model.Seats < 2 || model.Seats > 6)
            {
                return this.Error("Seats should be between 2 and 6.");
            }

            if (string.IsNullOrWhiteSpace(model.Description) || model.Description.Length > 80)
            {
                return this.Error("Description is required and should be to 80 characters");
            }

            this.tripService.Create(model);
            return this.Redirect("/Trips/All");
        }

        public HttpResponse Details(string tripId)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var trip = this.tripService.GetById(tripId);
            return this.View(trip);
        }

        public HttpResponse AddUserToTrip(string tripId)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var isUserAdded = this.tripService.AddToTrip(tripId, this.GetUserId());

            if (isUserAdded)
            {
                return this.Redirect("/Trips/All");
            }

            return this.Redirect($"/Trips/Details?tripId={tripId}");
        }
    }
}
