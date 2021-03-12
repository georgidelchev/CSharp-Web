using System.Collections.Generic;
using Git.ViewModels.Repositories;

namespace Git.Services
{
    public interface IRepositoriesService
    {
        void Create(string name, string type, string userId);

        string GetRepositoryName(string id);

        IEnumerable<DisplayAllRepositoriesViewModel> GetAll();
    }
}