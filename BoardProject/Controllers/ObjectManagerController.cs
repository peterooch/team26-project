using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoardProject.Data;
using BoardProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
            using var DBCon = new DataContext();
            if (ID != null)
            {
                User user = new User(DBCon.UserData.Find(UserID));

                model = user.Boards.Find(board => board.ID == ID);
            }
            else
            {
                /* Generate empty board to toy with */
                model = new Board
                {
                    ID = DBCon.BoardData.Max(b => b.ID) + 1,
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
        public string TileJSON(int? ID)
        {
            using var DBCon = new DataContext();
            Tile tile;
            if (ID == null)
            {
                /* Create blank tile */
                tile = new Tile
                {
                    ID = DBCon.TileData.Max(t => t.ID) + 1,
                    TileName = localizer["New Tile Name"],
                    TileText = localizer["New Tile Description"],
                    ActionType = TileBase.ActionID.Nothing,
                    ActionContext = string.Empty,
                    BackgroundColor = 0xFFFFFF,
                    Source = null
                };
            }
            else
            {
                tile = new Tile(DBCon.TileData.Find(ID));
            }

            return JsonConvert.SerializeObject(tile);
        }
        public string ImageJSON(int? ID)
        {
            Image image;
            using var DBCon = new DataContext();
            if (ID == null)
            {
                /* Create blank image */
                image = new Image
                {
                    ID = DBCon.Image.Max(i => i.ID) + 1,
                    Source = string.Empty,
                    Category = localizer["No category"],
                    ImageName = localizer["New Image"],
                    ReferenceCount = 0
                };
            }
            else
            {
                image = DBCon.Image.Find(ID);
            }
            return JsonConvert.SerializeObject(image);
        }
    }
}