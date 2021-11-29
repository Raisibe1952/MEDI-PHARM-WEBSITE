using Medipharmacy.Data;
using Medipharmacy.Models;
using Microsoft.AspNetCore.Mvc;
using Medipharmacy.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Medipharmacy.ViewModels;

namespace Medipharmacy.Controllers
{
    public class DashboardController : Controller
    {
        private DatabaseContext db;

        public DashboardController(DatabaseContext db)
        {
            this.db = db;
        }

        public IActionResult Index(List<Product> product_list)
        {
            var userId = 0;

            var strUserId = HttpContext.Session.GetInt32("UserId");
            if(strUserId.HasValue)
            {
                userId = strUserId.Value;

                if(userId != 0)
                {
                    var user = db.User.Find(userId);
                    if(user != null)
                    {
                        if (user.Super)
                        {
                            return Redirect("/inentory");
                        }
                    }
                }
            }

            var categories = new List<Category>();
            try
            {
                categories = db.Category.OrderBy(c => c.Name).ToList();
            }
            catch (Exception)
            {
                categories.Add(new Category(0, "No Category Found", ""));
            }
            ViewBag.categories = categories;

            var brands = new List<Brand>();
            try
            {
                brands = db.Brand.OrderBy(b => b.Name).ToList();
            }
            catch (Exception)
            {
                brands.Add(new Brand(0, "No Brands Found", ""));
            }
            ViewBag.brands = brands;

            var products = new List<ProductViewModel>();
            try
            {
                if(product_list == null)
                {
                    ViewBag.results = "No products found...";
                }
                else if (product_list.Count == 0)
                {
                    product_list = db.Product.OrderByDescending(p => p.Image).ToList();
                    
                    foreach (var product in product_list)
                    {
                        products.Add(SetProduct(product));
                    }
                }
                else
                {
                    foreach (var product in product_list)
                    {
                        products.Add(SetProduct(product));
                    }
                }
                
            } catch (Exception)
            {
                products = null;
            }

            ViewBag.products = products;

            return View("Index", products);
        }

        [HttpGet("dashbord/search/{text}")]
        public IActionResult Search(string text)
        {
            try
            {
                var products = db.Product.Where(p => p.Name.ToLower().Contains(text.ToLower()) || p.Description.ToLower().Contains(text.ToLower())).ToList();
                if(products.Count <= 0)
                {
                    return Index(null);
                }
                ViewBag.results = "Search results for, \"" + text + "\"";
                return Index(products);
            } catch(Exception) 
            {
                return Index(null);
            }
        }

        [HttpGet("dashboard/brand/{brandId}")]
        public IActionResult GetByBrand(int brandId)
        {
            try
            {
                var products = db.Product.Where(p => p.BrandId.Equals(brandId)).ToList();
                if(products.Count <= 0)
                {
                    return Index(null);
                }
                ViewBag.results = "Showing products by brand:  \"" + db.Brand.Find(brandId).Name + "\"";
                return Index(products);
            }
            catch (Exception)
            {
                return Index(null);
            }
        }

        [HttpGet("dashboard/category/{categoryId}")]
        public IActionResult GetByCategory(int categoryId)
        {
            try
            {
                var products = db.Product.Where(p => p.CategoryId.Equals(categoryId)).ToList();
                if(products.Count <= 0)
                {
                    return Index(null);
                }

                ViewBag.results = "Showing products by category:  \"" + db.Category.Find(categoryId).Name + "\"";
                return Index(products);
            }
            catch (Exception)
            {
                return Index(null);
            }
        }

        private ProductViewModel SetProduct(Product product)
        {
            var productViewModel = new ProductViewModel();

            productViewModel.ProductId = product.ProductId;
            productViewModel.Name = product.Name;
            productViewModel.Description = product.Description;
            productViewModel.Size = product.Size.ToString();
            productViewModel.Image = product.Image;

            try
            {
                productViewModel.Metric = db.Metric.Where(m => m.MetricId.Equals(product.MetricId)).First().Name;
            }
            catch (Exception)
            {
                productViewModel.Metric = "Size";
            }

            try
            {
                productViewModel.Unit = db.Unit.Where(u => u.UnitId.Equals(product.UnitId)).First().Code;
            }
            catch (Exception)
            {
                productViewModel.Unit = "Units";
            }

            try
            {
                productViewModel.Price = db.Price.Where(p => p.ProductId.Equals(product.ProductId)).OrderByDescending(o => o.DateCreated).First().Amount.ToString("0.00");
            }
            catch (Exception)
            {
                productViewModel.Price = "0.00";
            }

            try
            {
                var inventory = db.Inventory.Where(i => i.ProductId.Equals(product.ProductId)).First();
                productViewModel.Remaining = inventory.Quantity - inventory.Sold;
                if(inventory.Quantity > inventory.Sold)
                {
                    productViewModel.InStock = true;
                }
            }
            catch (Exception)
            {
                
            }

            return productViewModel;
        }

        [HttpGet("dashboard/product/view/{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = db.Product.Find(id);

            if(product != null)
            {
                try
                {
                    ViewBag.price = db.Price.Where(p => p.ProductId == product.ProductId).OrderByDescending(d => d.DateCreated).First().Amount.ToString("0.00");
                }
                catch (Exception)
                {
                    ViewBag.price = "0.00";
                }
                
                try
                {
                    ViewBag.category = db.Category.Where(p => p.CategoryId == product.CategoryId).First().Name;
                }
                catch (Exception)
                {
                    ViewBag.category = "Uncategorized";
                }
                
                try
                {
                    ViewBag.brand = db.Brand.Where(p => p.BrandId == product.BrandId).First().Name;
                }
                catch (Exception)
                {
                    ViewBag.brand = "Unbranded";
                }
                
                try
                {
                    ViewBag.metric = db.Metric.Where(p => p.MetricId == product.MetricId).First().Name;
                }
                catch (Exception)
                {
                    ViewBag.metric = "Size";
                }
                
                try
                {
                    ViewBag.units = db.Unit.Where(p => p.UnitId == product.UnitId).First().Code;
                }
                catch (Exception)
                {
                    ViewBag.units = "Units";
                }

                try
                {
                    ViewBag.inventory = db.Inventory.Where(i => i.ProductId.Equals(product.ProductId)).First();
                } catch (Exception)
                {
                    ViewBag.inventory = new Inventory(0, product.ProductId, 0, 0);
                }
                
                return PartialView(product);
            }
            return PartialView();
        }
    }
}