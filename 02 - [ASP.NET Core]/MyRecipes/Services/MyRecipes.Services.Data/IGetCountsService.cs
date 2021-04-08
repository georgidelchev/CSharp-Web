using MyRecipes.Services.Data.Models;

namespace MyRecipes.Services.Data
{
    public interface IGetCountsService
    {
        // 1. Use the view model.
        // 2. Create DTO -> view model
        // 3. Use Tuples
        CountsDto GetCounts();
    }
}
