using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BoardProject.Controllers
{
    public class TileController : Controller
    {
        public IActionResult AddTile()
        {
            return View();
        }
    }
}