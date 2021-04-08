using System.Threading.Tasks;

namespace MyRecipes.Services
{
    public interface IGotvachBgScraperService
    {
        Task PopulateDbWithRecipesAsync();
    }
}
