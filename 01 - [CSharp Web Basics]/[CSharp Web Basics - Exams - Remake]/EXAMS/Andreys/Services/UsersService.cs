using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Andreys.Data;
using Andreys.ViewModels.Users;

namespace Andreys.Services
{
    public class UsersService : IUsersService
    {
        private readonly AndreysDbContext dbContext;

        public UsersService(AndreysDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Register(RegisterUserInputModel input)
        {
            var user = new User()
            {
                Username = input.Username,
                Email = input.Email,
                Password = ComputeHash(input.Password),
            };

            this.dbContext.Add(user);
            this.dbContext.SaveChanges();
        }

        public string GetUserId(string username, string password)
            => this.dbContext
                .Users
                .FirstOrDefault(u => u.Username == username &&
                                     u.Password == ComputeHash(password))?.Id;

        public bool IsUsernameTaken(string username)
            => this.dbContext
                .Users
                .Any(u => u.Username == username);

        public bool IsEmailTaken(string email)
            => this.dbContext
                .Users
                .Any(u => u.Email == email);

        // Helper method for password hash
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
