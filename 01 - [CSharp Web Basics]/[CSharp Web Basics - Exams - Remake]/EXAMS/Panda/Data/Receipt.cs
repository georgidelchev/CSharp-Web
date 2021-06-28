using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Panda.Data
{
    public class Receipt
    {
        public Receipt()
        {
            this.Id = Guid
                .NewGuid()
                .ToString();
        }

        [Key]
        [Required]
        public string Id { get; set; }

        [Required]
        public decimal Fee { get; set; }

        [Required]
        public DateTime IssuedOn { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public string RecipientId { get; set; }

        public virtual User Recipient { get; set; }

        [Required]
        [ForeignKey(nameof(Package))]
        public string PackageId { get; set; }

        public virtual Package Package { get; set; }
    }
}