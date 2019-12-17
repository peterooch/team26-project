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
    public class BoardViewController : Controller
    {
        private readonly Localizer localizer;
        public BoardViewController(Localizer localizer)
        {
            this.localizer = localizer;
        }
        // GET /BoardView/id
        public IActionResult Index(int? ID)
        {
            int UserID = HttpContext.Session.GetInt32("SelectedUser") ?? default;
            using var DBCon = new DataContext();
            User UserObject;
            Board SelectedBoard;

            if (UserID == default)
                return View(null);
            else
                UserObject = new User(DBCon.UserData.Find(UserID));

            if (UserObject == null)
                return View(null);

            localizer.SetLocale(UserObject);

            if (ID == null)
                SelectedBoard = UserObject.HomeBoard;
            else
                SelectedBoard = UserObject.Boards.Find(board => board.ID == ID);

            return View(SelectedBoard);
        }
    }
}
