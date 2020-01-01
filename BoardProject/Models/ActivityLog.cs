using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardProject.Models
{
    public class ActivtyLog
    {
        public enum ActType
        {
            DisplayBoard,
            Entry,
            LoggedIn,
            AttemptedLogin,
            UserPrefChange,
            TileClick
        }
        public ulong TimeStamp { get; set; } // Decimal time stamp
        public int UserID { get; set; } // User Identifier
        public ActType ActivityType { get; set; } // Activity type
        public string ActivityDescription { get; set; } // A bit more verbose description of the activity

        /* Empty constructor for EF */
        public ActivtyLog()
        {
        }
        /* Constructor to be used for creating logs */
        public ActivtyLog(ActType ActivityType, UserBase user, string ActivityDescription)
        {
            TimeStamp = ulong.Parse(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
            UserID = user.ID;
            this.ActivityType = ActivityType;
            this.ActivityDescription = ActivityDescription;
        }
    }
}
