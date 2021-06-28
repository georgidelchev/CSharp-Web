using System.Collections.Generic;
using GameStore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SUS.HTTP;
using SUS.MvcFramework;

namespace GameStore
{
    public class Startup : IMvcApplication
    {
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
        }

        public void Configure(List<Route> routeTable)
        {
            new ApplicationDbContext().Database.Migrate();
        }
    }
}
