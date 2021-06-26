using System.Linq;
using System.Text;
using SharedTrip.Data;
using SharedTrip.ViewModels.Users;
using System.Security.Cryptography;

namespace SharedTrip.Services
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext dbContext;

        public UsersService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Create(RegisterInputModel input)
        {
            var user = new User()
            {
                Username = input.Username,
                Email = input.Email,
                Password = ComputeHash(input.Password)
            };

            this.dbContext.Users.Add(user);
            this.dbContext.SaveChanges();
        }

        public string GetUserId(string username, string password)
        {
            var passwordHash = ComputeHash(password);

            var user = this.dbContext
                .Users
                .FirstOrDefault(u => u.Username == username &&
                                     u.Password == passwordHash);

            return user?.Id;
        }

        public bool IsUsernameAvailable(string username)
            => !this.dbContext
                .Users
                .Any(u => u.Username == username);

        public bool IsEmailAvailable(string email)
        => !this.dbContext
            .Users
            .Any(u => u.Email == email);

        private static string ComputeHash(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);

            using var hash = SHA512.Create();

            var hashedInputBytes = hash.ComputeHash(bytes);
            var hashedInputStringBuilder = new System.Text.StringBuilder(128);

            foreach (var b in hashedInputBytes)
                hashedInputStringBuilder.Append(b.ToString("X2"));

            return hashedInputStringBuilder.ToString();
        }
    }
}