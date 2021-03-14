using SUS.HTTP;
using MishMash.Data;
using SUS.MvcFramework;
using MishMash.Services;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MishMash
{
    public class StartUp : IMvcApplication
    {
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.Add<IUsersService, UsersService>();

            serviceCollection.Add<IChannelsService, ChannelsService>();

            serviceCollection.Add<ITagsService, TagsService>();
        }

        public void Configure(List<Route> routeTable)
        {
            new ApplicationDbContext().Database.Migrate();
        }
    }
}
