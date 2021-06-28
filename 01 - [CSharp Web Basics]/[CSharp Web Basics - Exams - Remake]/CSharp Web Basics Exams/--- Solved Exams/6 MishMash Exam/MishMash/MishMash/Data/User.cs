using MishMash.Data.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MishMash.Data
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public Role Role { get; set; }

        public ICollection<UserChannel> Channels { get; set; }
            = new HashSet<UserChannel>();
    }
}