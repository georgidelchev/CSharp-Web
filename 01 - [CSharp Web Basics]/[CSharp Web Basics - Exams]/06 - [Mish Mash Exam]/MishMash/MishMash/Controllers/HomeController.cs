using System.Collections.Generic;
using MishMash.Services;
using MishMash.ViewModels.Home;
using SUS.HTTP;
using SUS.MvcFramework;

namespace MishMash.Controllers
{
    public class HomeController : Controller
    {
        private readonly IChannelsService channelsService;

        private readonly IUsersService usersService;

        public HomeController(IChannelsService channelsService, IUsersService usersService)
        {
            this.channelsService = channelsService;

            this.usersService = usersService;
        }

        [HttpGet("/")]
        public HttpResponse Index()
        {
            if (this.IsUserSignedIn())
            {
                var userId = int.Parse(this.GetUserId());

                var viewModel = new LoggedInIndexViewModel();

                viewModel.UserRole = this.usersService.GetUserRoleById(userId);

                viewModel.Username = this.usersService.GetUsernameById(userId);

                viewModel.YourChannels = this.channelsService.GetFollowedChannels(userId);

                viewModel.SuggestedChannels = this.channelsService.GetSuggestedChannels(userId);

                viewModel.SeeOther = this.channelsService.GetOtherChannels(userId);

                return this.View(viewModel, "LoggedInIndex");
            }

            return this.View();
        }
    }
}