using CarShop.Data;

using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CarShop.Services
{
    public class UsersService: IUsersService
    {

        public void Create(string username, string email, string password, string userType)
        {
            throw new System.NotImplementedException();
        }

        public string GetUserId(string username, string password)
        {
            throw new System.NotImplementedException();
        }

        public bool IsUserMechanic(string Userid)
        {
            throw new System.NotImplementedException();
        }

        public bool IsUsernameAvailable(string username)
        {
            throw new System.NotImplementedException();
        }
    }
}
