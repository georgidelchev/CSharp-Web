using SUS.HTTP;
using BattleCards.Data;
using SUS.MvcFramework;
using BattleCards.Services;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BattleCards
{
    public class Startup : IMvcApplication
    {
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.Add<IUsersService,UsersService>();

            serviceCollection.Add<ICardsService, CardsService>();
        }

        public void Configure(List<Route> routeTable)
        {
            new ApplicationDbContext().Database.Migrate();
        }
    }
}