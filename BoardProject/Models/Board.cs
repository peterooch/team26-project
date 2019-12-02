using BoardProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardProject.Models
{
    public class BoardBase
    {
        public int ID { get; set; }        // Identitfier
        public string BoardName { get; set; }   // Board Name
        public string BoardHeader { get; set; } // Header to be displayed in the board
        public int BackgroundColor { get; set; }
        public int TextColor { get; set; }
        public double FontSize { get; set; }
        public double Spacing { get; set; }

        /* Copy constructor */
        public BoardBase(BoardBase boardBase)
        {
            ID              = boardBase.ID;
            BoardName       = boardBase.BoardName;
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
        public string ButtonIDs { get; set; } //; delimited id numbers (ID1;ID2;ID3;...)
        public BoardData()
        {
        }
    }
    /* Logical Representation of the Board model */
    public class Board : BoardBase
    {
        public List<Button> Buttons; // Button objects associated with the Board object

        public Board(BoardData boardData)
            : base(boardData)
        {
            // Convert IDs to Button objects
            if (boardData.ButtonIDs != null)
            {
                using var DbCon = new DataContext();

                string[] ButtonIDs = boardData.ButtonIDs.Split(';');

                foreach (string Id in ButtonIDs)
                    Buttons.Add(new Button(DbCon.ButtonData.Single(button => button.ID == int.Parse(Id))));
            }
        }
    }
}
