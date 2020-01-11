using System;
using Xunit;
using BoardProject.Models;
using BoardProject.Data;
using BoardProject.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Tests
{
    public class TestControllerMethods
    {
        private readonly Localizer test_localizer = new Localizer();
        [Fact]
        public void TestHomeController_Index()
        {
            HomeController controller = new HomeController(test_localizer, true);
            ViewResult result = controller.Index() as ViewResult;
            Assert.NotNull(result);
        }
        [Fact]
        public void TestHomeController_Debug()
        {
            HomeController controller = new HomeController(test_localizer, true);
            ViewResult result = controller.Debug() as ViewResult;
            Assert.NotNull(result);
        }
        [Fact]
        public void TestHomeController_Contact()
        {
            HomeController controller = new HomeController(test_localizer, true);
            ViewResult result = controller.Contact() as ViewResult;
            Assert.NotNull(result);
        }
        [Fact]
        public void TestBoardViewController_Index()
        {
            BoardViewController controller = new BoardViewController(test_localizer, true);
            ViewResult result = controller.Index(null) as ViewResult;
            Assert.NotNull(result);
        }
        [Fact]
        public void TestLoginPageController_LoginPage()
        {
            LoginController controller = new LoginController(test_localizer, true);
            ViewResult result = controller.Index() as ViewResult;
            Assert.NotNull(result);
        }
    }

    public class TestModels
    {
        [Theory]
        [InlineData("1234")]
        [InlineData("$Hello world$")]
        [InlineData("ah!4e$2!d1-2 13t31")]
        public void User_VerifyPassword(string str)
        {
            UserData UnitTestUser = new UserData();

            /* Use supplied password to test hashing algorithm */
            UnitTestUser.StorePassword(str);

            Assert.True(UnitTestUser.VerifyPassword(str));
        }
        [Fact]
        public void User_DifferentSalts()
        {
            UserData UnitTestUser = new UserData();

            UnitTestUser.StorePassword("1234");
            string salt1 = UnitTestUser.PasswordSalt;
            UnitTestUser.StorePassword("1234");
            string salt2 = UnitTestUser.PasswordSalt;

            Assert.NotEqual(salt1, salt2);
        }
        [Fact]
        public void User_DifferentHashes()
        {
            UserData UnitTestUser = new UserData();

            UnitTestUser.StorePassword("1234");
            string hash1 = UnitTestUser.PasswordHash;
            UnitTestUser.StorePassword("1234");
            string hash2 = UnitTestUser.PasswordHash;

            Assert.NotEqual(hash1, hash2);
        }
    }

    public class TestLocalizer
    {
        [Fact]
        public void TestSetLocaleWithIncorrectID()
        {
            Localizer localizer = new Localizer();

            /* blah should not be any kind of language identitfier */
            Assert.Throws<Exception>(() => localizer.SetLocale("blah"));
        }
        [Theory]
        [InlineData("dir", "ltr")] // Should exist
        [InlineData("lang-en", "English")] // Should exist
        [InlineData("dummy_key", "dummy_key")] // Should not exist
        public void TestGetString(string key, string expected)
        {
            Localizer localizer = new Localizer("en");

            Assert.Equal(expected, localizer[key]);
        }
    }

    public class TestObjectMangerMethods
    {
        private readonly Image dummyImage;
        private readonly UserData dummyUser;
        public TestObjectMangerMethods()
        {
            dummyUser = new UserData
            {
                Username = "Test",
                BoardIDs = "",
                IsManager = false,
                IsPrimary = false,
                Language = "en",
            };
            dummyUser.StorePassword("1111");
            dummyImage = new Image
            {
                Source = "nothing.png",
                ImageName = "Test Image",
                ReferenceCount = 0,
                Category = "None",
            };
            using var DBCon = new DataContext();
            DBCon.Image.Add(dummyImage);
            DBCon.UserData.Add(dummyUser);
            DBCon.SaveChanges();
        }
        [Fact]
        public void TestTileJSON()
        {
            var httpContext = new DefaultHttpContext();
            var controllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            var controller = new ObjectManagerController(new Localizer())
            {
                ControllerContext = controllerContext
            };
            TileData tileData = new TileData()
            {
                ActionContext = "",
                ActionType = TileBase.ActionID.Nothing,
                TileName = "Test Tile",
                TileText = "Test Tile",
                BackgroundColor = 0,
                SourceID = dummyImage.ID
            };
            using var DBCon = new DataContext();
            DBCon.TileData.Add(tileData);
            DBCon.SaveChanges();
            Tile tile = new Tile(tileData);

            /* Check if the same object is being returned from controller */
            Assert.Equal(JsonConvert.SerializeObject(tile), controller.TileJSON(tile.ID));
        }
        [Fact]
        public void TestImageJSON()
        {
            var httpContext = new DefaultHttpContext();
            var controllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            var controller = new ObjectManagerController(new Localizer())
            {
                ControllerContext = controllerContext
            };
            Assert.Equal(JsonConvert.SerializeObject(dummyImage), controller.ImageJSON(dummyImage.ID));
        }
        [Fact]
        public void TestGetTileList()
        {
            var httpContext = new DefaultHttpContext();
            var controllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            var controller = new ObjectManagerController(new Localizer())
            {
                ControllerContext = controllerContext
            };
            TileData tileData = new TileData()
            {
                ActionContext = "",
                ActionType = TileBase.ActionID.Nothing,
                TileName = "Test Tile",
                TileText = "Test Tile",
                BackgroundColor = 0,
                SourceID = dummyImage.ID
            };
            using var DBCon = new DataContext();
            DBCon.TileData.Add(tileData);
            DBCon.SaveChanges();

            Assert.True(JsonConvert.DeserializeObject<Dictionary<string, string>>(controller.GetTileList()).ContainsKey(tileData.ID.ToString()));
        }
        [Fact]
        public void TestGetImageList()
        {
            var httpContext = new DefaultHttpContext();
            var controllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            var controller = new ObjectManagerController(new Localizer())
            {
                ControllerContext = controllerContext
            };
            Assert.True(JsonConvert.DeserializeObject<Dictionary<string, string>>(controller.GetImageList(null)).ContainsKey(dummyImage.ID.ToString()));
        }
        [Fact]
        public void TestUpdateBoard()
        {
            /* Setup test data */
            BoardData boardData = new BoardData
            {
                BoardName = "New Board",
                BoardHeader = "New Board Header",
                BackgroundColor = 0xFFFFFF,
                TextColor = 0x000000,
                FontSize = 100,
                IsPublic = false,
                TileIDs = "",
                Spacing = 0D
            };
            using (var DBCon = new DataContext())
            {
                DBCon.BoardData.Add(boardData);
                DBCon.SaveChanges();
            }
            /* End of test data setup */

            boardData.BoardName = "Updated Board Name";
            /* Set up a mocked POST request */
            var httpContext = new DefaultHttpContext();
            httpContext.Request.ContentType = "application/x-www-form-urlencoded";
            httpContext.Request.Body = new MemoryStream(System.Text.Encoding.UTF8.GetBytes("Model=" + JsonConvert.SerializeObject(boardData)));
            httpContext.Request.ContentLength = httpContext.Request.Body.Length;
            var controllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            var controller = new ObjectManagerController(new Localizer())
            {
                ControllerContext = controllerContext
            };
            controller.UpdateBoard();
            using var DBCon2 = new DataContext();
            Assert.Equal(boardData.BoardName, DBCon2.BoardData.Find(boardData.ID).BoardName);
        }
        [Fact]
        public void TestUpdateImage()
        {
            dummyImage.ImageName += " TestExtra";
            string name_holder = dummyImage.ImageName;
            var httpContext = new DefaultHttpContext();
            httpContext.Request.ContentType = "application/x-www-form-urlencoded";
            httpContext.Request.Body = new MemoryStream(System.Text.Encoding.UTF8.GetBytes("Model=" + JsonConvert.SerializeObject(dummyImage)));
            httpContext.Request.ContentLength = httpContext.Request.Body.Length;
            var controllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            var controller = new ObjectManagerController(new Localizer())
            {
                ControllerContext = controllerContext
            };
            controller.UpdateImage();

            using var DBCon = new DataContext();
            Assert.Equal(name_holder, DBCon.Image.Find(dummyImage.ID).ImageName);
        }

        [Fact]
        public void TestUpdateTile()
        {
            TileData dummyTile = new TileData
            {
                ActionContext = "",
                ActionType = TileBase.ActionID.Nothing,
                TileName = "Test Tile",
                TileText = "Test Tile",
                BackgroundColor = 0,
                SourceID = dummyImage.ID
            };
            using (var DBCon = new DataContext())
            {
                DBCon.TileData.Add(dummyTile);
                DBCon.SaveChanges();
            }
            dummyTile.TileName += " TestExtra";
            string name_holder = dummyTile.TileName;

            var httpContext = new DefaultHttpContext();
            httpContext.Request.ContentType = "application/x-www-form-urlencoded";
            httpContext.Request.Body = new MemoryStream(System.Text.Encoding.UTF8.GetBytes("Model=" + JsonConvert.SerializeObject(dummyTile)));
            httpContext.Request.ContentLength = httpContext.Request.Body.Length;
            var controllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            var controller = new ObjectManagerController(new Localizer())
            {
                ControllerContext = controllerContext
            };
            controller.UpdateTile();

            using var DBCon2 = new DataContext();
            Assert.Equal(name_holder, DBCon2.TileData.Find(dummyTile.ID).TileName);
        }
    }
}
