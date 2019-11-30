using BoardProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardProject.Models
{
    public class ButtonBase
    {
        public enum ActionID
        {
            Nothing,
            PlayGif,
            ExternalLink,
            SwitchBoard
        }

        public int ID { get; set; }               // Identifier
        public string ButtonName { get; set; }    // Button Name
        public string ButtonText { get; set; }    // Button Description
        public ActionID ActionType { get; set; }  // Action Type specifier
        public string ActionContext { get; set; } // Action context (resource location)

        public ButtonBase()
        {
        }

        /* Copy constructor */
        public ButtonBase(ButtonBase buttonBase)
        {
            ID            = buttonBase.ID;
            ButtonName    = buttonBase.ButtonName;
            ButtonText    = buttonBase.ButtonText;
            ActionType    = buttonBase.ActionType;
            ActionContext = buttonBase.ActionContext;
        }
    }
    /* Database Representation of the Button model */
    public class ButtonData : ButtonBase
    {
        public int SourceID { get; set; } // Database button identifier
    }
    /* Logical Representation of the User model */
    public class Button : ButtonBase
    {
        public Image Source; // Image object associated with the Button object

        public Button(ButtonData buttonData)
            : base(buttonData)
        {
            // Convert source ID to Image object
            using var DbCon = new DataContext();

            Source = DbCon.Image.Single(image => image.ID == buttonData.SourceID);
        }
    }
}
