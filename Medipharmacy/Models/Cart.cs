using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Medipharmacy.Models
{
    public class Cart
    {
        [Key]
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartId { get; set; }
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        [ForeignKey("Price")]
        public int PriceId { get; set; }
        [Required]
        public bool Checkedout { get; set; }

        public Cart(int cartId, int userId, int productId, int quantity, int priceId, bool checkedout)
        {
            CartId = cartId;
            UserId = userId;
            ProductId = productId;
            Quantity = quantity;
            PriceId = priceId;
            Checkedout = checkedout;
        }

        public Cart()
        {
        }
    }
}
