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
        // GET /BoardView/id
        public IActionResult Index(int? ID)
        {
            int UserID = (int)HttpContext.Session.GetInt32("SelectedUser");
            using var DBCon = new DataContext();
            User UserObject = new User(DBCon.UserData.Find(UserID));
            Board SelectedBoard;

            if (ID == null)
                SelectedBoard = UserObject.HomeBoard;
            else
                SelectedBoard = UserObject.Boards.Single(board => board.ID == ID);

            return View(SelectedBoard);
        }
    }
}
