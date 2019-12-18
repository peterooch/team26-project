using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BoardProject.Data;
using BoardProject.Models;
using Microsoft.AspNetCore.Http;

namespace BoardProject.Controllers
{
    public class UserPrefController : Controller
    {
        // GET: /<controller>/
        private readonly Localizer localizer;
        
        public UserPrefController(Localizer localizer)
        {
            this.localizer = localizer;
        }
        public IActionResult Index()
        {
            int UserID = HttpContext.Session.GetInt32("SelectedUser") ?? default;
            using var DBCon = new DataContext();
            User UserObject;
            if (UserID == default)
                return View(null);

            UserObject = new User(DBCon.UserData.Find(UserID));

            if (UserObject == null)
                return View(null);

            localizer.SetLocale(UserObject);

            return View(UserObject);
        }

        [Route("/UserPref")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PostIndex()
        {
            int UserID = HttpContext.Session.GetInt32("SelectedUser") ?? default;
            using var DBCon = new DataContext();
            UserData userData;

            if (UserID != default)
            {
                userData = DBCon.UserData.Find(UserID);

                if (userData != null)
                {
                    userData.Language = Request.Form["lang_pick"];
                    userData.Font = Request.Form["font_pick"];
                    userData.BackgroundColor = int.Parse(Request.Form["bg_color"],System.Globalization.NumberStyles.HexNumber);
                    userData.TextColor = int.Parse(Request.Form["tx_color"], System.Globalization.NumberStyles.HexNumber);
                    userData.DPI = int.Parse(Request.Form["dpi"]);
                    userData.FontSize = double.Parse(Request.Form["font_size"]);

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
                        user.FontSize = double.Parse(Request.Form["FontSize"]);

                    DBCon.SaveChanges();
                }
            }
            return "OK";
        }
    }
}
