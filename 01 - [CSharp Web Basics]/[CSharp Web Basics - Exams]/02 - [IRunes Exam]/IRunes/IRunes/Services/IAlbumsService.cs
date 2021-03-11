using IRunes.ViewModels.Albums;
using System.Collections.Generic;

namespace IRunes.Services
{
    public interface IAlbumsService
    {
        void Create(string name, string cover);

        DisplayAlbumDetailsViewModel GetDetails(string id);

        IEnumerable<DisplayAllAlbumsViewModel> GetAll();
    }
}