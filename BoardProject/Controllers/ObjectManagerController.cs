using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoardProject.Data;
using BoardProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoardProject.Controllers
{
    public class ObjectManagerController : Controller
    {
        private readonly Localizer localizer;
        public ObjectManagerController(Localizer localizer)
        {
            this.localizer = localizer;
        }
        public IActionResult BoardEditor(int? ID)
        {
            int UserID = HttpContext.Session.GetInt32("SelectedUser") ?? default;

            if (UserID == default)
                return RedirectToAction("Index", "Login");

            localizer.SetLocale(HttpContext.Session.GetString("Language"));

            Board model;
            if (ID != null)
            {
                using var DBCon = new DataContext();
                User user = new User(DBCon.UserData.Find(UserID));

                model = user.Boards.Find(board => board.ID == ID);
            }
            else
            {
                /* Generate empty board to toy with */
                model = new Board
                {
                    BoardName = localizer["New Board"],
                    BoardHeader = localizer["New Board Header"],
                    BackgroundColor = 0xFFFFFF,
                    TextColor = 0x000000,
                    FontSize = 100,
                    IsPublic = false,
                    Tiles = new List<Tile>(),
                    Spacing = 0D
                };
            }
            return View(model);
        }
    }
}