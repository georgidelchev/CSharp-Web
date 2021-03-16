using Panda.ViewModels.Users;
using System.Collections.Generic;

namespace Panda.Services
{
    public interface IUsersService
    {
        void Create(RegisterUserInputModel input);

        string GetUserId(string username, string password);

        string GetUsernameById(string userId);

        IEnumerable<GetAllUsersViewModel> GetAll();

        bool IsUsernameAvailable(string username);

        bool IsEmailAvailable(string email);
    }
}