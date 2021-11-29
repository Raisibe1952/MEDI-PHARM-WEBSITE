using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Medipharmacy.Models
{
    public class Inventory
    {
        [Key]
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InventoryId { get; set; }
        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int Sold { get; set; }

        public Inventory(int inventoryId, int productId, int quantity, int sold)
        {
            InventoryId = inventoryId;
            ProductId = productId;
            Quantity = quantity;
            Sold = sold;
        }

        public Inventory()
        {
        }
    }
}
