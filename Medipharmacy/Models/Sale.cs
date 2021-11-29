using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Medipharmacy.Models
{
    public class Sale
    {
        [Key]
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SaleId { get; set; }
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        [Required]
        public int Items { get; set; }
        [Required]
        public double Subtotal { get; set; }
        [Required]
        public double Tax { get; set; }
        [Required]
        public double Total { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }

        public Sale(int saleId, int userId, int items, double subtotal, double tax, double total, DateTime dateCreated)
        {
            SaleId = saleId;
            UserId = userId;
            Items = items;
            Subtotal = subtotal;
            Tax = tax;
            Total = total;
            DateCreated = dateCreated;
        }

        public Sale()
        {
        }
    }
}
