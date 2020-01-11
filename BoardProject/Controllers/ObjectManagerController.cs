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

            if (tile.ActionType == TileBase.ActionID.PlayGif)
            {
                tile.ActionContext = DBCon.Image.Single(i => i.Source == tile.ActionContext).ID.ToString();
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

            /* Check if we have something to work with */
            if (board != null)
            {
                using var DBCon = new DataContext();
                if (DBCon.BoardData.Any(b => b.ID == board.ID))
                {
                    DBCon.BoardData.Update(board);
                }
                else
                {
                    DBCon.BoardData.Add(board);
                    int UserID = HttpContext.Session.GetInt32("SelectedUser") ?? default;

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
            TileData tile = JsonConvert.DeserializeObject<TileData>(Request.Form["Model"]);

            if (tile == null)
                return "ERROR";

            using var DBCon = new DataContext();
            if (tile.ActionType == TileBase.ActionID.PlayGif)
            {
                tile.ActionContext = DBCon.Image.Single(i => i.ID == int.Parse(tile.ActionContext)).Source;
            }
            if (DBCon.TileData.Any(t => t.ID == tile.ID))
            {
                DBCon.TileData.Update(tile);
            }
            else
            {
                DBCon.TileData.Add(tile);
            }
            DBCon.SaveChanges();
            return "OK";
        }
        [HttpPost]
        public string UpdateImage()
        {
            Image image = JsonConvert.DeserializeObject<Image>(Request.Form["Model"]);

            if (image == null)
                return "ERROR";

            using var DBCon = new DataContext();
            if (DBCon.Image.Any(i => i.ID == image.ID))
            {
                DBCon.Image.Update(image);
            }
            else
            {
                DBCon.Image.Add(image);
            }
            DBCon.SaveChanges();
            return "OK";
        }
        public class FormUploadModel
        {
            [FromForm(Name="image_id")]
            public string ID { get; set; }
            [FromForm(Name="image_file")]
            public IFormFile File { get; set; }
        }
        [HttpPost]
        public string FileUpload([FromForm]FormUploadModel upload)
        {
            var FileName = System.IO.Path.GetExtension(upload.File.FileName);
            string ret = upload.ID + "_image" + FileName;
            using var DestFile = System.IO.File.OpenWrite("wwwroot/images/" + ret);
            using var UploadStream = upload.File.OpenReadStream();

            UploadStream.CopyTo(DestFile);

            return "/images/" + ret;
        }
        public string GetBoardList()
        {
            int UserID = HttpContext.Session.GetInt32("SelectedUser") ?? default;

            if (UserID == default)
                return "null";

            using var DBCon = new DataContext();

            User user = new User(DBCon.UserData.Find(UserID));
            Dictionary<string, string> boardList = new Dictionary<string, string>();

            foreach (Board board in user.Boards)
                boardList.Add(board.ID.ToString(), board.BoardName);

            return JsonConvert.SerializeObject(boardList);
        }
        public string GetTileList()
        {
            using var DBCon = new DataContext();
            Dictionary<string,string> tileList = new Dictionary<string, string>();

            foreach (TileData tile in DBCon.TileData)
                tileList.Add(tile.ID.ToString(),tile.TileName);

            return JsonConvert.SerializeObject(tileList);
        }
#nullable enable
        public string GetImageList(string? category)
        {
            using var DBCon = new DataContext();
            Dictionary<string,string> imageList = new Dictionary<string, string>();

            foreach (Image image in DBCon.Image)
            {
                if (string.IsNullOrEmpty(category) || (image.Category == category))
                    imageList.Add(image.ID.ToString(), image.ImageName);
            }

            return JsonConvert.SerializeObject(imageList);
        }
#nullable disable
        public string GetImageCategories()
        {
            using var DBCon = new DataContext();
            List<string> categories = new List<string>();

            foreach (Image image in DBCon.Image)
            {
                if (!categories.Contains(image.Category))
                    categories.Add(image.Category);
            }
            return JsonConvert.SerializeObject(categories);
        }
    }
}
