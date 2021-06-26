using System;
using System.Linq;
using SharedTrip.Data;
using System.Collections.Generic;
using System.Globalization;
using SharedTrip.ViewModels.Trips;

namespace SharedTrip.Services
{
    public class TripsService : ITripsService
    {
        private readonly ApplicationDbContext dbContext;

        public TripsService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Add(AddTripInputModel input)
        {
            var trip = new Trip()
            {
                DepartureTime = DateTime.ParseExact(input.DepartureTime, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None),
                Description = input.Description,
                EndPoint = input.EndPoint,
                ImagePath = input.ImagePath,
                Seats = input.Seats,
                StartPoint = input.StartPoint
            };

            this.dbContext.Trips.Add(trip);
            this.dbContext.SaveChanges();
        }

        public IEnumerable<GetAllTripsViewModel> GetAll()
            => this.dbContext
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

        public GetTripDetailsViewModel GetDetails(string tripId)
            => this.dbContext
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

        public bool AddUserToTrip(string userId, string tripId)
        {
            if (this.dbContext.UserTrips
                    .Any(ut => ut.UserId == userId &&
                                          ut.TripId == tripId) ||
                this.dbContext.Trips
                    .Where(t => t.Id == tripId)
                    .Select(t => t.Seats - t.UserTrips.Count)
                    .FirstOrDefault() == 0)
            {
                return false;
            }

            this.dbContext.UserTrips.Add(new UserTrip()
            {
                TripId = tripId,
                UserId = userId
            });

            this.dbContext.SaveChanges();

            return true;
        }
    }
}