using SUS.HTTP;
using IRunes.Services;
using SUS.MvcFramework;
using IRunes.ViewModels.Tracks;

namespace IRunes.Controllers
{
    public class TracksController : Controller
    {
        private readonly ITracksService tracksService;

        public TracksController(ITracksService tracksService)
        {
            this.tracksService = tracksService;
        }

        public HttpResponse Create(string albumId)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var viewModel = new TrackAlbumIdViewModel()
            {
                AlbumId = albumId
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public HttpResponse Create(CreateTrackInputModel input)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (!this.tracksService.IsAlbumExisting(input.AlbumId))
            {
                return this.Error("Album is not existing.");
            }

            if (string.IsNullOrEmpty(input.Name) ||
                input.Name.Length < 4 ||
                input.Name.Length > 20)
            {
                //return this.Error("Track name should be between 4 and 20 characters");

                return this.Redirect($"/Tracks/Create?albumId={input.AlbumId}");
            }

            if (string.IsNullOrEmpty(input.Link))
            {
                return this.Error("Video link is required.");
            }

            this.tracksService.Create(input);

            return this.Redirect($"/Albums/Details?id={input.AlbumId}");
        }

        public HttpResponse Details(string albumId, string trackId)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var viewModel = this.tracksService.GetDetails(albumId, trackId);

            return this.View(viewModel);
        }
    }
}