﻿using BoardProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardProject.Models
{ 
    public class UserBase
    {
        public int ID { get; set; }              // Identifier
        public string Username { get; set; }     // Login Identifier
        public string PasswordHash { get; set; } // SHA512 Password Hash <=> sha512(password+salt)
        public string PasswordSalt { get; set; } // Password salt
        public bool IsPrimary { get; set; }      // Is the user is the primary user? (SuperUser)
        public bool IsManager { get; set; }      // Is the current user is an manager?
        public string Language { get; set; }     // Selected User language
        public string Font { get; set; }         // Selected font name
        public double FontSize { get; set; }     // Selected font size
        public int BackgroundColor { get; set; } // Selected background color
        public int TextColor { get; set; }       // Selected text color
        public bool HighContrast { get; set; }   // Do we use the high contrast theme?
        public int DPI { get; set; }             // Selected DPI

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
            IsPrimary       = userBase.IsPrimary;
            IsManager       = userBase.IsManager;
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
        public string BoardIDs { get; set; }     // ; delimited id numbers (ID1;ID2;ID3;...)
        public string HomeBoardID { get; set; }// Primary board identifier
        public string ManagedUsersIDs { get; set; }
        public UserData()
        {
        }
        public UserData(User user)
            : base(user)
        {
            // Convert Board objects to IDs
            HomeBoardID = user.HomeBoard.ID.ToString();

            BoardIDs = string.Empty;
            ManagedUsersIDs = string.Empty;

            foreach (Board board in user.Boards)
                BoardIDs += board.ID.ToString() + ";";

            if (IsManager)
            {
                foreach (User _user in user.ManagedUsers)
                    ManagedUsersIDs += _user.ID + ";";
            }
        }
    }
    /* Logical Representation of the User model */
    public class User : UserBase
    {
        public List<Board> Boards; // Board objects associated with the User object
        public Board HomeBoard;    // HomeBoard object associated with the User object
        public List<User> ManagedUsers;

        public User(UserData userData)
            : base(userData)
        {
            // Convert IDs to Board objects
            using var DbCon = new DataContext();

            if (!string.IsNullOrEmpty(userData.BoardIDs))
            {
                string[] BoardIDs = userData.BoardIDs.Split(';');

                Boards = new List<Board>();

                foreach (string Id in BoardIDs)
                {
                    if (!string.IsNullOrEmpty(Id))
                        Boards.Add(new Board(DbCon.BoardData.Single(board => board.ID == int.Parse(Id))));
                }
            }

            if (!string.IsNullOrEmpty(userData.HomeBoardID))
                HomeBoard = new Board(DbCon.BoardData.Single(board => board.ID == int.Parse(userData.HomeBoardID)));
            else if (Boards.Count > 0)
                HomeBoard = Boards[0];

            if (IsManager && !string.IsNullOrEmpty(userData.ManagedUsersIDs))
            {
                string[] UserIDs = userData.ManagedUsersIDs.Split(';');

                foreach (string UserID in UserIDs)
                {
                    if (!string.IsNullOrEmpty(UserID))
                        ManagedUsers.Add(new User(DbCon.UserData.Single(user => user.ID == int.Parse(UserID))));
                }
            }
        }
    }
}
