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
        // GET /
        public IActionResult Index()
        {
            /* DEBUG: Seed and pick user no. 1 and redirect to MainPage */
            SeedData.PutTestData();
            HttpContext.Session.SetInt32("SelectedUser", 1);
            /* END DEBUG */
            int SelectedUser = 0;
            bool FoundUser = false;

            /* Open a database connection */
            using DataContext DBCon = new DataContext();

            /* Try to find the ID of currently selected User */
            SelectedUser = HttpContext.Session.GetInt32("SelectedUser") ?? default;

            if (SelectedUser != default)
                FoundUser = true;

            if (!FoundUser)
            {
                /* Redirect to login page to  */
                return RedirectToAction("LoginPage", "LoginPage");
            }

            /* Fetch the relevant User object from the database */
            User SelectedUserObject = new User(DBCon.UserData.Find(SelectedUser));
            /* Pass the User object to View */
            return View(SelectedUserObject);
        }
        public string CSS()
        {
            string ColorChanger = "white";
            int BackgroungChange = 0;
            string Font = "ariel";
            double FontSize = 0.0;
            return @$"body {{
                        color: {ColorChanger};
                        background-color: {BackgroungChange};
                        font-family: {Font};
                        font-size: {FontSize}%;
                        }};";
        }
        // GET /Home/Debug
        public IActionResult Debug()
        {
            DebugLists debugLists = new DebugLists();

            return View(debugLists);
        }

        // GET /Home/Contact
        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
