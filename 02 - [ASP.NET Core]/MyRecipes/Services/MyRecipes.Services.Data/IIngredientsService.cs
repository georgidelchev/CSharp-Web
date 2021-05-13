using System.Collections.Generic;

namespace MyRecipes.Services.Data
{
    public interface IIngredientsService
    {
        IEnumerable<T> GetAllPopular<T>();
    }
}