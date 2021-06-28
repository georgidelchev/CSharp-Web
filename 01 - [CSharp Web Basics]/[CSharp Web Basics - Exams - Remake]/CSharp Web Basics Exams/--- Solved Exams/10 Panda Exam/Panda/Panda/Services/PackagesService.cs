using System;
using Panda.Data;
using System.Linq;
using Panda.Data.Enums;
using Panda.ViewModels.Packages;
using System.Collections.Generic;

namespace Panda.Services
{
    public class PackagesService : IPackagesService
    {
        private readonly ApplicationDbContext db;
        private readonly IReceiptsService receiptsService;

        public PackagesService(ApplicationDbContext db, IReceiptsService receiptsService)
        {
            this.db = db;
            this.receiptsService = receiptsService;
        }

        public void Create(CreatePackageInputModel input, string recipient)
        {
            var recipientId = this.db
                .Users
                .FirstOrDefault(r => r.Username == recipient)
                ?.Id;

            var package = new Package()
            {
                Description = input.Description,
                Weight = input.Weight,
                Status = PackageStatus.Pending,
                ShippingAddress = input.ShippingAddress,
                EstimatedDeliveryDate = DateTime.UtcNow,
                RecipientId = recipientId
            };

            this.db.Packages.Add(package);

            this.db.SaveChanges();
        }

        public IEnumerable<PackageViewModel> GetPendingPackages()
        {
            var pendingPackages = this.db
                .Packages
                .Where(p => p.Status == PackageStatus.Pending)
                .Select(p => new PackageViewModel()
                {
                    Id = p.Id,
                    ShippingAddress = p.ShippingAddress,
                    Description = p.Description,
                    RecipientName = p.Recipient.Username,
                    Weight = p.Weight
                })
                .ToList();

            return pendingPackages;
        }

        public IEnumerable<PackageViewModel> GetDeliveredPackages()
        {
            var deliveredPackages = this.db
                .Packages
                .Where(p => p.Status == PackageStatus.Delivered)
                .Select(p => new PackageViewModel()
                {
                    Id = p.Id,
                    ShippingAddress = p.ShippingAddress,
                    Description = p.Description,
                    RecipientName = p.Recipient.Username,
                    Weight = p.Weight
                })
                .ToList();

            return deliveredPackages;

        }

        public void DeliverPackage(string id)
        {
            var package = this.db
                .Packages
                .FirstOrDefault(p => p.Id == id);

            package.Status = PackageStatus.Delivered;

            this.receiptsService.Create(package.Id, package.RecipientId);

            this.db.SaveChanges();
        }
    }
}