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
    public class UnitsController : Controller
    {
        private readonly DatabaseContext db;

        public UnitsController(DatabaseContext db)
        {
            this.db = db;
        }

        [Route("unit/create")]
        public IActionResult CreatePartial()
        {
            List<Metric> metrics = new List<Metric>();
            metrics.Add(new Metric(0, "Select Measurement", ""));
            try
            {
                var metric_list = db.Metric.ToList().OrderBy(m => m.Name);
                foreach(var metric in metric_list)
                {
                    metrics.Add(metric);
                }
            }
            catch (Exception)
            {
                metrics.Add(new Metric(0, "No Measurements", ""));
            }
            finally
            {
                ViewBag.metrics = metrics;
            }
            return PartialView();
        }

        [HttpPost]
        public IActionResult Add(Unit unit)
        {
            try
            {
                db.Unit.Where(c => c.Name.ToLower().Equals(unit.Name.ToLower())).First();
                return Redirect("~/unit?found=create");
            }
            catch (Exception)
            {
                db.Unit.Add(unit);
                try
                {
                    db.SaveChanges();
                    return Redirect("~/unit?success=create");
                }
                catch (Exception)
                {
                    return Redirect("~/unit?error=create");
                }
            }
        }

        [Route("unit")]
        public IActionResult Index()
        {
            var userId = Module.CheckUser(HttpContext, db);

            if (userId == 0)
            {
                return Redirect("/");
            }

            try
            {
                var units = db.Unit.ToList().OrderBy(u => u.Name);
                var metrics = new List<Metric>();
                foreach(var unit in units)
                {
                    var metric = db.Metric.Find(unit.MetricId);
                    if(metric != null)
                    {
                        metrics.Add(metric);
                    }
                    else
                    {
                        metrics.Add(new Metric(unit.MetricId, "Not Found", "Not Found"));
                    }
                }
                ViewBag.metrics = metrics;
                return View(units);
            }
            catch (Exception)
            {
                return View(null);
            }
        }

        [Route("unit/view/{id}")]
        public IActionResult ViewPartial(int id)
        {
            var unit = db.Unit.Find(id);
            if(unit != null)
            {
                var metric = db.Metric.Find(unit.MetricId);
                if(metric != null)
                {
                    ViewBag.metricName = metric.Name;
                }
                else
                {
                    ViewBag.metricName = "Not Found";
                }
            }
            return PartialView(unit);
        }

        [Route("unit/edit/{id}")]
        public IActionResult EditPartial(int id)
        {
            var unit = db.Unit.Find(id);
            if(unit != null)
            {
                List<Metric> metrics = new List<Metric>();
                metrics.Add(new Metric(0, "Select Measurement", ""));
                try
                {
                    var metric_list = db.Metric.ToList().OrderBy(m => m.Name);
                    foreach (var metric in metric_list)
                    {
                        metrics.Add(metric);
                    }
                }
                catch (Exception)
                {
                    metrics.Add(new Metric(0, "No Measurements", ""));
                }
                finally
                {
                    ViewBag.metrics = metrics;
                }
            }
            return PartialView(unit);
        }

        [HttpGet("unit/delete/{id}")]
        public IActionResult DeletePartial(int id)
        {
            return PartialView(db.Unit.Find(id));
        }

        public IActionResult Update(Unit unit)
        {
            var result = db.Unit.Find(unit.UnitId);
            if (result != null)
            {
                result.Name = unit.Name;
                result.Description = unit.Description;
                db.Unit.Update(result);
                try
                {
                    db.SaveChanges();
                    return Redirect("~/unit?success=update");
                }
                catch (Exception)
                {
                    return Redirect("~/unit?error=update");
                }
            }
            return Redirect("~/unit?notfound=update");
        }

        public IActionResult Remove(int id)
        {
            var result = db.Unit.Find(id);
            if (result != null)
            {
                db.Unit.Remove(result);
                try
                {
                    db.SaveChanges();
                    return Redirect("~/unit?success=delete");
                }
                catch (Exception)
                {
                    return Redirect("~/unit?error=delete");
                }
            }
            return Redirect("~/unit?notfound=delete");
        }
    }
}
