using Panda.ViewModels.Packages;
using System.Collections.Generic;

namespace Panda.Services
{
    public interface IPackagesService
    {
        void Create(CreatePackageInputModel input);

        IEnumerable<GetPackagesViewModel> GetPendings();

        IEnumerable<GetPackagesViewModel> GetDelivered();

        void Deliver(string id);
    }
}