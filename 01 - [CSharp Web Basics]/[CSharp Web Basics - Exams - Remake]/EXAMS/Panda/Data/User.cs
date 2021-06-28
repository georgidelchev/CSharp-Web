using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Panda.Data
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
        [MaxLength(20)]
        public string Username { get; set; }

        [Required]
        [MaxLength(20)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public virtual ICollection<Package> Packages { get; set; }
            = new HashSet<Package>();

        public virtual ICollection<Receipt> Receipts { get; set; }
            = new HashSet<Receipt>();
    }
}
