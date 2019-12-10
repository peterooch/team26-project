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
            /* FIXME: Fix issues with DataContext not finding the tables */
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
            LoginPageController controller = new LoginPageController();
            ViewResult result = controller.LoginPage() as ViewResult;
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
            ViewResult result = controller.DisplayTable() as ViewResult;
            Assert.NotNull(result);
        }
    }
}
