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
            
            return View(UserObject);
        }
    }
}
