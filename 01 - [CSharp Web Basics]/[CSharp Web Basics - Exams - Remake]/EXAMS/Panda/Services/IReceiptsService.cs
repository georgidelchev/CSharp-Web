using Panda.ViewModels.Receipts;
using System.Collections.Generic;

namespace Panda.Services
{
    public interface IReceiptsService
    {
        IEnumerable<GetAllReceiptsViewModel> GetAllForUser(string userId);

        void Create(string packageId, string recipientId);
    }
}
