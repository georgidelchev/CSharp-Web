using Git.ViewModels.Users;

namespace Git.Services
{
    public interface IUsersService
    {
        void Create(RegisterUserInputModel input);

        string GetUserId(string username, string password);

        bool IsUsernameAvailable(string username);

        bool IsEmailAvailable(string email);
    }
}