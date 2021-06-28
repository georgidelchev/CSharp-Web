using System;
using Panda.Data;
using System.Linq;
using Panda.ViewModels.Receipts;
using System.Collections.Generic;

namespace Panda.Services
{
    public class ReceiptsService : IReceiptsService
    {
        private readonly ApplicationDbContext dbContext;

        public ReceiptsService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<GetAllReceiptsViewModel> GetAllForUser(string userId)
            => this.dbContext
                .Receipts
                .Where(r => r.RecipientId == userId)
                .Select(r => new GetAllReceiptsViewModel()
                {
                    Fee = r.Fee,
                    Id = r.Id,
                    IssuedOn = r.IssuedOn,
                    Recipient = r.Recipient.Username
                })
                .ToList();

        public void Create(string packageId, string recipientId)
        {
            var package = this.dbContext
                .Packages
                .FirstOrDefault(p => p.Id == packageId);

            var receipt = new Receipt()
            {
                Fee = package.Weight * 2.67m,
                IssuedOn = DateTime.UtcNow,
                PackageId = packageId,
                RecipientId = recipientId,
            };

            this.dbContext.Add(receipt);
            this.dbContext.SaveChanges();
        }
    }
}
