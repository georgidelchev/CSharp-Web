using System.Linq;
using System.Text;
using IRunes.Data;
using IRunes.ViewModels.Home;
using System.Security.Cryptography;

namespace IRunes.Services
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext db;

        public UsersService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void Create(string username, string password, string email)
        {
            var user = new User()
            {
                Username = username,
                Email = email,
                Password = ComputeHash(password)
            };

            this.db.Users.Add(user);

            this.db.SaveChanges();
        }

        public string GetUserId(string username, string password)
        {
            var passwordHash = ComputeHash(password);

            var user = this.db
                .Users
                .FirstOrDefault(u => (u.Username == username &&
                          u.Password == passwordHash) || 
                                     (u.Email == username && 
                                      u.Password == passwordHash));

            return user?.Id;
        }

        public bool ConfirmUserLoginViaEmail(string email, string password)
        {
            var passwordHash = ComputeHash(password);

            return this.db
                .Users
                .Any(u => u.Email == email &&
                          u.Password == passwordHash);
        }

        public LoggedInUsernameViewModel GetUsername(string id)
        {
            var user = this.db
                .Users
                .Where(u => u.Id == id)
                .Select(u => new LoggedInUsernameViewModel()
                {
                    Username = u.Username
                })
                .FirstOrDefault();

            return user;
        }

        public bool IsUsernameAvailable(string username)
        {
            return !this.db
                .Users
                .Any(u => u.Username == username);
        }

        public bool IsEmailAvailable(string email)
        {
            return !this.db
                .Users
                .Any(u => u.Email == email);
        }

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