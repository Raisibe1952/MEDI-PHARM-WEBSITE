using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Medipharmacy.Models
{
    public class SaleProduct
    {
        [Key]
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SaleProductId { get; set; }
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        [Required]
        [ForeignKey("Sale")]
        public int SaleId { get; set; }
        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        [Required]
        [ForeignKey("Price")]
        public int PriceId { get; set; }
        [Required]
        public int Quantity { get; set; }

        public SaleProduct(int saleProductId, int userId, int saleId, int productId, int priceId, int quantity)
        {
            SaleProductId = saleProductId;
            UserId = userId;
            SaleId = saleId;
            ProductId = productId;
            PriceId = priceId;
            Quantity = quantity;
        }

        public SaleProduct()
        {
        }
    }
}
