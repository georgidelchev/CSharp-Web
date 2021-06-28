using Panda.ViewModels.Receipts;
using System.Collections.Generic;

namespace Panda.Services
{
    public interface IReceiptsService
    {
        void Create(string packageId, string recipientId);

        IEnumerable<GetReceiptsViewModel> GetAll(string userId);
    }
}