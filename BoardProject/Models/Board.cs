using BoardProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardProject.Models
{
    public class BoardBase
    {
        public int ID;          // Identitfier
        public string BoardName;   // Board Name
        public string BoardHeader; // Header to be displayed in the board

        /* Copy constructor */
        public BoardBase(BoardBase boardBase)
        {
            ID          = boardBase.ID;
            BoardName   = boardBase.BoardName;
            BoardHeader = boardBase.BoardHeader;
        }

        public BoardBase()
        {
        }
    }
    /* Database Representation of the Board model */
    public class BoardData : BoardBase
    {
        public string ButtonIDs; //; delimited id numbers (ID1;ID2;ID3;...)
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
