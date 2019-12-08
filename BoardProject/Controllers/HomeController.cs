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
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // GET /
        public IActionResult Index()
        {
            /* DEBUG: Seed and pick user no. 1 */
            SeedData.PutTestData();
            HttpContext.Session.SetInt32("SelectedUser", 1);
            return View();
        }
        // GET /Home/Debug
        public IActionResult Debug()
        {
            DebugLists debugLists = new DebugLists();

            return View(debugLists);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
