using System.Collections.Generic;
using Andreys.ViewModels.Products;

namespace Andreys.Services
{
    public interface IProductsService
    {
        void Add(AddProductInputModel input);

        IEnumerable<GetAllProductsViewModel> GetAll();

        GetProductDetailsViewModel GetDetails(int id);

        void Delete(int id);
    }
}
