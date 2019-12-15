using System.Collections.Generic;
using System.Linq;
using BoardProject.Models;

namespace BoardProject.Data
{
    public static partial class SeedData
    {
        public static void PutTestData()
        {
            using DataContext DBConnection = new DataContext();

            if (DBConnection.UserData.Any() ||
                DBConnection.BoardData.Any() ||
                DBConnection.TileData.Any() ||
                DBConnection.Image.Any())
            {
                return;
            }
            UserData TestUser = new UserData {
                Username = "TestUser",
                IsPrimary = true,
                IsManager = false,
                Font = "Comic Sans",
                FontSize = 200.0,
                BackgroundColor = 0x800080,
                TextColor = 0xFF0000,
                DPI = 110,
                BoardIDs = "1;2;3;",
                HomeBoardID = "1"
            };
            TestUser.StorePassword("1234");
            DBConnection.UserData.Add(TestUser);
            BoardData TestBoard = new BoardData {
                BoardName = "TestBoard",
                BoardHeader = "Test Header",
                TileIDs = "1;2;3;4;5;6;",
                BackgroundColor = 0xFFFFFF,
                TextColor = 0x000080,
                FontSize = 100.0
            };
            DBConnection.BoardData.Add(TestBoard);
            List<TileData> TestTiles = new List<TileData>
            {
                new TileData { TileName = "TestTile1", TileText = "Test1", BackgroundColor = 0xFF0000, SourceID = 1 },
                new TileData { TileName = "TestTile2", TileText = "Test2", BackgroundColor = 0x0000FF, SourceID = 2 },
                new TileData { TileName = "TestTile3", TileText = "Test3", BackgroundColor = 0x00FF00, SourceID = 3 },
                new TileData { TileName = "TestTile4", TileText = "Test4", BackgroundColor = 0xFFFF00, SourceID = 4 },
                new TileData { TileName = "TestTile5", TileText = "Test5", BackgroundColor = 0xFFFFFF, SourceID = 5 },
                new TileData { TileName = "TestTile6", TileText = "Test6", BackgroundColor = 0x000000, SourceID = 6 }
            };
            DBConnection.TileData.AddRange(TestTiles);
            List<Image> TestImages = new List<Image>
            {
                new Image { Source = "/images/test1.jpg", ImageName = "TestImage1", Category = "No Category", ReferenceCount = 0},
                new Image { Source = "/images/test2.gif", ImageName = "TestImage2", Category = "No Category", ReferenceCount = 0},
                new Image { Source = "/images/test3.jpg", ImageName = "TestImage3", Category = "No Category", ReferenceCount = 0},
                new Image { Source = "/images/test4.jpg", ImageName = "TestImage4", Category = "No Category", ReferenceCount = 0},
                new Image { Source = "/images/test5.jpg", ImageName = "TestImage5", Category = "No Category", ReferenceCount = 0},
                new Image { Source = "/images/test6.jpg", ImageName = "TestImage6", Category = "No Category", ReferenceCount = 0}
            };
            DBConnection.Image.AddRange(TestImages);
            DBConnection.SaveChanges();

            /* Load Image objects from autogenerated list */
            List<Image> NewImages = AutogenList();
            /* Prepare a tile list to accept incoming autogenerated image objects */
            List<TileData> NewTiles = new List<TileData>(NewImages.Count);

            foreach (Image image in NewImages)
            {
                NewTiles.Add(new TileData { ID = image.ID,
                                            SourceID = image.ID,
                                            TileName = image.ImageName,
                                            TileText = image.ImageName,
                                            BackgroundColor = 0xFFFFFF });

                /* Probe for specifics */
                if (image.Source.Contains("pictograms"))
                    image.ImageName = "ARASAAC: " + image.ImageName;
                else if (image.Source.Contains("gif_parrots"))
                    image.ImageName = "Party Parrot GIF: " + image.ImageName;

                NewTiles.Last().TileName = image.ImageName;
            }
            /* Now use our newly generated tile objects in some sample boards */
            BoardData AnimalTest = new BoardData
            {
                BoardName = "Animal Board",
                BoardHeader = "Animal Pictograms",
                TileIDs = "26;28;29;36;42;50;87;161;",
                BackgroundColor = 0xFFFFFF,
                TextColor = 0x000080,
                FontSize = 100.0
            };
            DBConnection.BoardData.Add(AnimalTest);
            DBConnection.SaveChanges();

            int CurrentID = NewTiles.Last().ID;

            /* Interactive test tiles */
            NewTiles.AddRange(new List<TileData>
            {
                new TileData { ID = ++CurrentID,
                               SourceID = 162,
                               TileName = "TEST: Click Nothing",
                               TileText = "Does Nothing",
                               BackgroundColor = 0xFFFFFF,
                               ActionType = TileBase.ActionID.Nothing,
                               ActionContext = "" },
                new TileData { ID = ++CurrentID,
                               SourceID = 98,
                               TileName = "TEST: Play GIF",
                               TileText = "Plays a gif",
                               BackgroundColor = 0xFF0000,
                               ActionType = TileBase.ActionID.PlayGif,
                               ActionContext = "/images/gif_parrots/originalparrot.gif" }, // Maybe change to image id?
                new TileData { ID = ++CurrentID,
                               SourceID = 217,
                               TileName = "TEST: Open External Link",
                               TileText = "Opens an external link",
                               BackgroundColor = 0x00FF00,
                               ActionType = TileBase.ActionID.ExternalLink,
                               ActionContext = "https://www.youtube.com/watch?v=9T1vfsHYiKY" }, // Hmm?
                new TileData { ID = ++CurrentID,
                               SourceID = 28,
                               TileName = "TEST: Switch board",
                               TileText = "Switch to other board",
                               BackgroundColor = 0x0000FF,
                               ActionType = TileBase.ActionID.SwitchBoard,
                               ActionContext = "2" }
            });
            BoardData InteractiveParrot = new BoardData
            {
                BoardName = "Interactive parrot tile test",
                BoardHeader = "Parrots everywhere",
                TileIDs = (CurrentID-3).ToString()+";"+(CurrentID - 2).ToString() + ";"+ (CurrentID - 1).ToString() + ";"+ (CurrentID).ToString() + ";",
                BackgroundColor = 0xFFFFFF,
                TextColor = 0x000080,
                FontSize = 100.0
            };
            DBConnection.BoardData.Add(InteractiveParrot);
            DBConnection.Image.AddRange(NewImages);
            DBConnection.TileData.AddRange(NewTiles);
            DBConnection.SaveChanges();
        }
    }
}