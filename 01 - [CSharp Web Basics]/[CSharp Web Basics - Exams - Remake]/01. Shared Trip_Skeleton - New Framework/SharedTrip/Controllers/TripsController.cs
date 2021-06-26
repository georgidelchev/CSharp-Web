using System;
using MyWebServer.Http;
using SharedTrip.Services;
using System.Globalization;
using MyWebServer.Controllers;
using SharedTrip.ViewModels.Trips;

namespace SharedTrip.Controllers
{
    public class TripsController : Controller
    {
        private readonly ITripsService tripsService;

        public TripsController(
            ITripsService tripsService)
        {
            this.tripsService = tripsService;
        }

        [HttpGet]
        public HttpResponse All()
        {
            if (!this.User.IsAuthenticated)
            {
                return this.Redirect("/Users/Login");
            }

            var viewModel = this.tripsService.GetAll();

            return this.View(viewModel);
        }

        [HttpGet]
        public HttpResponse Add()
        {
            if (!this.User.IsAuthenticated)
            {
                return this.Redirect("/Users/Login");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Add(AddTripInputModel input)
        {
            if (!this.User.IsAuthenticated)
            {
                return this.Redirect("/Users/Login");
            }

            if (string.IsNullOrEmpty(input.StartPoint))
            {
                return this.Redirect("/Trips/Add");
            }

            if (string.IsNullOrEmpty(input.EndPoint))
            {
                return this.Redirect("/Trips/Add");
            }

            if (input.Seats < 2 ||
                input.Seats > 6)
            {
                return this.Redirect("/Trips/Add");
            }

            if (string.IsNullOrEmpty(input.Description) ||
                input.Description.Length > 80)
            {
                return this.Redirect("/Trips/Add");
            }

            if (!DateTime.TryParseExact(input.DepartureTime, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                return this.Redirect("/Trips/Add");
            }

            this.tripsService.Add(input);

            return this.Redirect("/Trips/All");
        }

        [HttpGet]
        public HttpResponse Details(string tripId)
        {
            if (!this.User.IsAuthenticated)
            {
                return this.Redirect("/Users/Login");
            }

            var viewModel = this.tripsService.GetDetails(tripId);

            return this.View(viewModel);
        }

        [HttpGet]
        public HttpResponse AddUserToTrip(string tripId)
        {
            if (!this.User.IsAuthenticated)
            {
                return this.Redirect("/Users/Login");
            }

            var userId = this.User.Id;

            if (!this.tripsService.AddUserToTrip(userId, tripId))
            {
                return this.Redirect($"/Trips/Details?tripId={tripId}");
            }

            return this.Redirect("/Trips/All");
        }
    }
}