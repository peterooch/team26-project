using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BoardProject.Data;
using BoardProject.Models;

namespace BoardProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly Localizer localizer;
        private readonly bool UnitTest;

        public HomeController(Localizer localizer, bool UnitTest = false)
        {
            this.localizer = localizer;
            this.UnitTest = UnitTest;
        }
        // GET /
        public IActionResult Index()
        {
            SeedData.PutTestData();
            int SelectedUser;

            /* Open a database connection */
            using DataContext DBCon = new DataContext();

            /* Check if there are any users in the database*/
            if (!DBCon.UserData.Any())
                return RedirectToAction(nameof(FirstRun));

            /* Try to find the ID of currently logged in User */
            if (!UnitTest)
                SelectedUser = HttpContext.Session.GetInt32("SelectedUser") ?? default;
            else
                SelectedUser = 1;

            User SelectedUserObject = null;
            if (SelectedUser == default)
            {
                /* Check if there is a cookie */
                if (!Request.Cookies.TryGetValue("LoggedUser", out string cookieID))
                    return RedirectToAction("Index", "Login");

                /* Preload user object for logging */
                SelectedUser = int.Parse(cookieID);
                SelectedUserObject = new User(DBCon.UserData.Find(SelectedUser));

                /* Bogus ID, redirect back to login */
                if (SelectedUserObject == null)
                    return RedirectToAction("Index", "Login");

                HttpContext.Session.SetInt32("SelectedUser", SelectedUser);

                DBCon.ActivityLogs.Add(new ActivtyLog(ActivtyLog.ActType.Entry,
                                                     SelectedUserObject, $"User {SelectedUserObject.Username} (ID:{SelectedUserObject.ID}) has entered the system"));
                DBCon.SaveChanges();
            }

            /* Fetch the relevant User object from the database if not already loaded */
            if (SelectedUserObject == null)
                SelectedUserObject = new User(DBCon.UserData.Find(SelectedUser));
            /* Set system localization */
            localizer.SetLocale(SelectedUserObject, this);
            /* Pass the User object to View */
            return View(SelectedUserObject);
        }
        // GET /Home/Contact
        public IActionResult Contact()
        {
            if (UnitTest)
                return View();

            /* Get system localization from session variable */
            localizer.SetLocale(HttpContext.Session.GetString("Language"));
            return View();
        }

        public IActionResult FirstRun(string lang = "en")
        {
            using var DBCon = new DataContext();

            if (DBCon.UserData.Any())
                return RedirectToAction(nameof(Index));

            localizer.SetLocale(lang);
            HttpContext.Session.SetString("Language", lang);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FirstRunRegister()
        {
            using var DBCon = new DataContext();

            UserData newUser = new UserData()
            {
                Username = Request.Form["username"],
                IsPrimary = true,
                IsManager = false,
                Language = HttpContext.Session.GetString("Language"),
                Font = "Arial",
                FontSize = 100,
                BackgroundColor = 0xFFFFFF,
                TextColor = 0,
                HighContrast = false,
                DPI = 100,
                /* Give user some boards to toy with */
                BoardIDs = "1;2;3",
                HomeBoardID = "1"
            };
            newUser.StorePassword(Request.Form["password"]);

            DBCon.UserData.Add(newUser);
            DBCon.SaveChanges();

            HttpContext.Session.SetInt32("FirstRun", 1);
            HttpContext.Session.SetInt32("SelectedUser", newUser.ID);

            /* Now go to the preferences page to change some settings */
            return RedirectToAction("Index", "UserPref");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #region DebugCode
        // GET /Home/Debug
        public IActionResult Debug()
        {
            DebugLists debugLists = new DebugLists();

            return View(debugLists);
        }
        
        public string Logs()
        {
            string result = string.Empty;
            using DataContext dataContext = new DataContext();

            foreach (var log in dataContext.ActivityLogs)
                result += $"{log.UserID};{log.TimeStamp};{log.ActivityType};{log.ActivityDescription}\n";
            return result;
        }
        #endregion
    }
}
