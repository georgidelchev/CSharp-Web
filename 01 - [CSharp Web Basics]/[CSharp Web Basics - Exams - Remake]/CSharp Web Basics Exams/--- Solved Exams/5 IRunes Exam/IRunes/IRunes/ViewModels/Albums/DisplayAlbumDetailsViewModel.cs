using System.Collections.Generic;

namespace IRunes.ViewModels.Albums
{
    public class DisplayAlbumDetailsViewModel
    {
        public string Cover { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Id { get; set; }

        public List<DisplayTracksViewModel> Tracks { get; set; }
    }
}