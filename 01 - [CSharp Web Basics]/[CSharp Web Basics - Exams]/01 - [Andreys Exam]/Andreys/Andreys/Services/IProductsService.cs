using System.Collections.Generic;
using Andreys.ViewModels.Products;

namespace Andreys.Services
{
    public interface IProductsService
    {
        void Add(AddProductInputModel model);

        IEnumerable<DisplayAllProductsViewModel> DisplayAll();

        DisplayProductDetailViewModel GetDetail(int id);

        bool IsProductExisting(int id);

        void Delete(int id);
    }
}