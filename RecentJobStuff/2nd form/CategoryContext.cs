using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecentJobStuff._2nd_form
{
    public class CategoryContext : DbContext
    {
        public DbSet<CategoryModel> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer("Server=tcp:researcgh-db-server.database.windows.net,1433;Initial Catalog=research-db;Persist Security Info=False;User ID=sqlAdmin;Password=Password!1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

    }
}
