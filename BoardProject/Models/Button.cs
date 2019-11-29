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

        public int ID;               // Identifier
        public string ButtonName;    // Button Name
        public ActionID ActionType;  // Action Type specifier
        public string ActionContext; // Action context (resource location)

        public ButtonBase()
        {
        }

        /* Copy constructor */
        public ButtonBase(ButtonBase buttonBase)
        {
            ID            = buttonBase.ID;
            ButtonName    = buttonBase.ButtonName;
            ActionType    = buttonBase.ActionType;
            ActionContext = buttonBase.ActionContext;
        }
    }
    /* Database Representation of the Button model */
    public class ButtonData : ButtonBase
    {
        public int SourceID; // Database button identifier
    }
    /* Logical Representation of the User model */
    public class Button : ButtonBase
    {
        public Image Source; // Image object associated with the Button object

        public Button(ButtonData buttonData)
            : base(buttonData)
        {
            // Convert source string to Image object
        }
    }
}
