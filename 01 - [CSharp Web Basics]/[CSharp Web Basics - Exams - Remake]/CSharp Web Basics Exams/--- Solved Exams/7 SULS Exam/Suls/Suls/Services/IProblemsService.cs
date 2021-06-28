using System.Collections.Generic;
using Suls.ViewModels.Problems;

namespace Suls.Services
{
    public interface IProblemsService
    {
        void Create(string name, int points);

        IEnumerable<DisplayAllProblemsViewModel> GetAll();

        DisplayProblemDetailsViewModel GetDetails(string id);

        string GetNameById(string id);
    }
}