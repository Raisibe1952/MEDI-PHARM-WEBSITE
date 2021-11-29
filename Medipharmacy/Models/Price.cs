using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Medipharmacy.Models
{
    public class Price
    {
        [Key]
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PriceId { get; set; }
        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public bool Promotion { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }

        public Price(int priceId, int productId, double amount, bool promotion, DateTime dateCreated)
        {
            PriceId = priceId;
            ProductId = productId;
            Amount = amount;
            Promotion = promotion;
            DateCreated = dateCreated;
        }

        public Price()
        {
        }
    }
}
