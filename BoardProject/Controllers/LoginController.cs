using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BoardProject.Models;
using BoardProject.Data;
using Microsoft.AspNetCore.Http;

namespace BoardProject.Controllers
{
    public class LoginController : Controller
    {
        private readonly Localizer localizer;
        private readonly bool UnitTest;

        public LoginController(Localizer localizer, bool UnitTest = false)
        {
            this.localizer = localizer;
            this.UnitTest = UnitTest;
        }
        public IActionResult Index()
        {
            if (UnitTest)
                return View(null);

            LoginError model;
            /* Check for failed login attempt*/
            if (HttpContext.Session.TryGetValue("LoginError", out _))
            {
                string[] values = HttpContext.Session.GetString("LoginError").Split(";");
                HttpContext.Session.Remove("LoginError");
                model = new LoginError
                {
                    username = values[0],
                    username_error = bool.Parse(values[1]),
                    password_error = bool.Parse(values[2])
                };
            }
            else
            {
                model = null;
            }
            localizer.SetLocale(HttpContext.Session.GetString("Language"));
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UserLogin()
        {
            using var DBCon = new DataContext();
            UserData userData;
            string username = Request.Form["username"];
            string password = Request.Form["password"];

            try
            {
                userData = DBCon.UserData.Single(user => user.Username == username);
            }
            catch (InvalidOperationException)
            {
                HttpContext.Session.SetString("LoginError", string.Join(";", "", true, false));
                return RedirectToAction("Index", "Login");
            }

            if (!userData.VerifyPassword(password))
            {
                /* Record login attempt */
                DBCon.ActivityLogs.Add(new ActivtyLog(ActivtyLog.ActType.AttemptedLogin,
                                                     userData, $"User {userData.Username} (ID:{userData.ID}) has tried to log in but with incorrect password"));
                DBCon.SaveChanges();

                HttpContext.Session.SetString("LoginError", string.Join(";", username, false, true));
                return RedirectToAction("Index","Login");
            }

            /* Set session variable and cookie for a week */
            HttpContext.Session.SetInt32("SelectedUser", userData.ID);

            Response.Cookies.Append("LoggedUser",
                                     userData.ID.ToString(),
                                     new CookieOptions { Expires =  new DateTimeOffset(DateTime.Now.AddDays(7)) });
            /* Record login success */
            DBCon.ActivityLogs.Add(new ActivtyLog(ActivtyLog.ActType.LoggedIn,
                                                 userData, $"User {userData.Username} (ID:{userData.ID}) has successfully logged in"));
            DBCon.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}