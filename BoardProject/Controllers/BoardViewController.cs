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
        private readonly bool UnitTest;

        public BoardViewController(Localizer localizer, bool UnitTest = false)
        {
            this.localizer = localizer;
            this.UnitTest = UnitTest;
        }
        // GET /BoardView/ID
        public IActionResult Index(int? ID)
        {
            int UserID =  UnitTest ? 1 : HttpContext.Session.GetInt32("SelectedUser") ?? default;
            using var DBCon = new DataContext();
            User UserObject;
            Board SelectedBoard;

            if (UserID == default)
                return View(null);

            UserObject = new User(DBCon.UserData.Find(UserID));

            if (UserObject == null)
                return View(null);

            localizer.SetLocale(UserObject);

            if (ID == null)
                SelectedBoard = UserObject.HomeBoard;
            else
                SelectedBoard = UserObject.Boards.Find(board => board.ID == ID);

            if (SelectedBoard != null)
            {
                /* Record board access*/
                DBCon.ActivityLogs.Add(new ActivtyLog(ActivtyLog.ActType.DisplayBoard, UserObject,
                    $"User {UserObject.Username} (ID:{UserObject.ID}) has selected board \"{SelectedBoard.BoardName}\"(ID:{SelectedBoard.ID})"));
                DBCon.SaveChanges();
            }
            return View(SelectedBoard);
        }
        /* Receive AJAX requests from TileOnClick for logging purposes */
        [HttpPost]
        public string LogTileClick()
        {
            int UserID = HttpContext.Session.GetInt32("SelectedUser") ?? default;
            using var DBCon = new DataContext();

            if (!string.IsNullOrEmpty(Request.Form["TileID"]) && UserID != default)
            {
                UserData userData = DBCon.UserData.Find(UserID);

                if (userData != null)
                {
                    /* Fetch relevant tile data */
                    TileData ClickedTile = DBCon.TileData.Find(int.Parse(Request.Form["TileID"]));
                    /* Record tile click */
                    DBCon.ActivityLogs.Add(new ActivtyLog(ActivtyLog.ActType.TileClick, userData,
                        $"User {userData.Username} (ID:{userData.ID}) has clicked tile \"{ClickedTile.TileName}\" (ID:{ClickedTile.ID})"));
                    DBCon.SaveChanges();
                }
            }
            return "OK";
        }
    }
}
