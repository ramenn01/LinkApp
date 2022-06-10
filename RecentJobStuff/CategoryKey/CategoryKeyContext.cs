﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecentJobStuff.CategoryKey
{
    public class CategoryKeyContext : DbContext
    {
        public DbSet<CategoryKeyModel> CategoryKeys { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer("Server=tcp:researcgh-db-server.database.windows.net,1433;Initial Catalog=research-db;Persist Security Info=False;User ID=sqlAdmin;Password=Password!1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

    }
}
