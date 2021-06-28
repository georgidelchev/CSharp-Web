using SUS.HTTP;
using Andreys.Data;
using Andreys.Services;
using SUS.MvcFramework;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Andreys
{
    public class Startup : IMvcApplication
    {
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.Add<IUsersService, UsersService>();
            serviceCollection.Add<IProductsService, ProductsService>();
        }

        public void Configure(List<Route> routeTable)
        {
            new AndreysDbContext().Database.Migrate();
        }
    }
}
