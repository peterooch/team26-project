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
        /* Return a JSON string of a requested tile object to simplify manipulation in JS */
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
        /* Return a JSON string of a requested image object to simplify manipulation in JS */
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
        /* Receives a JSON string of a BoardData object,
         * convert back into a BoardData object,
         * and save said object
         * */
        [HttpPost]
        public string UpdateBoard()
        {
            BoardData board = JsonConvert.DeserializeObject<BoardData>(Request.Form["Model"]);
            int UserID = HttpContext.Session.GetInt32("SelectedUser") ?? default;

            /* Check if we have something to work with */
            if (board != null)
            {
                using var DBCon = new DataContext();
                /* This is VERY^3 wrong, concurrent use will cause overwriting of boards *shrug* */
                if (board.ID <= DBCon.BoardData.Max(b => b.ID))
                {
                    /* Current board is *probably* a updated one */
                    DBCon.BoardData.Update(board);
                }
                else
                {
                    /* Current board is *probably* a new one */
                    DBCon.BoardData.Add(board);

                    UserData user = DBCon.UserData.Find(UserID);

                    /* Associate new board with the user */
                    if (user != null)
                        user.BoardIDs += board.ID.ToString() + ";";
                }
                DBCon.SaveChanges();
            }
            return "OK";
        }
        /* Removes a board that has the same value as the ID POST parameter*/
        [HttpPost]
        public string RemoveBoard()
        {
            int UserID = HttpContext.Session.GetInt32("SelectedUser") ?? default;

            if (UserID != default)
            {
                using var DBCon = new DataContext();
                UserData user = DBCon.UserData.Find(UserID);

                if (user != null)
                {
                    /* Remove association from board list */
                    user.BoardIDs = user.BoardIDs.Replace(Request.Form["ID"], "");

                    /* Check if its the Homeboard, if so remove it and assign first board in BoardIDs */
                    if (user.HomeBoardID == Request.Form["ID"])
                        user.HomeBoardID = user.BoardIDs.Split(";")?[0] ?? string.Empty;
                    DBCon.SaveChanges();
                    return "OK";
                }
            }
            return "ERROR";
        }
        [HttpPost]
        public string UpdateTile()
        {
            Tile tile = JsonConvert.DeserializeObject<Tile>(Request.Form["Model"]);

            if (tile == null)
                return "ERROR";

            TileData data = new TileData(tile);

            using var DBCon = new DataContext();
            if (tile.ID <= DBCon.TileData.Max(t => t.ID))
            {
                DBCon.TileData.Update(data);
            }
            else
            {
                DBCon.TileData.Add(data);
            }
            return "OK";
        }
        [HttpPost]
        public string UpdateImage()
        {
            Image image = JsonConvert.DeserializeObject<Image>(Request.Form["Model"]);

            if (image == null)
                return "ERROR";

            using var DBCon = new DataContext();
            if (image.ID <= DBCon.Image.Max(i => i.ID))
            {
                DBCon.Image.Update(image);
            }
            else
            {
                DBCon.Image.Add(image);
            }
            return "OK";
        }
        public class FormUploadModel
        {
            [FromForm(Name="id")]
            public string ID { get; set; }
            [FromForm(Name="file")]
            public IFormFile File { get; set; }
        }
        [HttpPost]
        public string FileUpload([FromForm]FormUploadModel upload)
        {
            var FileName = System.IO.Path.GetExtension(upload.File.FileName);

            using var DestFile = System.IO.File.OpenWrite("wwwroot/images/" + upload.ID + "_" + FileName);
            using var UploadStream = upload.File.OpenReadStream();

            UploadStream.CopyTo(DestFile);

            return "/images/" + upload.ID + "_" + FileName;
        }
    }
}
