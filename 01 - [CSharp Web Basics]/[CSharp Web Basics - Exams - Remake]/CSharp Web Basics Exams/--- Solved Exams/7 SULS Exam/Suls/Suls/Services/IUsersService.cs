using Suls.ViewModels.Users;

namespace Suls.Services
{
    public interface IUsersService
    {
        void Create(RegisterUserInputModel input);

        string GetUserId(string username, string password);

        bool IsEmailAvailable(string email);

        bool IsUsernameAvailable(string username);
    }
}