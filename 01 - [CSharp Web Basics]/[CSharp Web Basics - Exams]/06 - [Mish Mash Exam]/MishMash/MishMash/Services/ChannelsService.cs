using System;
using System.Linq;
using MishMash.Data;
using System.Collections.Generic;
using MishMash.ViewModels.Channels;
using Type = MishMash.Data.Enums.Type;

namespace MishMash.Services
{
    public class ChannelsService : IChannelsService
    {
        private readonly ApplicationDbContext db;

        private readonly ITagsService tagsService;

        public ChannelsService(ApplicationDbContext db, ITagsService tagsService)
        {
            this.db = db;

            this.tagsService = tagsService;
        }

        public void Create(string name, string description, string tags, string type)
        {
            var splitTags = tags
                .Split(", ")
                .ToList();

            var channel = new Channel()
            {
                Name = name,
                Type = Enum.Parse<Type>(type),
                Description = description,
            };

            this.db.Channels.Add(channel);

            this.db.SaveChanges();

            foreach (var tag in splitTags)
            {
                var tagId = 0;

                if (this.tagsService.CheckForTagExisting(tag))
                {
                    tagId = this.tagsService.FindTagIdByName(tag);
                }
                else
                {
                    tagId = this.tagsService.Create(tag);
                }

                this.db.ChannelTags.Add(new ChannelTag()
                {
                    ChannelId = channel.Id,
                    TagId = tagId
                });

                this.db.SaveChanges();
            }
        }

        public ChannelViewModel GetChannelById(int id)
        {
            var channels = this.db
                .Channels
                .Where(c => c.Id == id)
                .Select(c => new ChannelViewModel()
                {
                    Description = c.Description,
                    FollowersCount = c.Followers.Count,
                    Name = c.Name,
                    Type = c.Type,
                    Tags = c.Tags.Select(t => t.Tag.Name)
                })
                .FirstOrDefault();

            return channels;
        }

        public void FollowChannel(int userId, int channelId)
        {
            this.db.UserChannels.Add(new UserChannel()
            {
                ChannelId = channelId,
                UserId = userId
            });

            this.db.SaveChanges();
        }

        public bool IsUserFollowsChannel(int userId, int channelId)
        {
            return this.db
                .UserChannels
                .Any(uc => uc.UserId == userId &&
                           uc.ChannelId == channelId);
        }

        public IEnumerable<BaseChannelViewModel> GetFollowedChannels(int userId)
        {
            var followedChannels = this.db
                .UserChannels
                .Where(uc => uc.UserId == userId)
                .Select(uc => new BaseChannelViewModel()
                {
                    Name = uc.Channel.Name,
                    Type = uc.Channel.Type,
                    FollowersCount = uc.Channel.Followers.Count,
                    Id = uc.Channel.Id
                })
                .ToList();

            return followedChannels;
        }

        public IEnumerable<BaseChannelViewModel> GetSuggestedChannels(int userId)
        {
            var followedChannelsTags = this.db
                .Channels
                .Where(c => c.Followers.Any(f => f.UserId == userId))
                .SelectMany(c => c.Tags.Select(t => t.TagId))
                .ToList();

            var suggestedChannels = this.db
                .Channels
                .Where(c => c.Followers.All(f => f.UserId != userId) &&
                            c.Tags.Any(t => followedChannelsTags.Contains(t.TagId)))
                .Select(c => new BaseChannelViewModel()
                {
                    Id = c.Id,
                    Type = c.Type,
                    FollowersCount = c.Followers.Count,
                    Name = c.Name
                })
                .ToList();

            return suggestedChannels;
        }

        public IEnumerable<BaseChannelViewModel> GetOtherChannels(int userId)
        {
            var ids = GetFollowedChannels(userId)
                .Select(c => c.Id)
                .ToList();

            ids = ids.Concat(GetSuggestedChannels(userId)
                    .Select(c => c.Id))
                .ToList();

            var otherChannels = this.db
                .Channels
                .Where(c => !ids.Contains(c.Id))
                .Select(c => new BaseChannelViewModel()
                {
                    FollowersCount = c.Followers.Count,
                    Id = c.Id,
                    Name = c.Name,
                    Type = c.Type
                })
                .ToList();

            return otherChannels;
        }

        public void UnfollowChannel(int userId, int channelId)
        {
            var channel = this.db
                .UserChannels
                .FirstOrDefault(uc => uc.ChannelId == channelId &&
                                      uc.UserId == userId);

            this.db.UserChannels.Remove(channel);

            this.db.SaveChanges();
        }
    }
}