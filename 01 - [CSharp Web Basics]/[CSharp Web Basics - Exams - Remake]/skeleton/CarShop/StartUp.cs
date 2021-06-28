using CarShop.Data;
using CarShop.Services;

using Microsoft.EntityFrameworkCore;

using SUS.HTTP;
using SUS.MvcFramework;

using System.Collections.Generic;

namespace CarShop
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
        }
    }
}
