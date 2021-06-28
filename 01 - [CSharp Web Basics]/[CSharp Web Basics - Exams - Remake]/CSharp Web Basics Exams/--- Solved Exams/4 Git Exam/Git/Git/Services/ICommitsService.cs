using Git.ViewModels.Commits;
using System.Collections.Generic;

namespace Git.Services
{
    public interface ICommitsService
    {
        void Create(string description, string repositoryId, string userId);

        IEnumerable<DisplayAllCommitsViewModel> GetAll(string userId);

        bool Delete(string id, string userId);
    }
}