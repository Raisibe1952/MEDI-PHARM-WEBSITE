using Medipharmacy.Data;
using Medipharmacy.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Medipharmacy.Modules;

namespace Medipharmacy.Controllers
{
    public class ProductsController : Controller
    {
        private readonly DatabaseContext db;
        private readonly IWebHostEnvironment whe;

        public ProductsController(DatabaseContext db, IWebHostEnvironment whe)
        {
            this.db = db;
            this.whe = whe;
        }

        [Route("product/create")]
        public IActionResult CreatePartial()
        {
            List<Category> categories = new List<Category>();
            categories.Add(new Category(0, "Select Category", ""));
            try
            {
                var category_list = db.Category.ToList().OrderBy(c => c.Name);
                foreach (var category in category_list)
                {
                    categories.Add(category);
                }
            }
            catch (Exception)
            {
                categories.Add(new Category(0, "No Categories Found", ""));
            }
            finally
            {
                ViewBag.categories = categories;
            }

            List<Brand> brands = new List<Brand>();
            brands.Add(new Brand(0, "Select Brand", ""));
            try
            {
                var brand_list = db.Brand.ToList();
                foreach(var brand in brand_list)
                {
                    brands.Add(brand);
                }
            }
            catch (Exception)
            {
                brands.Add(new Brand(0, "No Brands Found", ""));
            }
            finally
            {
                ViewBag.brands = brands;
            }

            List<Metric> metrics = new List<Metric>();
            metrics.Add(new Metric(0, "Select Measurement", ""));
            try
            {
                var metric_list = db.Metric.ToList().OrderBy(c => c.Name);
                foreach (var metric in metric_list)
                {
                    metrics.Add(metric);
                }
            }
            catch (Exception)
            {
                metrics.Add(new Metric(0, "No Measurements Found", ""));
            }
            finally
            {
                ViewBag.metrics = metrics;
            }

            List<Unit> units = new List<Unit>();
            units.Add(new Unit(0, 0, "Select Unit", "", ""));
            try
            {
                var unit_list = db.Unit.ToList().OrderBy(c => c.Name);
                foreach (var unit in unit_list)
                {
                    units.Add(unit);
                }
            }
            catch (Exception)
            {
                units.Add(new Unit(0, 0, "No Units Found", "", ""));
            }
            finally
            {
                ViewBag.units = units;
            }

            return PartialView();
        }

        private string GenerateCode(Product product)
        {
            string code = product.DateCreated.ToString("yyyy") + product.DateCreated.ToString("MM") + product.DateCreated.ToString("dd") + product.DateCreated.ToString("HH") + product.DateCreated.ToString("mm") + product.DateCreated.ToString("ss") + product.DateCreated.ToString("tt") + product.Name.Substring(0, 3).ToUpper();
            return code;
        }

        [HttpPost]
        public IActionResult Add(Product product)
        {
            product.DateCreated = DateTime.Now;
            product.DateUpdated = DateTime.Now;
            product.Code = GenerateCode(product);
            try
            {
                db.Product.Where(p => p.Name.ToLower().Equals(product.Name.ToLower()) && p.BrandId.Equals(product.BrandId) && p.MetricId.Equals(product.MetricId) && p.UnitId.Equals(product.UnitId)).First();
                return Redirect("~/product?found=create");
            }
            catch (Exception)
            {
                try
                {
                    var files = HttpContext.Request.Form.Files;
                    string imageUrlRel = @"~/img/product/default.png";
                    string imageUrlAbs = Path.Combine(whe.WebRootPath, "img", "product", "default.png");
                    if (files.Count > 0)
                    {
                        string imageName = Guid.NewGuid().ToString() + Path.GetExtension(files[0].FileName);
                        imageUrlAbs = Path.Combine(whe.WebRootPath, "img", "product", imageName);
                        imageUrlRel = @"~/img/product/" + imageName;
                        using (var filestream = new FileStream(imageUrlAbs, FileMode.Create))
                        {
                            files[0].CopyTo(filestream);
                        }
                    }
                    product.Image = imageUrlRel;
                    db.Product.Add(product);
                    db.SaveChanges();

                    int productId = db.Product.Where(p => p.Name.ToLower().Equals(product.Name.ToLower()) && p.MetricId.Equals(product.MetricId) && p.UnitId.Equals(product.UnitId)).First().ProductId;
                    db.Inventory.Add(new Inventory(0, productId, 0, 0));
                    db.SaveChanges();

                    return Redirect("~/product?success=create");
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.StackTrace);
                    return Redirect("~/product?error=create");
                }
            }
        }

        [Route("product")]
        public IActionResult Index()
        {
            var userId = Module.CheckUser(HttpContext, db);

            if (userId == 0)
            {
                return Redirect("/");
            }

            try
            {
                var products = db.Product.ToList().OrderBy(p => p.Name);
                var brands = new List<Brand>();
                var categories = new List<Category>();
                var units = new List<Unit>();
                foreach(var product in products)
                {
                    var category = db.Category.Find(product.CategoryId);
                    if(category != null)
                    {
                        categories.Add(category);
                    }
                    else
                    {
                        categories.Add(new Category(product.CategoryId, "Not Found", "Not Found"));
                    }

                    var unit = db.Unit.Find(product.UnitId);
                    if(unit != null)
                    {
                        units.Add(unit);
                    }
                    else
                    {
                        units.Add(new Unit(product.UnitId, 0, "Not Found", "Not Found", "Not Found"));
                    }

                    var brand = db.Brand.Find(product.BrandId);
                    if (brand != null)
                    {
                        brands.Add(brand);
                    }
                    else
                    {
                        brands.Add(new Brand(product.BrandId, "Not Found", "Not Found"));
                    }
                }

                ViewBag.categories = categories;
                ViewBag.brands = brands;
                ViewBag.units = units;
                return View(products);
            }
            catch (Exception)
            {
                return View();
            }
        }

        [Route("product/view/{id}")]
        public IActionResult ViewPartial(int id)
        {
            var product = db.Product.Find(id);
            if(product != null)
            {
                ViewBag.categoryName = db.Category.Find(product.CategoryId).Name;
                ViewBag.brandName = db.Brand.Find(product.BrandId).Name;
                ViewBag.metricName = db.Metric.Find(product.MetricId).Name;
                ViewBag.unitName = db.Unit.Find(product.UnitId).Name;
            }
            return PartialView(product);
        }

        [Route("product/edit/{id}")]
        public IActionResult EditPartial(int id)
        {
            var product = db.Product.Find(id);
            if(product != null)
            {
                List<Category> categories = new List<Category>();
                categories.Add(new Category(0, "Select Category", ""));
                try
                {
                    var category_list = db.Category.ToList().OrderBy(c => c.Name);
                    foreach (var category in category_list)
                    {
                        categories.Add(category);
                    }
                }
                catch (Exception)
                {
                    categories.Add(new Category(0, "No Categories Found", ""));
                }
                finally
                {
                    ViewBag.categories = categories;
                }

                List<Metric> metrics = new List<Metric>();
                metrics.Add(new Metric(0, "Select Measurement", ""));
                try
                {
                    var metric_list = db.Metric.ToList().OrderBy(c => c.Name);
                    foreach (var metric in metric_list)
                    {
                        metrics.Add(metric);
                    }
                }
                catch (Exception)
                {
                    metrics.Add(new Metric(0, "No Measurements Found", ""));
                }
                finally
                {
                    ViewBag.metrics = metrics;
                }

                List<Unit> units = new List<Unit>();
                units.Add(new Unit(0, 0, "Select Unit", "", ""));
                try
                {
                    var unit_list = db.Unit.ToList().OrderBy(c => c.Name);
                    foreach (var unit in unit_list)
                    {
                        units.Add(unit);
                    }
                }
                catch (Exception)
                {
                    units.Add(new Unit(0, 0, "No Units Found", "", ""));
                }
                finally
                {
                    ViewBag.units = units;
                }
            }
            return PartialView(db.Product.Find(id));
        }

        [HttpGet("product/delete/{id}")]
        public IActionResult DeletePartial(int id)
        {
            return PartialView(db.Product.Find(id));
        }

        public IActionResult Update(Product product)
        {
            var result = db.Product.Find(product.ProductId);
            if(result != null)
            {
                result.Name = product.Name;
                result.BrandId = product.BrandId;
                result.Description = product.Description;
                result.MetricId = product.MetricId;
                result.UnitId = product.UnitId;
                result.Size = product.Size;
                result.CategoryId = product.CategoryId;
                string imageUrlAbs = Path.Combine(whe.WebRootPath, "img", "product", result.Image.Substring(result.Image.LastIndexOf("/") + 1));
                try
                {
                    var files = HttpContext.Request.Form.Files;
                    if (files.Count > 0)
                    {
                        System.IO.File.Delete(imageUrlAbs);
                        string imageName = Guid.NewGuid().ToString() + Path.GetExtension(files[0].FileName);
                        imageUrlAbs = Path.Combine(whe.WebRootPath, "img", "product", imageName);
                        string imageUrlRel = @"~/img/product/" + imageName;
                        using (var filestream = new FileStream(imageUrlAbs, FileMode.Create))
                        {
                            files[0].CopyTo(filestream);
                        }
                        result.Image = imageUrlRel;
                    }

                    db.Product.Update(result);
                    db.SaveChanges();
                    return Redirect("~/product?success=update");
                }
                catch (Exception)
                {
                    return Redirect("~/product?error=update");
                }
            }
            return Redirect("~/product?notfound=update");
        }

        public IActionResult Remove(int id)
        {
            var product = db.Product.Find(id);
            if (product != null)
            {
                try
                {
                    string imageName = product.Image.Substring(product.Image.LastIndexOf("/") + 1);
                    string imageUrlAbs = Path.Combine(whe.WebRootPath, "img", "product", imageName);
                    System.IO.File.Delete(imageUrlAbs);

                    try
                    {
                        var inventory = db.Inventory.Where(i => i.ProductId.Equals(product.ProductId)).First();
                        db.Inventory.Remove(inventory);
                    }
                    catch (Exception)
                    {}

                    db.Product.Remove(product);
                    db.SaveChanges();
                    return Redirect("~/product?success=delete");
                }
                catch (Exception)
                {
                    return Redirect("~/product?error=delete");
                }
            }
            return Redirect("~/product?notfound=delete");
        }
    }
}
