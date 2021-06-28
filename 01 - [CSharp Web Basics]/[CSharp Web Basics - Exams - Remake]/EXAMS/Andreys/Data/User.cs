using System;
using System.ComponentModel.DataAnnotations;

namespace Andreys.Data
{
    public class User
    {
        public User()
        {
            this.Id = Guid
                .NewGuid()
                .ToString();
        }

        [Key]
        [Required]
        public string Id { get; set; }

        [Required]
        [MaxLength(10)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public string Email { get; set; }
    }
}
