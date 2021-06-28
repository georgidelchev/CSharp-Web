using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SulsApp.Data;
using SUS.HTTP;
using SUS.MvcFramework;
namespace SulsApp
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
