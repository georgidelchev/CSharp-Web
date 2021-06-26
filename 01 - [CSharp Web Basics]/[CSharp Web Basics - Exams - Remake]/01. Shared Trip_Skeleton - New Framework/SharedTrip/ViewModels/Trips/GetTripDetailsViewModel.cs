using System;

namespace SharedTrip.ViewModels.Trips
{
    public class GetTripDetailsViewModel
    {
        public string Id { get; init; }

        public string StartPoint { get; init; }

        public string EndPoint { get; init; }

        public DateTime DepartureTime { get; init; }

        public string DepartureTimeAsString
            => this.DepartureTime.ToString("s");

        public int Seats { get; init; }

        public string Description { get; init; }

        public string ImageUrl { get; init; }
    }
}