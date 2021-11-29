using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Medipharmacy.Models
{
    public class Product
    {
        [Key]
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }
        [MaxLength(length:24)]
        public string Code { get; set; }
        [Required]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        [Required]
        [ForeignKey("Brand")]
        public int BrandId { get; set; }
        [Required]
        [MaxLength(length: 100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(length:250)]
        public string Description { get; set; }
        [Required]
        [ForeignKey("Metric")]
        public int MetricId { get; set; }
        [Required]
        [ForeignKey("Unit")]
        public int UnitId { get; set; }
        [Required]
        public double Size { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        [Required]
        public DateTime DateUpdated { get; set; }
        [Required]
        public string Image { get; set; }

        public Product(int productId, string code, int categoryId, int brandId, string name, string description, int metricId, int unitId, double size, DateTime dateCreated, DateTime dateUpdated, string image)
        {
            ProductId = productId;
            Code = code;
            CategoryId = categoryId;
            BrandId = brandId;
            Name = name;
            Description = description;
            MetricId = metricId;
            UnitId = unitId;
            Size = size;
            DateCreated = dateCreated;
            DateUpdated = dateUpdated;
            Image = image;
        }

        public Product()
        {
        }
    }
}
