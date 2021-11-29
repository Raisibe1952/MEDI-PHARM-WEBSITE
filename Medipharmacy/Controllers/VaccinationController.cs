using Medipharmacy.Data;
using Medipharmacy.Models;
using Medipharmacy.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using static Medipharmacy.Modules.Module;

namespace Medipharmacy.Controllers
{
    public class VaccinationController : Controller
    {
        private DatabaseContext db;

        public VaccinationController(DatabaseContext db)
        {
            this.db = db;
        }

        [HttpGet("vaccination/book")]
        public IActionResult BookPartial()
        {
            return PartialView(new Booking(0, "", "", "", "", "", DateTime.Now.AddYears(-14).Date, "", "", DateTime.Now.Date, DateTime.Now.AddDays(5).Date, false, ""));
        }

        [HttpPost("vaccination/add")]
        public IActionResult Book()
        {
            var body = HttpContext.Request.Form.ToList()[0].ToString();
            body = body.Replace("[{", "").Replace("}, ]", "");
            var booking = ConvertToBooking(body);
            try
            {
                var b = db.Booking.Where(b => b.SocialSecurityId.Equals(booking.SocialSecurityId)).First();
                var status = new StatusViewModel(Status.WARNING, "A booking with the provided ID already exist!");
                return PartialView("StatusPartial", status);
            }
            catch (Exception)
            {
                db.Booking.Add(booking);
                db.SaveChanges();
                var status = new StatusViewModel(Status.SUCCESS, "A booking was made successfully...");
                return PartialView("StatusPartial", status);
            }
        }

        private Booking ConvertToBooking(string json)
        {
            Booking booking = new Booking();
            var dict = new Dictionary<string, object>();
            var tokens = json.Split(',');
            for(int i = 0; i < tokens.Length - 1; i++)
            {
                var items = tokens[i].Replace("\"", "").Split(':');
                if(items[0] == "BookingId")
                {
                    dict.Add(items[0], int.Parse(items[1]));
                }
                else if(items[0] == "DateCreated" || items[0] == "DateBooked" || items[0] == "DateOfBirth")
                {
                    var strDate = "";
                    for(int j = 1; j < items.Length; j++)
                    {
                        strDate += items[j] + ":";
                    }
                    dict.Add(items[0], DateTime.Parse(strDate.Remove(strDate.Length - 1)));
                }
                else if(items[0] == "Vaccinated")
                {
                    dict.Add(items[0], bool.Parse(items[1]));
                }
                else
                {
                    dict.Add(items[0], items[1]);
                }
            }
            json = JsonSerializer.Serialize(dict);
            booking = JsonSerializer.Deserialize<Booking>(json);
            
            return booking;
        }

        [HttpGet("vaccination/view")]
        public IActionResult ViewPartial()
        {
            return PartialView(new Pair<string, string>());
        }

        [HttpPost("vaccination/show")]
        public IActionResult ShowPartial()
        {
            var body = HttpContext.Request.Form.ToList()[0].ToString();
            body = "{" + body.Replace("[{", "").Replace("}, ]", "") + "}";
            var details = JsonSerializer.Deserialize<Pair<string, string>>(body);
            try
            {
                var booking = db.Booking.Where(b => b.SocialSecurityId.Equals(details.Key) && b.Pin.Equals(details.Value)).First();
                return PartialView(booking);
            }
            catch (Exception)
            {
                return PartialView();
            }
        }

        [HttpGet("vaccination/remove/{bookingId}")]
        public IActionResult DeletePartial(int bookingId)
        {
            var booking = db.Booking.Find(bookingId);
            if (booking != null)
            {
                db.Booking.Remove(booking);
                db.SaveChanges();
                return PartialView();
            }
            else
            {
                return PartialView();
            }
        }
    }
}
