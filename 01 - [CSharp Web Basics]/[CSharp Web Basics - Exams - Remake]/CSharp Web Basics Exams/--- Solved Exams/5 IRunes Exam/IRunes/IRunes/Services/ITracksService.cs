using IRunes.ViewModels.Tracks;

namespace IRunes.Services
{
    public interface ITracksService
    {
        void Create(CreateTrackInputModel model);

        GetTrackDetailsViewModel GetDetails(string albumId, string trackId);

        bool IsAlbumExisting(string albumId);
    }
}