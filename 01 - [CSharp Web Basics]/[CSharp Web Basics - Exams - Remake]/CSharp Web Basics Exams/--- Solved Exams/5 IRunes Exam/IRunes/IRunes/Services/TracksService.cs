using System.Linq;
using IRunes.Data;
using IRunes.ViewModels.Tracks;

namespace IRunes.Services
{
    public class TracksService : ITracksService
    {
        private readonly ApplicationDbContext db;

        public TracksService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void Create(CreateTrackInputModel model)
        {
            var track = new Track()
            {
                AlbumId = model.AlbumId,
                Link = model.Link,
                Name = model.Name,
                Price = model.Price
            };

            this.db.Tracks.Add(track);

            this.db.SaveChanges();
        }

        public GetTrackDetailsViewModel GetDetails(string albumId, string trackId)
        {
            var trackDetails = this.db
                .Tracks
                .Where(t => t.AlbumId == albumId &&
                            t.Id == trackId)
                .Select(t => new GetTrackDetailsViewModel()
                {
                    Name = t.Name,
                    AlbumId = t.AlbumId,
                    Link = t.Link,
                    Id = t.Id,
                    Price = t.Price
                })
                .FirstOrDefault();

            return trackDetails;
        }

        public bool IsAlbumExisting(string albumId)
        {
            return this.db
                .Albums
                .Any(a => a.Id == albumId);
        }
    }
}