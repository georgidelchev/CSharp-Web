using System;
using Panda.Data.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Panda.Data
{
    public class Package
    {
        public Package()
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
        public string Description { get; set; }

        [Required]
        public decimal Weight { get; set; }

        [Required]
        public string ShippingAddress { get; set; }

        [Required]
        public Status Status { get; set; }

        [Required]
        public DateTime EstimatedDeliveryDate { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public string RecipientId { get; set; }

        public virtual User Recipient { get; set; }
    }
}
