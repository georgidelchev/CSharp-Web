using Andreys.ViewModels.Users;

namespace Andreys.Services
{
    public interface IUsersService
    {
        void Register(RegisterUserInputModel input);

        string GetUserId(string username, string password);

        bool IsUsernameTaken(string username);

        bool IsEmailTaken(string email);
    }
}
