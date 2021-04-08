using System.Collections.Generic;

namespace MyRecipes.Services.Data
{
    public interface ICategoriesService
    {
        IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs();
    }
}
