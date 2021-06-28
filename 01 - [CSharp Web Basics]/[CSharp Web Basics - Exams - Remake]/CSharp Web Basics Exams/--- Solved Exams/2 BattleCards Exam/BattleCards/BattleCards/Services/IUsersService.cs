using BattleCards.ViewModels.Users;

namespace BattleCards.Services
{
    public interface IUsersService
    {
        void Create(RegisterUserInputModel input);

        string GetUserId(string username, string password);

        bool IsUsernameAvailable(string username);

        bool IsEmailAvailable(string email);
    }
}