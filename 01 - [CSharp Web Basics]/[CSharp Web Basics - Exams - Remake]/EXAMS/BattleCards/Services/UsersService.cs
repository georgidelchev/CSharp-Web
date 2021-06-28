using System.Linq;
using System.Text;
using BattleCards.Data;
using BattleCards.ViewModels.Users;
using System.Security.Cryptography;

namespace BattleCards.Services
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext dbContext;

        public UsersService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Register(RegisterUserInputModel input)
        {
            var user = new User()
            {
                Username = input.Username,
                Password = ComputeHash(input.Password),
                Email = input.Email,
            };

            this.dbContext.Add(user);
            this.dbContext.SaveChanges();
        }

        public string GetUserId(string username, string password)
            => this.dbContext
                .Users
                .FirstOrDefault(u => u.Username == username &&
                                     u.Password == ComputeHash(password))
                ?.Id;

        public bool IsUsernameTaken(string username)
            => this.dbContext
                .Users
                .Any(u => u.Username == username);

        public bool IsEmailTaken(string email)
            => this.dbContext
                .Users
                .Any(u => u.Email == email);

        private static string ComputeHash(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);

            using (var hash = SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);

                // Convert to text
                // StringBuilder Capacity is 128, because 512 bits / 8 bits in byte * 2 symbols for byte 
                var hashedInputStringBuilder = new System.Text.StringBuilder(128);

                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));

                return hashedInputStringBuilder.ToString();
            }
        }
    }
}
