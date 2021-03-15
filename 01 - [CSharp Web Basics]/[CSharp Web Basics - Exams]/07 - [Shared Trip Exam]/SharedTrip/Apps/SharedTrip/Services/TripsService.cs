using System;
using System.Linq;
using SharedTrip.Data;
using System.Collections.Generic;
using SharedTrip.ViewModels.Trips;

namespace SharedTrip.Services
{
    public class TripsService : ITripsService
    {
        private readonly ApplicationDbContext db;

        public TripsService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void Add(AddTripInputModel input)
        {
            var trip = new Trip()
            {
                DepartureTime = DateTime.Parse(input.DepartureTime),
                Description = input.Description,
                EndPoint = input.EndPoint,
                ImagePath = input.ImagePath,
                Seats = input.Seats,
                StartPoint = input.StartPoint
            };

            this.db.Trips.Add(trip);

            this.db.SaveChanges();
        }

        public IEnumerable<GetAllTripsViewModel> GetAll()
        {
            var trips = this.db
                .Trips
                .Select(t => new GetAllTripsViewModel()
                {
                    Id = t.Id,
                    DepartureTime = t.DepartureTime,
                    AvailableSeats = t.Seats - t.UserTrips.Count,
                    EndPoint = t.EndPoint,
                    StartPoint = t.StartPoint
                })
                .ToList();

            return trips;
        }

        public GetTripDetailsViewModel GetDetails(string tripId)
        {
            var trip = this.db
                .Trips
                .Where(t => t.Id == tripId)
                .Select(t => new GetTripDetailsViewModel()
                {
                    Id = t.Id,
                    DepartureTime = t.DepartureTime,
                    EndPoint = t.EndPoint,
                    Description = t.Description,
                    ImageUrl = t.ImagePath,
                    Seats = t.Seats - t.UserTrips.Count,
                    StartPoint = t.StartPoint
                })
                .FirstOrDefault();

            return trip;
        }

        public bool AddUserToTrip(string userId, string tripId)
        {
            if (this.db.UserTrips
                    .Any(ut => ut.UserId == userId &&
                                          ut.TripId == tripId) ||
                this.db.Trips
                    .Where(t => t.Id == tripId)
                    .Select(t => t.Seats - t.UserTrips.Count)
                    .FirstOrDefault() == 0)
            {
                return false;
            }

            this.db.UserTrips.Add(new UserTrip()
            {
                TripId = tripId,
                UserId = userId
            });

            this.db.SaveChanges();

            return true;
        }
    }
}