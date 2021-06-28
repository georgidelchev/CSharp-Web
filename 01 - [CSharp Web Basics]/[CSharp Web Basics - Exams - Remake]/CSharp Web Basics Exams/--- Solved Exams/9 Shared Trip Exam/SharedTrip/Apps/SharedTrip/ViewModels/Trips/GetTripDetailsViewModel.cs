using System;

namespace SharedTrip.ViewModels.Trips
{
    public class GetTripDetailsViewModel
    {
        public string Id { get; set; }

        public string StartPoint { get; set; }

        public string EndPoint { get; set; }

        public DateTime DepartureTime { get; set; }

        public string DepartureTimeAsString
            => this.DepartureTime.ToString("s");

        public int Seats { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }
    }
}