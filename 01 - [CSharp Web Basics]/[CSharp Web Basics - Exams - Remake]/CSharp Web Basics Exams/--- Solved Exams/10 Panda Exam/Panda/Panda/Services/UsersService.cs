using Panda.Data;
using System.Linq;
using System.Text;
using Panda.ViewModels.Users;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Panda.Services
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext db;

        public UsersService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void Create(RegisterUserInputModel input)
        {
            var user = new User()
            {
                Email = input.Email,
                Username = input.Username,
                Password = ComputeHash(input.Password),
            };

            this.db.Users.Add(user);

            this.db.SaveChanges();
        }

        public string GetUserId(string username, string password)
        {
            var passwordHash = ComputeHash(password);

            var user = this.db
                .Users
                .FirstOrDefault(u => u.Username == username &&
                                     u.Password == passwordHash);

            return user?.Id;
        }

        public string GetUsernameById(string userId)
        {
            var user = this.db
                .Users
                .FirstOrDefault(u => u.Id == userId);

            return user?.Username;
        }

        public IEnumerable<GetAllUsersViewModel> GetAll()
        {
            var users = this.db
                .Users
                .Select(u => new GetAllUsersViewModel()
                {
                    Username = u.Username
                })
                .ToList();

            return users;
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