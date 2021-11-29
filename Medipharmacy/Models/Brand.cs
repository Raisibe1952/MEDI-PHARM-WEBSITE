using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Medipharmacy.Models
{
    public class Brand
    {
        [Key]
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BrandId { get; set; }
        [Required]
        [MaxLength(length: 100)]
        public string Name { get; set; }
        [MaxLength(length: 250)]
        public string Description { get; set; }

        public Brand(int brandId, string name, string description)
        {
            BrandId = brandId;
            Name = name;
            Description = description;
        }

        public Brand()
        {
        }
    }
}
