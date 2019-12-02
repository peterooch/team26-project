using BoardProject.Data;
using System;
using System.Collections.Generic;
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
        public string TileName { get; set; }    // Tile Name
        public string TileText { get; set; }    // Tile Description
        public ActionID ActionType { get; set; }  // Action Type specifier
        public string ActionContext { get; set; } // Action context (resource location)
        public int BackgroundColor { get; set; }
        public TileBase()
        {
        }

        /* Copy constructor */
        public TileBase(TileBase tileBase)
        {
            ID              = tileBase.ID;
            TileName      = tileBase.TileName;
            TileText      = tileBase.TileText;
            ActionType      = tileBase.ActionType;
            ActionContext   = tileBase.ActionContext;
            BackgroundColor = tileBase.BackgroundColor;
        }
    }
    /* Database Representation of the Tile model */
    public class TileData : TileBase
    {
        public int SourceID { get; set; } // Database tile identifier

        public TileData()
        {
        }
    }
    /* Logical Representation of the User model */
    public class Tile : TileBase
    {
        public Image Source; // Image object associated with the Tile object

        public Tile(TileData tileData)
            : base(tileData)
        {
            // Convert source ID to Image object
            using var DbCon = new DataContext();

            Source = DbCon.Image.Single(image => image.ID == tileData.SourceID);
        }
    }
}
