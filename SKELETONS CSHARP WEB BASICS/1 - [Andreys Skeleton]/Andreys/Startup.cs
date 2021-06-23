using System.Collections.Generic;
using Andreys.Data;
using Microsoft.EntityFrameworkCore;
using SUS.HTTP;
using SUS.MvcFramework;

namespace Andreys
{
    public class Startup : IMvcApplication
    {
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
        }

        public void Configure(List<Route> routeTable)
        {
            new AndreysDbContext().Database.Migrate();
        }
    }
}
