using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BoardProject.Models;
using BoardProject.Data;
using Microsoft.AspNetCore.Http;

namespace BoardProject.Controllers
{
    public class MainPageController : Controller
    {
        // GET MainPage/MainPage

        public IActionResult MainPage()
        {
            int SelectedUser = 1;
            byte[] TestValue;

            using DataContext DBCon = new DataContext();

            if (HttpContext.Session.TryGetValue("SelectedUser", out TestValue))
            {
                SelectedUser = (int)HttpContext.Session.GetInt32("SelectedUser");
            }

            User SelectedUserObject = new User(DBCon.UserData.Find(SelectedUser));
            return View(SelectedUserObject);
        }
    }
}