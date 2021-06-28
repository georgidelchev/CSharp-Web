using Panda.ViewModels.Users;
using System.Collections.Generic;

namespace Panda.Services
{
    public interface IUsersService
    {
        void Register(RegisterUserInputModel input);

        string GetUserId(string username, string password);

        bool IsUsernameTaken(string username);

        bool IsEmailTaken(string email);

        string GetUsername(string id);

        IEnumerable<GetAllUsersViewModel> GetAllUsernames();
    }
}
