using System.Collections.Generic;
using MishMash.ViewModels.Channels;

namespace MishMash.Services
{
    public interface IChannelsService
    {
        void Create(string name, string description, string tags, string type);

        ChannelViewModel GetChannelById(int id);

        void FollowChannel(int userId, int channelId);

        bool IsUserFollowsChannel(int userId, int channelId);

        IEnumerable<BaseChannelViewModel> GetFollowedChannels(int userId);

        IEnumerable<BaseChannelViewModel> GetSuggestedChannels(int userId);

        IEnumerable<BaseChannelViewModel> GetOtherChannels(int userId);

        void UnfollowChannel(int userId, int channelId);
    }
}