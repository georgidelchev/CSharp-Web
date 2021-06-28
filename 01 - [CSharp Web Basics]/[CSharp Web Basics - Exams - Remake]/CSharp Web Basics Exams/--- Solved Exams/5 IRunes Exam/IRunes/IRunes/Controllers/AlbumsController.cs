using SUS.HTTP;
using IRunes.Services;
using SUS.MvcFramework;

namespace IRunes.Controllers
{
    public class AlbumsController : Controller
    {
        private readonly IAlbumsService albumsService;

        private readonly ITracksService tracksService;

        public AlbumsController(IAlbumsService albumsService,ITracksService tracksService)
        {
            this.albumsService = albumsService;
            this.tracksService = tracksService;
        }

        public HttpResponse Create()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Create(string name, string cover)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (string.IsNullOrEmpty(name) ||
                name.Length < 4 ||
                name.Length > 20)
            {
                //return this.Error("Name should be between 4 and 20 characters.");

                return this.Redirect("/Albums/Create");
            }

            if (string.IsNullOrEmpty(cover))
            {
                //return this.Error("Cover is required");

                return this.Redirect("/Albums/Create");
            }

            this.albumsService.Create(name, cover);

            return this.Redirect("/Albums/All");
        }

        public HttpResponse All()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var viewModel = this.albumsService.GetAll();

            return this.View(viewModel);
        }

        public HttpResponse Details(string id)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (!this.tracksService.IsAlbumExisting(id))
            {
                return this.Error("Album is not existing.");
            }

            var viewModel = this.albumsService.GetDetails(id);

            return this.View(viewModel);
        }
    }
}