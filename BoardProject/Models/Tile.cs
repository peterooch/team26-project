using BoardProject.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BoardProject.Models
{
    public class TileBase
    {
        public enum ActionID
        {
            Nothing,
            PlayGif,
            ExternalLink,
            SwitchBoard
        }

        public int ID { get; set; }               // Identifier
        [Display(Name = "Tile Name")]
        public string TileName { get; set; }      // Tile Name
        public bool IsPublic { get; set; }        // Is the tile public to all users?
        [Display(Name = "Tile Description")]
        public string TileText { get; set; }      // Tile Description
        [Display(Name = "Action Type")]
        public ActionID ActionType { get; set; }  // Action Type specifier
        [Display(Name = "Action Context")]
        public string ActionContext { get; set; } // Action context (resource location)
        [Display(Name = "Background Color")]
        public int BackgroundColor { get; set; }  // Tile background color
        public TileBase()
        {
        }

        /* Copy constructor */
        public TileBase(TileBase tileBase)
        {
            ID              = tileBase.ID;
            TileName        = tileBase.TileName;
            IsPublic        = tileBase.IsPublic;
            TileText        = tileBase.TileText;
            ActionType      = tileBase.ActionType;
            ActionContext   = tileBase.ActionContext;
            BackgroundColor = tileBase.BackgroundColor;
        }
    }
    /* Database Representation of the Tile model */
    public class TileData : TileBase
    {
        [Display(Name = "Included Image ID")]
        public int SourceID { get; set; } // Database tile identifier

        public TileData()
        {
        }
        public TileData(Tile tile) : base(tile)
        {
            SourceID = tile.Source?.ID ?? 0;
        }
    }
    /* Logical Representation of the User model */
    public class Tile : TileBase
    {
        public Image Source; // Image object associated with the Tile object

        public Tile()
        {
        }
        public Tile(TileData tileData)
            : base(tileData)
        {
            // Convert source ID to Image object
            using var DbCon = new DataContext();

            Source = DbCon.Image.Single(image => image.ID == tileData.SourceID);
        }
    }
}
