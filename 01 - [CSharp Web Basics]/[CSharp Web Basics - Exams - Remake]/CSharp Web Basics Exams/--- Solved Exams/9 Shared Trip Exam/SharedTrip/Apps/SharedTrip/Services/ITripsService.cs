using System.Collections.Generic;
using SharedTrip.ViewModels.Trips;

namespace SharedTrip.Services
{
    public interface ITripsService
    {
        void Add(AddTripInputModel input);

        IEnumerable<GetAllTripsViewModel> GetAll();

        GetTripDetailsViewModel GetDetails(string tripId);

        bool AddUserToTrip(string userId, string tripId);
    }
}