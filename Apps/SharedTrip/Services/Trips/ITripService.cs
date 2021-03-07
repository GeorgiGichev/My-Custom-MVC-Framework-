using SharedTrip.ViewModels.Trips;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedTrip.Services.Trips
{
    public interface ITripService
    {
        IEnumerable<TripViewModel> GetAll();

        void Create(TripInputModel model);

        TripViewModel GetById(string Id);

        bool AddToTrip(string tripId, string userId);
    }
}
