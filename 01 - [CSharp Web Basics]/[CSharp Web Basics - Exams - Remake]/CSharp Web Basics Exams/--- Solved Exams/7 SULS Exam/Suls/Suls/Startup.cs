using SUS.HTTP;
using Suls.Data;
using Suls.Services;
using SUS.MvcFramework;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Suls
{
    public class Startup : IMvcApplication
    {
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.Add<IUsersService, UsersService>();

            serviceCollection.Add<IProblemsService, ProblemsService>();

            serviceCollection.Add<ISubmissionsService, SubmissionsService>();
        }

        public void Configure(List<Route> routeTable)
        {
            new ApplicationDbContext().Database.Migrate();
        }
    }
}
