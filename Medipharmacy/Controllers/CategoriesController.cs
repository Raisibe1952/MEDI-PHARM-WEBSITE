using Medipharmacy.Data;
using Medipharmacy.Models;
using Medipharmacy.Modules;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medipharmacy.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly DatabaseContext db;

        public CategoriesController(DatabaseContext db)
        {
            this.db = db;
        }

        [Route("category/create")]
        public IActionResult CreatePartial()
        {
            return PartialView("CreatePartial", new Category());
        }

        [HttpPost]
        public IActionResult Add(Category category)
        {
            try
            {
                db.Category.Where(c => c.Name.ToLower().Equals(category.Name.ToLower())).First();
                return Redirect("~/category?found=create");
            }
            catch (Exception)
            {
                db.Category.Add(category);
                try
                {
                    db.SaveChanges();
                    return Redirect("~/category?success=create");
                }
                catch (Exception)
                {
                    return Redirect("~/category?error=create");
                }
            }
        }

        [Route("category")]
        public IActionResult Index()
        {
            var userId = Module.CheckUser(HttpContext, db);

            if (userId == 0)
            {
                return Redirect("/");
            }

            try
            {
                return View(db.Category.ToList().OrderBy(c => c.Name));
            }
            catch (Exception)
            {
                return View(null);
            }
        }

        [HttpGet("category/view/{id}")]
        public IActionResult ViewPartial(int id)
        {
            return PartialView(db.Category.Find(id));
        }

        [HttpGet("category/edit/{id}")]
        public IActionResult EditPartial(int id)
        {
            return PartialView(db.Category.Find(id));
        }

        [HttpGet("category/delete/{id}")]
        public IActionResult DeletePartial(int id)
        {
            return PartialView(db.Category.Find(id));
        }

        public IActionResult Update(Category category)
        {
            var result = db.Category.Find(category.CategoryId);
            if(result != null)
            {
                result.Name = category.Name;
                result.Description = category.Description;
                db.Category.Update(result);
                try
                {
                    db.SaveChanges();
                    return Redirect("~/category?success=update");
                }
                catch (Exception)
                {
                    return Redirect("~/category?error=update");
                }
            }
            return Redirect("~/category?notfound=update");
        }

        public IActionResult Remove(int id)
        {
            var result = db.Category.Find(id);
            if(result != null)
            {
                db.Category.Remove(result);
                try
                {
                    db.SaveChanges();
                    return Redirect("~/category?success=delete");
                }
                catch (Exception)
                {
                    return Redirect("~/category?error=delete");
                }
            }
            return Redirect("~/category?notfound=delete");
        }
    }
}
