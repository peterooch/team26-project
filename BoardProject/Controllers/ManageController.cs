
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
    public class ManageController : Controller
    {
        public IActionResult Index()
        {
            int UserID = HttpContext.Session.GetInt32("SelectedUser") ?? default;
            using var DBCon = new DataContext();
            User UserObject;
            if (UserID == default)
                return RedirectToAction("Index", "Login");

            UserObject = new User(DBCon.UserData.Find(UserID));
            if (UserObject == null)
                return RedirectToAction("Index", "Login");
            if (!UserObject.IsPrimary && !UserObject.IsManager)
                return RedirectToAction("Index", "Home");

            return View(UserObject);
        }
        public IActionResult Users() //manage users
        {
            int UserID = HttpContext.Session.GetInt32("SelectedUser") ?? default;
            using var DBCon = new DataContext();
            User UserObject;
            if (UserID == default)
                return RedirectToAction("Index", "Login");

            UserObject = new User(DBCon.UserData.Find(UserID));
            if (UserObject == null)
                return RedirectToAction("Index", "Login");
            if (!UserObject.IsPrimary && !UserObject.IsManager)
                return RedirectToAction("Index", "Home");

            if (UserObject.IsPrimary)
            {
                UserObject.ManagedUsers.Clear();

                foreach (UserData user in DBCon.UserData)
                    UserObject.ManagedUsers.Add(new User(user));
            }
            return View(UserObject);
        }
        public IActionResult Boards() //manage boards
        {
            return null;
        }
        public IActionResult Tiles() //manage tiles
        {
            return null;
        }
        public IActionResult Images() // manage images
        {
            return null;
        }
        public IActionResult Reports(int? ID) //get reports
        {
            return null;
        }
    }
}




   