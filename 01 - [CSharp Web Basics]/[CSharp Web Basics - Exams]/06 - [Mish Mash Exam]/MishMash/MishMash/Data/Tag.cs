using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MishMash.Data
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string Name { get; set; }

        public virtual ICollection<ChannelTag> Channels { get; set; }
            = new HashSet<ChannelTag>();
    }
}