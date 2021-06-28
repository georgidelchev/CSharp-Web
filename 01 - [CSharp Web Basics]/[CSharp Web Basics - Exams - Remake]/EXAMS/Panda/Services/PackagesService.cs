using System;
using System.Linq;
using Panda.Data;
using Panda.Data.Enumerations;
using Panda.ViewModels.Packages;
using System.Collections.Generic;

namespace Panda.Services
{
    public class PackagesService : IPackagesService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IReceiptsService receiptsService;

        public PackagesService(
            ApplicationDbContext dbContext,
            IReceiptsService receiptsService)
        {
            this.dbContext = dbContext;
            this.receiptsService = receiptsService;
        }

        public void Create(CreatePackageInputModel input)
        {
            var package = new Package()
            {
                Description = input.Description,
                Status = Status.Pending,
                Weight = input.Weight,
                ShippingAddress = input.ShippingAddress,
                RecipientId = this.dbContext.Users.FirstOrDefault(u => u.Username == input.RecipientName)?.Id,
                EstimatedDeliveryDate = DateTime.UtcNow,
            };

            this.dbContext.Add(package);
            this.dbContext.SaveChanges();
        }

        public IEnumerable<GetPackagesViewModel> GetPendings()
            => this.dbContext
                .Packages
                .Where(p => p.Status == Status.Pending)
                .Select(p => new GetPackagesViewModel()
                {
                    Id = p.Id,
                    Description = p.Description,
                    Recipient = p.Recipient.Username,
                    ShippingAddress = p.ShippingAddress,
                    Weight = p.Weight,
                })
                .ToList();

        public IEnumerable<GetPackagesViewModel> GetDelivered()
            => this.dbContext
                .Packages
                .Where(p => p.Status == Status.Delivered)
                .Select(p => new GetPackagesViewModel()
                {
                    Id = p.Id,
                    Description = p.Description,
                    Recipient = p.Recipient.Username,
                    ShippingAddress = p.ShippingAddress,
                    Weight = p.Weight,
                })
                .ToList();

        public void Deliver(string id)
        {
            var package = this.dbContext
                .Packages
                .FirstOrDefault(p => p.Id == id);

            package.Status = Status.Delivered;
            this.receiptsService.Create(package.Id, package.RecipientId);

            this.dbContext.Update(package);
            this.dbContext.SaveChangesAsync();
        }
    }
}
