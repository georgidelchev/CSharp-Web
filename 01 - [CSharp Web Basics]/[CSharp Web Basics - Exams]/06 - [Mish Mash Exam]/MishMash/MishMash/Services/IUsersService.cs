using MishMash.Data;
using MishMash.ViewModels.Users;

namespace MishMash.Services
{
    public interface IUsersService
    {
        void Create(RegisterUserInputModel input);

        int? GetUserId(string username, string password);

        bool IsUsernameAvailable(string username);

        bool IsEmailAvailable(string email);

        string GetUserRoleById(int userId);

        string GetUsernameById(int userId);
    }
}