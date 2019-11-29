using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BoardProject.Models;

namespace BoardProject.Data
{
    public class DataContext : DbContext
    {
        public DbSet<UserData>   DBUsers;
        public DbSet<BoardData>  DBBoards;
        public DbSet<ButtonData> DBButtons;
        public DbSet<Image>      DBImages;

        /* Use SQLite for now */
        protected override void OnConfiguring(DbContextOptionsBuilder options) =>
            options.UseSqlite("Data Source=data.db");
    }
}
