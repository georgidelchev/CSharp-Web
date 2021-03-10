using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SharedTrip.Data;
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
                DepartureTime = DateTime.ParseExact(input.DepartureTime, "dd.MM.yyyy HH:ss", CultureInfo.InvariantCulture),
                Description = input.Description,
                EndPoint = input.EndPoint,
                Seats = input.Seats,
                ImagePath = input.ImagePath,
                StartPoint = input.StartPoint
            };

            this.db.Trips.Add(trip);

            this.db.SaveChanges();
        }

        public IEnumerable<TripViewModel> GetAll()
        {
            var trips = this.db
                .Trips
                .Select(t => new TripViewModel()
                {
                    AvailableSeats = t.Seats - t.UserTrips.Count(),
                    DepartureTime = t.DepartureTime,
                    StartPoint = t.StartPoint,
                    EndPoint = t.EndPoint,
                    Id = t.Id
                })
                .ToList();

            return trips;
        }

        public TripDetailsViewModel GetDetails(string tripId)
        {
            return this.db
                .Trips
                .Where(t => t.Id == tripId)
                .Select(t => new TripDetailsViewModel()
                {
                    Id = t.Id,
                    Description = t.Description,
                    StartPoint = t.StartPoint,
                    EndPoint = t.EndPoint,
                    DepartureTime = t.DepartureTime,
                    AvailableSeats = t.Seats - t.UserTrips.Count(),
                    ImagePath = t.ImagePath
                })
                .FirstOrDefault();
        }

        public bool HasAvailableSeats(string tripId)
        {
            var trip = this.db
                .Trips
                .Where(t => t.Id == tripId)
                .Select(t => new
                {
                    t.Seats,
                    TakenSeats = t.UserTrips.Count()
                })
                .FirstOrDefault();

            var availableSeats = trip.Seats - trip.TakenSeats;

            return availableSeats > 0;
        }

        public bool AddUserToTrip(string userId, string tripId)
        {
            var userInTrip = this.db
                .UserTrips
                .Any(u => u.UserId == userId &&
                            u.TripId == tripId);

            if (userInTrip)
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