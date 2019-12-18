using System;
using Xunit;
using BoardProject.Models;
using BoardProject.Data;
using BoardProject.Controllers;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Tests
{
    public class TestControllerMethods
    {
        private readonly Localizer test_localizer = new Localizer();
        [Fact]
        public void TestHomeController_Index()
        {
            HomeController controller = new HomeController(test_localizer);
            ViewResult result = controller.Index() as ViewResult;
            Assert.NotNull(result);
        }
        [Fact]
        public void TestHomeController_Debug()
        {
            HomeController controller = new HomeController(test_localizer);
            ViewResult result = controller.Debug() as ViewResult;
            Assert.NotNull(result);
        }
        [Fact]
        public void TestHomeController_Contact()
        {
            HomeController controller = new HomeController(test_localizer);
            ViewResult result = controller.Contact() as ViewResult;
            Assert.NotNull(result);
        }
        [Fact]
        public void TestBoardViewController_Index()
        {
            BoardViewController controller = new BoardViewController(test_localizer);
            ViewResult result = controller.Index(null) as ViewResult;
            Assert.NotNull(result);
        }
        [Fact]
        public void TestLoginPageController_LoginPage()
        {
            LoginController controller = new LoginController(test_localizer);
            ViewResult result = controller.Index() as ViewResult;
            Assert.NotNull(result);
        }
        [Fact]
        public void TestMIController_MI()
        {
            MIController controller = new MIController();
            ViewResult result = controller.MI() as ViewResult;
            Assert.NotNull(result);
        }
        [Fact]
        public void TestTileController_AddTile()
        {
            TileController controller = new TileController();
            ViewResult result = controller.AddTile() as ViewResult;
            Assert.NotNull(result);
        }
        [Fact]
        public void TestDisplayTableController_DisplayTable()
        {
            DisplayTableController controller = new DisplayTableController();
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
        public void TestSetLocaleWithInCorrectID()
        {
            Localizer localizer = new Localizer();

            /* blah should not be any kind of language identitfier */
            Assert.Throws<Exception>(()=> localizer.SetLocale("blah"));
        }
        [Theory]
        [InlineData("dir","ltr")] // Should exist
        [InlineData("lang-en","English")] // Should exist
        [InlineData("dummy_key", "dummy_key")] // Should not exist
        public void TestGetString(string key, string expected)
        {
            Localizer localizer = new Localizer("en");

            Assert.Equal(expected, localizer[key]);
        }
    }
}
