using Medipharmacy.Data;
using Medipharmacy.Models;
using Medipharmacy.Modules;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Medipharmacy.Controllers
{
    public class InventoriesController : Controller
    {
        private readonly DatabaseContext db;

        public InventoriesController(DatabaseContext db)
        {
            this.db = db;
        }

        [Route("inventory")]
        public IActionResult Index()
        {
            var userId = Module.CheckUser(HttpContext, db);

            if (userId == 0)
            {
                return Redirect("/");
            }

            try
            {
                var inventories = db.Inventory.ToList().OrderBy(i => i.InventoryId);
                List<Product> products = new List<Product>();
                foreach(var inventory in inventories)
                {
                    products.Add(db.Product.Find(inventory.ProductId));
                }
                ViewBag.Products = products;
                return View(inventories);
            }
            catch (Exception)
            {
                return View();
            }
        }

        [HttpGet("inventory/get/{id}")]
        public IActionResult EditPartial(int id)
        {
            var inventory = db.Inventory.Find(id);
            var productName = db.Product.Find(inventory.ProductId).Name;
            if(productName != null)
            {
                ViewBag.ProductName = productName.ToUpper();
            }
            else
            {
                ViewBag.ProductName = "PRODUCT";
            }
            return PartialView(inventory);
        }

        public IActionResult Update(Inventory inventory)
        {
            var result = db.Inventory.Find(inventory.InventoryId);
            if(result != null)
            {
                result.Quantity += inventory.Quantity;
                db.Inventory.Update(result);
                db.SaveChanges();

                return Redirect("~/inventory?success=update");
            }
            else
            {
                return Redirect("~/inventory?notfound=update");
            }
        }
    }
}
