using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MishMash.Services;
using SUS.HTTP;
using SUS.MvcFramework;

namespace MishMash.Controllers
{
    public class ChannelsController : Controller
    {
        private readonly IChannelsService channelsService;

        public ChannelsController(IChannelsService channelsService)
        {
            this.channelsService = channelsService;
        }

        public HttpResponse Create()
        {
            return this.View();
        }

        [HttpPost]
        public HttpResponse Create(string name, string description, string tags, string type)
        {
            this.channelsService.Create(name, description, tags, type);

            return this.Redirect("/");
        }

        public HttpResponse Details(int id)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var viewModel = this.channelsService.GetChannelById(id);

            return this.View(viewModel);
        }

        public HttpResponse Follow(int id)
        {
            var userId = int.Parse(this.GetUserId());

            if (this.channelsService.IsUserFollowsChannel(userId, id))
            {
                return this.Redirect("/Channels/Followed");
            }

            this.channelsService.FollowChannel(userId, id);

            return this.Redirect("/Channels/Followed");
        }

        public HttpResponse Followed()
        {
            var userId = int.Parse(this.GetUserId());

            var viewModel = this.channelsService.GetFollowedChannels(userId);

            return this.View(viewModel);
        }

        public HttpResponse Unfollow(int id)
        {
            var userId = int.Parse(this.GetUserId());

            this.channelsService.UnfollowChannel(userId, id);

            return this.Redirect("/Channels/Followed");
        }
    }
}