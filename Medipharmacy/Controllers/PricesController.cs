using Medipharmacy.Data;
using Medipharmacy.Models;
using Medipharmacy.Modules;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medipharmacy.Controllers
{
    public class PricesController : Controller
    {
        private readonly DatabaseContext db;

        public PricesController(DatabaseContext db)
        {
            this.db = db;
        }

        [Route("price/create")]
        public IActionResult CreatePartial()
        {
            List<Product> products = new List<Product>();
            products.Add(new Product(0, "", 0, 0, "Select Product", "", 0, 0, 0, DateTime.Now, DateTime.Now, ""));
            try
            {
                var product_list = db.Product.ToList().OrderBy(p => p.Name);
                foreach (var product in product_list)
                {
                    products.Add(product);
                }
            }
            catch (Exception)
            {
                products.Add(new Product(0, "", 0, 0, "No Products", "", 0, 0, 0, DateTime.Now, DateTime.Now, ""));
            }
            finally
            {
                ViewBag.products = products;
            }

            List<SelectListItem> promotions = new List<SelectListItem>()
            {
                new SelectListItem("true", "Promotion Price"),
                new SelectListItem("false", "Normal Price")
            };
            ViewBag.promotions = promotions;

            return PartialView();
        }

        [HttpPost]
        public IActionResult Add(Price price)
        {
            price.DateCreated = DateTime.Now;
            db.Price.Add(price);
            try
            {
                db.SaveChanges();
                return Redirect("~/price?success=create");
            }
            catch (Exception)
            {
                return Redirect("~/price?error=create");
            }
        }

        [Route("price")]
        public IActionResult Index()
        {
            var userId = Module.CheckUser(HttpContext, db);

            if (userId == 0)
            {
                return Redirect("/");
            }

            try
            {
                var prices = db.Price.ToList().OrderBy(u => u.DateCreated);
                var products = new List<Product>();
                foreach(var price in prices)
                {
                    var product = db.Product.Find(price.ProductId);
                    if(product != null)
                    {
                        products.Add(product);
                    }
                    else
                    {
                        products.Add(new Product(price.ProductId, "", 0, 0, "Not Found", "", 0, 0, 0, DateTime.Now, DateTime.Now, ""));
                    }
                }
                ViewBag.products = products;
                return View(prices);
            }
            catch (Exception)
            {
                return View(null);
            }
        }

        [Route("price/view/{id}")]
        public IActionResult ViewPartial(int id)
        {
            var price = db.Price.Find(id);
            if(price != null)
            {
                var product = db.Product.Find(price.ProductId);
                if(product != null)
                {
                    ViewBag.productName = db.Product.Find(price.ProductId).Name;
                }
                else
                {
                    ViewBag.productName = "Not Found";
                }
            }
            return PartialView(price);
        }

        [Route("price/edit/{id}")]
        public IActionResult EditPartial(int id)
        {
            var price = db.Price.Find(id);
            if(price != null)
            {
                List<Product> products = new List<Product>();
                products.Add(new Product(0, "", 0, 0, "Select Product", "", 0, 0, 0, DateTime.Now, DateTime.Now, ""));
                try
                {
                    var product_list = db.Product.ToList().OrderByDescending(p => p.Name);
                    foreach (var product in product_list)
                    {
                        products.Add(product);
                    }
                }
                catch (Exception)
                {
                    products.Add(new Product(0, "", 0, 0, "No Products", "", 0, 0, 0, DateTime.Now, DateTime.Now, ""));
                }
                finally
                {
                    ViewBag.products = products;
                }

                List<SelectListItem> promotions = new List<SelectListItem>()
                {
                    new SelectListItem("true", "Promotion Price"),
                    new SelectListItem("false", "Normal Price")
                };
                ViewBag.promotions = promotions;
            }
            return PartialView(price);
        }

        [HttpGet("price/delete/{id}")]
        public IActionResult DeletePartial(int id)
        {
            return PartialView(db.Price.Find(id));
        }

        public IActionResult Update(Price price)
        {
            var result = db.Price.Find(price.PriceId);
            if (result != null)
            {
                result.Amount = price.Amount;
                result.Promotion = price.Promotion;
                db.Price.Update(result);
                try
                {
                    db.SaveChanges();
                    return Redirect("~/price?success=update");
                }
                catch (Exception)
                {
                    return Redirect("~/price?error=update");
                }
            }
            return Redirect("~/price?notfound=update");
        }

        public IActionResult Remove(int id)
        {
            var result = db.Price.Find(id);
            if (result != null)
            {
                db.Price.Remove(result);
                try
                {
                    db.SaveChanges();
                    return Redirect("~/price?success=delete");
                }
                catch (Exception)
                {
                    return Redirect("~/price?error=delete");
                }
            }
            return Redirect("~/price?notfound=delete");
        }
    }
}
