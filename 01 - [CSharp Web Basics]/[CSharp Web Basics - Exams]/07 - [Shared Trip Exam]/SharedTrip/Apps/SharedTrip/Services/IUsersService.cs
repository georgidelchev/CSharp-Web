using SharedTrip.ViewModels.Users;

namespace SharedTrip.Services
{
    public interface IUsersService
    {
        void Create(RegisterInputModel input);

        string GetUserId(string username, string password);

        bool IsUsernameAvailable(string username);

        bool IsEmailAvailable(string email);
    }
}