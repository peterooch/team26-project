using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BoardProject.Models;
using BoardProject.Data;

namespace BoardProject.Controllers
{
    public class DisplayTableController : Controller
    {
        // GET Views/DisplayTable/

        public IActionResult DisplayTable()
        {
            return View();
        }
    }
}