using Panda.ViewModels.Packages;
using System.Collections.Generic;

namespace Panda.Services
{
    public interface IPackagesService
    {
        void Create(CreatePackageInputModel input, string userId);

        IEnumerable<PackageViewModel> GetPendingPackages();

        IEnumerable<PackageViewModel> GetDeliveredPackages();

        void DeliverPackage(string id);
    }
}