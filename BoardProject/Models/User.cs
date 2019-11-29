using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardProject.Models
{ 
    public class UserBase
    {
        public int ID;              // Identifier
        public string Username;     // Login Identifier
        public string PasswordHash; // SHA512 Password Hash <=> sha512(password+salt)
        public string PasswordSalt; // Password salt
        public bool IsAdmin;        // Is the current user is an administrator?
        public string Language;     // Selected User language
        public string Font;         // Selected font name
        public int FontSize;        // Selected font size
        public int BackgroundColor; // Selected background color
        public int TextColor;       // Selected text color
        public bool HighContrast;   // Do we use the high contrast theme?
        public int DPI;             // Selected DPI

        public UserBase()
        {
        }

        /* Copy constructor */
        public UserBase(UserBase userBase)
        {
            ID              = userBase.ID;
            Username        = userBase.Username;     
            PasswordHash    = userBase.PasswordHash;
            PasswordSalt    = userBase.PasswordSalt;
            IsAdmin         = userBase.IsAdmin;
            Language        = userBase.Language;
            Font            = userBase.Font;        
            FontSize        = userBase.FontSize;
            BackgroundColor = userBase.BackgroundColor;
            TextColor       = userBase.TextColor;
            HighContrast    = userBase.HighContrast;   
            DPI             = userBase.DPI;         
        }
    }
    /* Database Representation of the User model */
    public class UserData : UserBase
    {
        public string BoardIDs;     // ; delimited id numbers (ID1;ID2;ID3;...)
        public string HomeBoardID;  // Primary board identifier
    }
    /* Logical Representation of the User model */
    public class User : UserBase
    {
        public List<Board> Boards; // Board objects associated with the User object
        public Board HomeBoard;    // HomeBoard object associated with the User object

        public User(UserData userData)
            : base(userData)
        {
            // Convert IDs to Board objects
        }
    }
}
