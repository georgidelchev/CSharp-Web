using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IRunes.Data
{
    public class Album
    {
        public Album()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        public string Cover { get; set; }

        [Required]
        public decimal Price { get; set; }

        public virtual ICollection<Track> Tracks { get; set; }
            = new HashSet<Track>();
    }
}