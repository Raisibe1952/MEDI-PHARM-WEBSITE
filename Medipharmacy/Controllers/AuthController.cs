using Medipharmacy.Data;
using Medipharmacy.Models;
using Medipharmacy.Modules;
using Medipharmacy.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Medipharmacy.Controllers
{
    public class AuthController : Controller
    {
        private DatabaseContext db;

        public AuthController(DatabaseContext db)
        {
            this.db = db;
        }

        [HttpGet("auth/account")]
        public IActionResult AccountPartial()
        {
            var userId = Module.CheckCustomer(HttpContext);
            if(userId != 0)
            {
                return PartialView(db.User.Find(userId));
            }
            ViewBag.message = "You need to be signed in to view account";
            return PartialView("SigninPartial");
        }

        [HttpGet("auth/signinform")]
        public IActionResult SigninPartial()
        {
            return PartialView("SigninPartial");
        }

        [HttpGet("auth/signupform")]
        public IActionResult SignupPartial()
        {
            return PartialView("SignupPartial");
        }

        [HttpGet("auth/signout")]
        public IActionResult Signout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }

        [HttpPost("auth/signin")]
        public IActionResult Signin()
        {
            var strJson = HttpContext.Request.Form.Take(1).First().Key;
            var pair = ConvertToPair(strJson);
            pair.Key = pair.Key.ToLower();
            pair.Value = Module.HashPassword(pair.Value);
            if(pair != null)
            {
                try
                {
                    var user = db.User.Where(u => u.Email.Equals(pair.Key) && u.Password.Equals(pair.Value)).First();
                    HttpContext.Session.SetInt32("UserId", user.UserId);
                    return PartialView("StatusPartial", new StatusViewModel(Module.Status.SUCCESS, "Welcome back, " + user.Firstname +"..."));
                } catch (Exception)
                {
                    return PartialView("StatusPartial", new StatusViewModel(Module.Status.FAILURE, "Incorrect password/email!"));
                }
            }

            return PartialView("StatusPartial", new StatusViewModel(Module.Status.FAILURE, "Form is invalid!"));
        }

        private Pair<string, string> ConvertToPair(string strJson)
        {
            strJson = strJson.Remove(strJson.LastIndexOf(','), strJson.Length - strJson.LastIndexOf(',')) + '}';

            try
            {
                return JsonSerializer.Deserialize<Pair<string, string>>(strJson);
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost("auth/signup")]
        public IActionResult Signup()
        {
            var strJson = HttpContext.Request.Form.Take(1).First().Key;
            var user = ConvertToUser(strJson);
            user.Email = user.Email.Trim().ToLower();
            user.Password = Module.HashPassword(user.Password);
            if(user != null)
            {
                try
                {
                    var ckeckUser = db.User.Where(u => u.Email.Equals(user.Email)).First();
                    return PartialView("StatusPartial", new StatusViewModel(Module.Status.WARNING, "User with provided email already exists!"));
                }
                catch (Exception)
                {
                    try
                    {
                        db.User.Add(user);
                        db.SaveChanges();
                        return PartialView("StatusPartial", new StatusViewModel(Module.Status.SUCCESS, "User created successfully..."));
                    } catch (Exception)
                    {
                        return PartialView("StatusPartial", new StatusViewModel(Module.Status.WARNING, "Server error occured, try again later!"));
                    }
                }
            }

            return PartialView("StatusPartial", new StatusViewModel(Module.Status.FAILURE, "Form is invalid!"));
        }

        private User ConvertToUser(string strJson)
        {
            strJson = strJson.Remove(strJson.LastIndexOf(','), strJson.Length - strJson.LastIndexOf(',')).Replace("{", "").Replace("\",\"", "#").Replace("\"", ""); 
            var tokens = strJson.Split('#');

            var dict = new Dictionary<string, object>();

            foreach (var token in tokens)
            {
                var items = token.Split(':');
                if (items[0].Equals("UserId"))
                {
                    dict.Add(items[0], int.Parse(items[1]));
                } else if(items[0].Equals("Super"))
                {
                    dict.Add(items[0], bool.Parse(items[1]));
                }
                else
                {
                    dict.Add(items[0], items[1]);
                }
            }

            try
            {
                var json = JsonSerializer.Serialize(dict);
                var user = JsonSerializer.Deserialize<User>(json);
                return user;
            } catch (Exception)
            {
                return null;
            }
        }
    }
}
