using System.Collections.Generic;
using BoardProject.Models;

namespace BoardProject.Data
{
    public static class SeedData
    {
        public static void PutTestData()
        {
            using DataContext DBConnection = new DataContext();

            if (DBConnection.UserData.Find(1) != null ||
                DBConnection.BoardData.Find(1) != null ||
                DBConnection.TileData.Find(1) != null ||
                DBConnection.Image.Find(1) != null)
            {
                return;
            }
            UserData TestUser = new UserData {
                Username = "TestUser",
                PasswordHash = "1111",
                PasswordSalt = "1111",
                IsPrimary = true,
                IsManager = false,
                Font = "Comic Sans",
                FontSize = 200,
                BackgroundColor = 0x800080,
                TextColor = 0xFF0000,
                DPI = 110,
                BoardIDs = "1;",
                HomeBoardID = "1"
            };
            DBConnection.UserData.Add(TestUser);
            BoardData TestBoard = new BoardData {
                BoardName = "TestBoard",
                BoardHeader = "Test Header",
                TileIDs = "1;2;3;4;5;6;"
            };
            DBConnection.BoardData.Add(TestBoard);
            List<TileData> TestTiles = new List<TileData>
            {
                new TileData { TileName = "Tile1", BackgroundColor = 0xFF0000, SourceID = 1 },
                new TileData { TileName = "Tile2", BackgroundColor = 0x0000FF, SourceID = 2 },
                new TileData { TileName = "Tile3", BackgroundColor = 0x00FF00, SourceID = 3 },
                new TileData { TileName = "Tile4", BackgroundColor = 0xFFFF00, SourceID = 4 },
                new TileData { TileName = "Tile5", BackgroundColor = 0xFFFFFF, SourceID = 5 },
                new TileData { TileName = "Tile6", BackgroundColor = 0x000000, SourceID = 6 }
            };
            DBConnection.TileData.AddRange(TestTiles);
            List<Image> TestImages = new List<Image>
            {
                new Image { Source = "/images/1.jpg", ImageName = "TestImage1", Category = "No Category", ReferenceCount = 0},
                new Image { Source = "/images/2.gif", ImageName = "TestImage2", Category = "No Category", ReferenceCount = 0},
                new Image { Source = "/images/3.jpg", ImageName = "TestImage3", Category = "No Category", ReferenceCount = 0},
                new Image { Source = "/images/4.jpg", ImageName = "TestImage4", Category = "No Category", ReferenceCount = 0},
                new Image { Source = "/images/5.jpg", ImageName = "TestImage5", Category = "No Category", ReferenceCount = 0},
                new Image { Source = "/images/6.jpg", ImageName = "TestImage6", Category = "No Category", ReferenceCount = 0}
            };
            DBConnection.Image.AddRange(TestImages);
            DBConnection.SaveChanges();
        }
    }
}