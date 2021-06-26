using System;
using System.Globalization;

namespace SharedTrip.ViewModels.Trips
{
    public class GetAllTripsViewModel
    {
        public string Id { get; init; }

        public string StartPoint { get; init; }

        public string EndPoint { get; init; }

        public DateTime DepartureTime { get; init; }

        public string DepartureTimeAsString
            => this.DepartureTime
                .ToString("dd.MM.yyyy HH:mm:ss", CultureInfo.GetCultureInfo("bg-BG"));

        public int AvailableSeats { get; init; }
    }
}