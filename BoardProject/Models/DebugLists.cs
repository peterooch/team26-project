using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoardProject.Data;

namespace BoardProject.Models
{
    public class DebugLists
    {
        public List<UserData> userDatas;
        public List<BoardData> boardDatas;
        public List<TileData> tileDatas;
        public List<Image> images;

        public DebugLists()
        {
            using var DbCon = new DataContext();

            userDatas = DbCon.UserData.ToList();
            boardDatas = DbCon.BoardData.ToList();
            tileDatas = DbCon.TileData.ToList();
            images = DbCon.Image.ToList();
        }
    }
}
