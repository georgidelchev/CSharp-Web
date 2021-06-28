using System.ComponentModel.DataAnnotations;

namespace MishMash.Data
{
    public class ChannelTag
    {
        [Key]
        public int Id { get; set; }

        public int ChannelId { get; set; }

        public virtual Channel Channel { get; set; }

        public int TagId { get; set; }

        public virtual Tag Tag { get; set; }
    }
}