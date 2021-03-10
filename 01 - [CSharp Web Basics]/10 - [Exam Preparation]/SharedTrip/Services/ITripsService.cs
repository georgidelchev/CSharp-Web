using System;
using System.Collections.Generic;
using SharedTrip.ViewModels.Trips;

namespace SharedTrip.Services
{
    public interface ITripsService
    {
        void Add(AddTripInputModel input);

        IEnumerable<TripViewModel> GetAll();

        TripDetailsViewModel GetDetails(string tripId);

        bool HasAvailableSeats( string tripId);

        bool AddUserToTrip(string userId, string tripId);
    }
}