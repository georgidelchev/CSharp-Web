using System;
using Panda.Data;
using System.Linq;
using Panda.ViewModels.Receipts;
using System.Collections.Generic;

namespace Panda.Services
{
    public class ReceiptsService : IReceiptsService
    {
        private readonly ApplicationDbContext db;

        public ReceiptsService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void Create(string packageId, string recipientId)
        {
            var package = this.db
                .Packages
                .Find(packageId);

            var receipt = new Receipt()
            {
                IssuedOn = DateTime.UtcNow,
                PackageId = packageId,
                RecipientId = recipientId,
                Fee = package.Weight * 2.67m,
            };

            this.db.Receipts.Add(receipt);

            this.db.SaveChanges();
        }

        public IEnumerable<GetReceiptsViewModel> GetAll(string userId)
        {
            var receipts = this.db
                .Receipts
                .Where(r => r.RecipientId == userId)
                .Select(r => new GetReceiptsViewModel()
                {
                    Fee = r.Fee,
                    Id = r.Id,
                    IssuedOn = r.IssuedOn,
                    RecipientName = r.Recipient.Username
                })
                .ToList();

            return receipts;
        }
    }
}