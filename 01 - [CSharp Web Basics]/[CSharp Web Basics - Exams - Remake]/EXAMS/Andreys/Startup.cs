using System.Collections.Generic;
using Andreys.Data;
using Andreys.Services;
using Microsoft.EntityFrameworkCore;
using SUS.HTTP;
using SUS.MvcFramework;

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
