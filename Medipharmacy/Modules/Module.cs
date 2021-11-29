using Medipharmacy.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Medipharmacy.Modules
{
    public class Module
    {
        public static int CheckUser(HttpContext context, DatabaseContext db)
        {
            var strUserId = context.Session.GetInt32("UserId");
            if (strUserId.HasValue)
            {
                var userId = strUserId.Value;

                if (userId != 0)
                {
                    var user = db.User.Find(userId);
                    if (user != null)
                    {
                        if (user.Super)
                        {
                            return userId;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        public static int CheckCustomer(HttpContext context)
        {
            var strUserId = context.Session.GetInt32("UserId");
            if (strUserId.HasValue)
            {
                return strUserId.Value;
            }
            return 0;
        }

        public static bool IsLoggedIn(HttpRequest request)
        {
            var userId = request.HttpContext.Session.GetInt32("UserId");
            if(userId != null)
            {
                if(userId.Value > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public static string HashPassword(string password)
        {
            SHA1 algorithm = SHA1.Create();
            byte[] byteArray = null;
            byteArray = algorithm.ComputeHash(Encoding.Default.GetBytes(password));
            string hashedPassword = "";
            for (int i = 0; i < byteArray.Length - 1; i++)
            {
                hashedPassword += byteArray[i].ToString("x2");
            }
            return hashedPassword;
        }

        public enum Status
        {
            SUCCESS,
            FAILURE,
            WARNING
        }
    }
}
