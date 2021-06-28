using System.ComponentModel.DataAnnotations;

namespace MishMash.Data
{
    public class UserChannel
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }

        public int ChannelId { get; set; }

        public virtual Channel Channel { get; set; }
    }
}