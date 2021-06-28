using System.Collections.Generic;
using MishMash.Data.Enums;

namespace MishMash.ViewModels.Channels
{
    public class ChannelViewModel
    {
        public string Name { get; set; }

        public Type Type { get; set; }

        public string Description { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public string TagsAsString
            => string.Join(", ", this.Tags);

        public int FollowersCount { get; set; }
    }
}