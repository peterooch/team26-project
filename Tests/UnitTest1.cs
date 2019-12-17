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
        [Fact]
        public void TestHomeController_Index()
        {
            HomeController controller = new HomeController();
            ViewResult result = controller.Index() as ViewResult;
            Assert.NotNull(result);
        }
        [Fact]
        public void TestHomeController_Debug()
        {
            HomeController controller = new HomeController();
            ViewResult result = controller.Debug() as ViewResult;
            Assert.NotNull(result);
        }
        [Fact]
        public void TestHomeController_Contact()
        {
            HomeController controller = new HomeController();
            ViewResult result = controller.Contact() as ViewResult;
            Assert.NotNull(result);
        }
        [Fact]
        public void TestBoardViewController_Index()
        {
            BoardViewController controller = new BoardViewController();
            ViewResult result = controller.Index(null) as ViewResult;
            Assert.NotNull(result);
        }
        [Fact]
        public void TestLoginPageController_LoginPage()
        {
            LoginController controller = new LoginController();
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
    }
}
