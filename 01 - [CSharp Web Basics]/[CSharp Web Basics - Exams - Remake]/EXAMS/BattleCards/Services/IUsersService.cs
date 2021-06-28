using BattleCards.ViewModels.Users;

namespace BattleCards.Services
{
    public interface IUsersService
    {
        void Register(RegisterUserInputModel input);

        string GetUserId(string username, string password);

        bool IsUsernameTaken(string username);

        bool IsEmailTaken(string email);
    }
}
