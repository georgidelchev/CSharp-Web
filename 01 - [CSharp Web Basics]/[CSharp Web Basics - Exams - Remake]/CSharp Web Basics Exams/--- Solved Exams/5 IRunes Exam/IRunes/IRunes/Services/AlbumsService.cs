using System.Linq;
using IRunes.Data;
using IRunes.ViewModels.Albums;
using System.Collections.Generic;

namespace IRunes.Services
{
    public class AlbumsService : IAlbumsService
    {
        private readonly ApplicationDbContext db;

        public AlbumsService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void Create(string name, string cover)
        {
            var album = new Album()
            {
                Cover = cover,
                Name = name,
                Price = 0
            };

            this.db.Albums.Add(album);

            this.db.SaveChanges();
        }

        public DisplayAlbumDetailsViewModel GetDetails(string id)
        {
            var albumDetails = this.db
                .Albums
                .Where(a => a.Id == id)
                .Select(a => new DisplayAlbumDetailsViewModel()
                {
                    Id = a.Id,
                    Name = a.Name,
                    Price = a.Tracks.Sum(t => t.Price) - (a.Tracks.Sum(t => t.Price) * 0.13m),
                    Cover = a.Cover,
                    Tracks = a.Tracks.Select(t => new DisplayTracksViewModel()
                    {
                        Name = t.Name,
                        Id = t.Id
                    })
                        .ToList()
                })
                .FirstOrDefault();

            return albumDetails;
        }

        public IEnumerable<DisplayAllAlbumsViewModel> GetAll()
        {
            var albums = this.db
                .Albums
                .Select(a => new DisplayAllAlbumsViewModel()
                {
                    Id = a.Id,
                    Name = a.Name
                })
                .ToList();

            return albums;
        }
    }
}