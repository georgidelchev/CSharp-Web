using System;
using System.Globalization;

namespace SharedTrip.ViewModels.Trips
{
    public class GetAllTripsViewModel
    {
        public string Id { get; set; }

        public string StartPoint { get; set; }

        public string EndPoint { get; set; }

        public DateTime DepartureTime { get; set; }

        public string DepartureTimeAsString
            => this.DepartureTime.ToString("dd.MM.yyyy HH:mm:ss", CultureInfo.GetCultureInfo("bg-BG"));

        public int AvailableSeats { get; set; }
    }
}