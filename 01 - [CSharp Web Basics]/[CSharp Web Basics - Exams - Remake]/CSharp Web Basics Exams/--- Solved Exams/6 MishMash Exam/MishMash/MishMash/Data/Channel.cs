using MishMash.Data.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MishMash.Data
{
    public class Channel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string Name { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Description { get; set; }

        [Required]
        public Type Type { get; set; }

        public virtual ICollection<ChannelTag> Tags { get; set; }
            = new HashSet<ChannelTag>();

        public virtual ICollection<UserChannel> Followers { get; set; }
            = new HashSet<UserChannel>();
    }
}