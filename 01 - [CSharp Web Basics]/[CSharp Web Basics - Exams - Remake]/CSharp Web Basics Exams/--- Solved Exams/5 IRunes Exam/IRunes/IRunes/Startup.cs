using SUS.HTTP;
using IRunes.Data;
using IRunes.Services;
using SUS.MvcFramework;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace IRunes
{
    public class Startup : IMvcApplication
    {
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.Add<IUsersService, UsersService>();

            serviceCollection.Add<IAlbumsService, AlbumsService>();

            serviceCollection.Add<ITracksService, TracksService>();
        }

        public void Configure(List<Route> routeTable)
        {
            new ApplicationDbContext().Database.Migrate();
        }
    }
}