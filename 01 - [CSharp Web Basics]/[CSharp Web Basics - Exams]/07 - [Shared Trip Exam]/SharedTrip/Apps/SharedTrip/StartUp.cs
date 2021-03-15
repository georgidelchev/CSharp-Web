using SUS.HTTP;
using SharedTrip.Data;
using SUS.MvcFramework;
using SharedTrip.Services;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

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
