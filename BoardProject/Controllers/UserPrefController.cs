using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BoardProject.Data;
using BoardProject.Models;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace BoardProject.Controllers
{
    public class UserPrefController : Controller
    {
        private readonly Localizer localizer;
        private readonly StringBuilder LogMessage;
        private readonly Func<string[], StringBuilder> LogCheckAndAppend;

        public UserPrefController(Localizer localizer)
        {
            this.localizer = localizer;
            LogMessage = new StringBuilder();
            /* Setup the lambda function */
            LogCheckAndAppend = input => LogMessage.Append((input[1] != input[2]) ? $"Changed {input[0]} to \"{input[1]}\", " : "");
        }
        // GET: /UserPref/
        public IActionResult Index()
        {
            int UserID = HttpContext.Session.GetInt32("SelectedUser") ?? default;
            using var DBCon = new DataContext();
            User UserObject;
            if (UserID == default)
                return View(null);

            UserObject = new User(DBCon.UserData.Find(UserID));

            /* This should not happen normally */
            if (UserObject == null)
                return RedirectToAction("Index", "Login");

            localizer.SetLocale(UserObject);

            return View(UserObject);
        }

        // POST: /UserPref/
        [Route("/UserPref")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PostIndex()
        {
            int UserID = HttpContext.Session.GetInt32("SelectedUser") ?? default;
            using var DBCon = new DataContext();

            if (UserID != default)
            {
                UserData userData = DBCon.UserData.Find(UserID);

                if (userData != null)
                {
                    /* Check for differences and add them to logging message */
                    LogCheckAndAppend(new[] { "Language", Request.Form["lang_pick"].ToString(), userData.Language });
                    LogCheckAndAppend(new[] { "Font", Request.Form["font_pick"].ToString(), userData.Font });
                    LogCheckAndAppend(new[] { "Background Color", Request.Form["bg_color"].ToString(), userData.BackgroundColor.ToString("X6") });
                    LogCheckAndAppend(new[] { "Text Color", Request.Form["tx_color"].ToString(), userData.TextColor.ToString("X6") });
                    LogCheckAndAppend(new[] { "DPI", Request.Form["dpi"].ToString(), userData.DPI.ToString() });
                    LogCheckAndAppend(new[] { "Font size", Request.Form["font_size"].ToString(), userData.FontSize.ToString() });

                    userData.Language = Request.Form["lang_pick"];
                    userData.Font = Request.Form["font_pick"];
                    userData.BackgroundColor = int.Parse(Request.Form["bg_color"],System.Globalization.NumberStyles.HexNumber);
                    userData.TextColor = int.Parse(Request.Form["tx_color"], System.Globalization.NumberStyles.HexNumber);
                    userData.DPI = int.Parse(Request.Form["dpi"]);
                    userData.FontSize = double.Parse(Request.Form["font_size"]);

                    if (LogMessage.Length != 0)
                    {
                        DBCon.ActivityLogs.Add(new ActivtyLog(ActivtyLog.ActType.UserPrefChange,
                                                 userData, $"User {userData.Username} (ID:{userData.ID}) has " + LogMessage.ToString()));
                    }
                    DBCon.SaveChanges();
                }
            }
            return RedirectToAction("Index", "Home");
        }
        /* Directly update user preferences without need to use the form */
        [HttpPost]
        public string AjaxUpdate()
        {
            int UserID = HttpContext.Session.GetInt32("SelectedUser") ?? default;

            if (UserID != default)
            {
                using var DBCon = new DataContext();
                UserData user = DBCon.UserData.Find(UserID);

                if (user != null)
                {
                    if (!string.IsNullOrEmpty(Request.Form["FontSize"]))
                    {
                        LogCheckAndAppend(new[] { "Font size", Request.Form["font_size"].ToString(), user.FontSize.ToString() });
                        user.FontSize = double.Parse(Request.Form["FontSize"]);
                    }

                    if (LogMessage.Length != 0)
                    {
                        DBCon.ActivityLogs.Add(new ActivtyLog(ActivtyLog.ActType.UserPrefChange,
                                                 user, $"User {user.Username} (ID:{user.ID}) has " + LogMessage.ToString()));
                    }

                    DBCon.SaveChanges();
                }
            }
            return "OK";
        }
    }
}
