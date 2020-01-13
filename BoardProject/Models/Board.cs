using BoardProject.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BoardProject.Models
{
    public class BoardBase
    {
        public int ID { get; set; }       // Identitfier
        [Display(Name = "Board Name")]
        public string BoardName { get; set; }    // Board Name
        public bool IsPublic { get; set; }    // Is the board public to all users?
        [Display(Name = "Board Header")]
        public string BoardHeader { get; set; }  // Header to be displayed in the board
        [Display(Name = "Background Color")]
        public int BackgroundColor { get; set; }
        [Display(Name = "Text Color")]
        public int TextColor { get; set; }
        [Display(Name = "Font size")]
        public double FontSize { get; set; }
        public double Spacing { get; set; }

        /* Copy constructor */
        public BoardBase(BoardBase boardBase)
        {
            ID              = boardBase.ID;
            BoardName       = boardBase.BoardName;
            IsPublic        = boardBase.IsPublic;
            BoardHeader     = boardBase.BoardHeader;
            BackgroundColor = boardBase.BackgroundColor;
            TextColor       = boardBase.TextColor;
            FontSize        = boardBase.FontSize;
            Spacing         = boardBase.Spacing;
        }

        public BoardBase()
        {
        }
    }
    /* Database Representation of the Board model */
    public class BoardData : BoardBase
    {
        [Display(Name = "Included Tile IDs")]
        public string TileIDs { get; set; } //; delimited id numbers (ID1;ID2;ID3;...)
        public BoardData()
        {
        }
    }
    /* Logical Representation of the Board model */
    public class Board : BoardBase
    {
        public List<Tile> Tiles; // Tile objects associated with the Board object

        public Board()
        {
        }
        public Board(BoardData boardData)
            : base(boardData)
        {
            Tiles = new List<Tile>();
            // Convert IDs to Tile objects
            if (!string.IsNullOrEmpty(boardData.TileIDs))
            {
                using var DbCon = new DataContext();

                string[] TileIDs = boardData.TileIDs.Split(';');

                foreach (string Id in TileIDs)
                {
                    if (!string.IsNullOrEmpty(Id))
                        Tiles.Add(new Tile(DbCon.TileData.Single(tile => tile.ID == int.Parse(Id))));
                }
            }
        }
    }
}
