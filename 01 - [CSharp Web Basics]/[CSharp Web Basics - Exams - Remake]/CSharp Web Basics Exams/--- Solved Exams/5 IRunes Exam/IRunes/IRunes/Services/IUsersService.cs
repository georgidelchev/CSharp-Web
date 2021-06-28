using IRunes.ViewModels.Home;

namespace IRunes.Services
{
    public interface IUsersService
    {
        void Create(string username, string password, string email);

        string GetUserId(string username, string password);

        bool ConfirmUserLoginViaEmail(string email, string password);

        LoggedInUsernameViewModel GetUsername(string id);

        bool IsUsernameAvailable(string username);

        bool IsEmailAvailable(string email);
    }
}