using Medipharmacy.Data;
using Microsoft.AspNetCore.Mvc;

namespace Medipharmacy.Controllers
{
    public class SaleController : Controller
    {
        private DatabaseContext db;

        public SaleController(DatabaseContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
