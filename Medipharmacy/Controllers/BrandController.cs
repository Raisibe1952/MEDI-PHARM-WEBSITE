using Medipharmacy.Data;
using Medipharmacy.Models;
using Medipharmacy.Modules;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medipharmacy.Controllers
{
    public class BrandController : Controller
    {
        private readonly DatabaseContext db;

        public BrandController(DatabaseContext db)
        {
            this.db = db;
        }

        [Route("brand/create")]
        public IActionResult CreatePartial()
        {
            return PartialView(new Brand());
        }

        [HttpPost]
        public IActionResult Add(Brand brand)
        {
            try
            {
                db.Brand.Where(b => b.Name.ToLower().Equals(brand.Name.ToLower())).First();
                return Redirect("~/brand?found=create");
            }
            catch (Exception)
            {
                db.Brand.Add(brand);
                db.SaveChanges();
                return Redirect("~/brand?success=create");
            }
        }

        [Route("brand")]
        public IActionResult Index()
        {
            var userId = Module.CheckUser(HttpContext, db);

            if(userId == 0)
            {
                return Redirect("/");
            }

            try
            {
                return View(db.Brand.ToList().OrderBy(b => b.Name));
            }
            catch (Exception)
            {
                return View();
            }
        }

        [HttpGet("brand/view/{id}")]
        public IActionResult ViewPartial(int id)
        {
            return PartialView(db.Brand.Find(id));
        }

        [HttpGet("brand/edit/{id}")]
        public IActionResult EditPartial(int id)
        {
            return PartialView(db.Brand.Find(id));
        }

        public IActionResult DeletePartial(int id)
        {
            return PartialView(db.Brand.Find(id));
        }

        public IActionResult Update(Brand brand)
        {
            var result = db.Brand.Find(brand.BrandId);
            if(result != null)
            {
                result.Name = brand.Name;
                result.Description = brand.Description;
                db.Brand.Update(result);
                db.SaveChanges();
                return Redirect("~/brand?success=update");
            }
            return Redirect("~/brand?notfound=update");
        }

        public IActionResult Remove(int id)
        {
            var brand = db.Brand.Find(id);
            if(brand != null)
            {
                db.Brand.Remove(brand);
                db.SaveChanges();
                return Redirect("~/brand?success=delete");
            }
            return Redirect("~/brand?notfound=delete");
        }
    }
}
