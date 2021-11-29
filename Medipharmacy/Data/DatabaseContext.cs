using Medipharmacy.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medipharmacy.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<Metric> Metric { get; set; }
        public DbSet<Unit> Unit { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Price> Price { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<Brand> Brand { get; set; }
        public DbSet<User> User { get; set; }
        //public DbSet<Profile> Profile { get; set; }
        //public DbSet<Cart> Cart { get; set; }
        //public DbSet<Sale> Sale { get; set; }
        //public DbSet<SaleProduct> SalePoduct { get; set; }
        public DbSet<Booking> Booking { get; set; }
        public DbSet<Medipharmacy.Models.Cart> Cart { get; set; }
        //public DbSet<Message> Message { get; set; } 
    }
}
