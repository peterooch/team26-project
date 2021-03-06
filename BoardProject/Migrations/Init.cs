﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace BoardProject.Migrations
{
    /* Every data object change or its base change must be reflected here! */
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserData",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false),
                    Username = table.Column<string>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: false),
                    PasswordSalt = table.Column<string>(nullable: false),
                    IsPrimary = table.Column<bool>(nullable: false),
                    IsManager = table.Column<bool>(nullable: false),
                    Language = table.Column<string>(nullable: true),
                    Font = table.Column<string>(nullable: true),
                    FontSize = table.Column<double>(nullable: true),
                    BackgroundColor = table.Column<int>(nullable: true),
                    TextColor = table.Column<int>(nullable: true),
                    HighContrast = table.Column<bool>(nullable: true),
                    DPI = table.Column<int>(nullable: true),
                    BoardIDs = table.Column<string>(nullable: true),
                    HomeBoardID = table.Column<string>(nullable: true),
                    ManagedUsersIDs = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserData", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BoardData",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false),
                    BoardName = table.Column<string>(nullable: false),
                    IsPublic = table.Column<bool>(nullable: false),
                    BackgroundColor = table.Column<int>(nullable: true),
                    TextColor = table.Column<int>(nullable: true),
                    FontSize = table.Column<double>(nullable: true),
                    Spacing = table.Column<double>(nullable: true),
                    BoardHeader = table.Column<string>(nullable: true),
                    TileIDs = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardData", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TileData",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false),
                    TileName = table.Column<string>(nullable: false),
                    IsPublic = table.Column<bool>(nullable: false),
                    TileText = table.Column<string>(nullable: true),
                    ActionType = table.Column<long>(nullable: false),
                    ActionContext = table.Column<string>(nullable: true),
                    BackgroundColor = table.Column<int>(nullable: true),
                    SourceID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TileData", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false),
                    Source = table.Column<string>(nullable: false),
                    ImageName = table.Column<string>(nullable: false),
                    Category = table.Column<string>(nullable: false),
                    ReferenceCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.ID);
                });
            migrationBuilder.CreateTable(
                name: "ActivityLogs",
                columns: table => new
                {
                    TimeStamp = table.Column<ulong>(nullable: false),
                    UserID = table.Column<int>(nullable: false),
                    ActivityType = table.Column<int>(nullable: false),
                    ActivityDescription = table.Column<string>(nullable: false)
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserData");
            migrationBuilder.DropTable(
                name: "BoardData");
            migrationBuilder.DropTable(
                name: "TileData");
            migrationBuilder.DropTable(
                name: "Image");
            migrationBuilder.DropTable(
                name: "ActivityLogs");
        }
    }
}
