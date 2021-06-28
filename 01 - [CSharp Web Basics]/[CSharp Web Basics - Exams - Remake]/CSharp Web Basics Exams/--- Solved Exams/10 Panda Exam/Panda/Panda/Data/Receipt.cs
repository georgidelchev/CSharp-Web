using System;
using System.ComponentModel.DataAnnotations;

namespace Panda.Data
{
    public class Receipt
    {
        public Receipt()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        public decimal Fee { get; set; }

        [Required]
        public DateTime IssuedOn { get; set; }

        [Required]
        public string RecipientId { get; set; }

        public virtual User Recipient { get; set; }

        [Required]
        public string PackageId { get; set; }

        public virtual Package Package { get; set; }
    }
}