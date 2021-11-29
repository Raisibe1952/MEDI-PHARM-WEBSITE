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
    public class MetricsController : Controller
    {
        private readonly DatabaseContext db;

        public MetricsController(DatabaseContext db)
        {
            this.db = db;
        }

        [Route("metric/create")]
        public IActionResult CreatePartial()
        {
            return PartialView(new Metric());
        }

        [HttpPost]
        public IActionResult Add(Metric metric)
        {
            try
            {
                db.Metric.Where(c => c.Name.ToLower().Equals(metric.Name.ToLower())).First();
                return Redirect("~/metric?found=create");
            }
            catch (Exception)
            {
                db.Metric.Add(metric);
                try
                {
                    db.SaveChanges();
                    return Redirect("~/metric?success=create");
                }
                catch (Exception)
                {
                    return Redirect("~/metric?error=create");
                }
            }
        }

        [Route("metric")]
        public IActionResult Index()
        {
            var userId = Module.CheckUser(HttpContext, db);

            if (userId == 0)
            {
                return Redirect("/");
            }

            try
            {
                return View(db.Metric.ToList());
            }
            catch (Exception)
            {
                return View(null);
            }
        }

        [HttpGet("metric/view/{id}")]
        public IActionResult ViewPartial(int id)
        {
            return PartialView(db.Metric.Find(id));
        }

        [HttpGet("metric/edit/{id}")]
        public IActionResult EditPartial(int id)
        {
            return PartialView(db.Metric.Find(id));
        }

        [HttpGet("metric/delete/{id}")]
        public IActionResult DeletePartial(int id)
        {
            return PartialView(db.Metric.Find(id));
        }

        public IActionResult Update(Metric metric)
        {
            var result = db.Metric.Find(metric.MetricId);
            if (result != null)
            {
                result.Name = metric.Name;
                result.Description = metric.Description;
                db.Metric.Update(result);
                try
                {
                    db.SaveChanges();
                    return Redirect("~/metric?success=update");
                }
                catch (Exception)
                {
                    return Redirect("~/metric?error=update");
                }
            }
            return Redirect("~/metric?notfound=update");
        }

        public IActionResult Remove(int id)
        {
            var result = db.Metric.Find(id);
            if (result != null)
            {
                db.Metric.Remove(result);
                try
                {
                    db.SaveChanges();
                    return Redirect("~/metric?success=delete");
                }
                catch (Exception)
                {
                    return Redirect("~/metric?error=delete");
                }
            }
            return Redirect("~/metric?notfound=delete");
        }
    }
}
