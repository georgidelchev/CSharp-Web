using SUS.HTTP;
using SharedTrip.Data;
using SUS.MvcFramework;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SharedTrip.Services;

namespace SharedTrip
{
    public class Startup : IMvcApplication
    {
        public void Configure(List<Route> routeTable)
        {
            new ApplicationDbContext().Database.Migrate();
        }

        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.Add<IUsersService, UsersService>();

            serviceCollection.Add<ITripsService, TripsService>();
        }
    }
}
