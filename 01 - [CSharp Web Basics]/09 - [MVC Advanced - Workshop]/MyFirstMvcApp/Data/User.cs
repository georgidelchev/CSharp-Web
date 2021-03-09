using System;
using SUS.MvcFramework;
using System.Collections.Generic;

namespace BattleCards.Data
{
    public class User : IdentityUser<string>
    {
        public User()
        {
            this.Id = Guid.NewGuid()
                .ToString();

            this.Role = IdentityRole.User;
        }

        public virtual ICollection<UserCard> Cards { get; set; }
            = new HashSet<UserCard>();
    }
}