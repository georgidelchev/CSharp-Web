using MishMash.Data.Enums;

namespace MishMash.ViewModels.Channels
{
    public class BaseChannelViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Type Type { get; set; }

        public int FollowersCount { get; set; }
    }
}