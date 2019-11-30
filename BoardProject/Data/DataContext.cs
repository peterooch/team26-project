﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BoardProject.Models;

namespace BoardProject.Data
{
    public class DataContext : DbContext
    {
        /* Use SQLite for now */
        protected override void OnConfiguring(DbContextOptionsBuilder options) =>
            options.UseSqlite("Data Source=data.db");

        public DbSet<UserData> UserData { get; set; }
        public DbSet<BoardData> BoardData { get; set; }
        public DbSet<ButtonData> ButtonData { get; set; }
        public DbSet<Image> Image { get; set; }
    }
}
