using SharedTrip.Models;
using SharedTrip.ViewModels.Trips;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace SharedTrip.Services.Trips
{
    public class TripService : ITripService
    {
        private readonly ApplicationDbContext db;

        public TripService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public bool AddToTrip(string tripId, string userId)
        {
            var userTrip = this.db.UsersTrips.FirstOrDefault(x => x.UserId == userId && x.TripId == tripId);
            if (userTrip != null)
            {
                return false;
            }

            var trip = this.db.Trips.FirstOrDefault(x => x.Id == tripId);

            if (trip.Seats == 0)
            {
                return true;
            }
            
            trip.Seats--;
            this.db.Trips.Update(trip);
            this.db.UsersTrips.Add(new UserTrip
            {
                UserId = userId,
                TripId = tripId
            });
            this.db.SaveChanges();

            return true;
        }

        public void Create(TripInputModel model)
        {
            var trip = new Trip
            {
                StartPoint = model.StartPoint,
                EndPoint = model.EndPoint,
                DepartureTime = DateTime.Parse(model.DepartureTime, CultureInfo.InvariantCulture),
                ImagePath = model.ImagePath,
                Seats = model.Seats,
                Description = model.Description
            };

            this.db.Trips.Add(trip);
            this.db.SaveChanges();
        }

        public IEnumerable<TripViewModel> GetAll()
        {
            return this.db.Trips.Select(x => new TripViewModel
            {
                Id = x.Id,
                StartPoint = x.StartPoint,
                EndPoint = x.EndPoint,
                DepartureTime = x.DepartureTime,
                Seats = x.Seats,
                Description = x.Description
            }).ToList();
        }

        public TripViewModel GetById(string Id)
        {
            var trip = this.db.Trips.FirstOrDefault(x => x.Id == Id);

            return new TripViewModel
            {
                Id = trip.Id,
                StartPoint = trip.StartPoint,
                EndPoint = trip.EndPoint,
                DepartureTime = trip.DepartureTime,
                Seats = trip.Seats,
                Description = trip.Description,
                ImagePath = trip.ImagePath
            };
        }
    }
}
