using Medipharmacy.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medipharmacy.Controllers
{
    public class UserController : Controller
    {
        [HttpGet("user/login/get")]
        public IActionResult Index()
        {
            return PartialView(new Pair<string, string>());
        }

        public IActionResult Login(KeyValuePair<string, string> credentials)
        {
            var url = Request.Query["url"];
            System.Diagnostics.Debug.WriteLine(credentials.Key);
            
            System.Diagnostics.Debug.WriteLine(credentials.Value);

            return Redirect("/");
        }
    }
}
