using AV00_Shared.Models;
using AV00_Control_Application.Utilities;
using Microsoft.EntityFrameworkCore;
using AV00_Control_Application.Models.Configuration;

namespace AV00_Control_Application.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<LogEventModel> LogMessages { get; set; }

        public ApplicationDbContext()
        {

        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbConnection = $"Filename={DbPath.GetPath("application.db")}";
            optionsBuilder.UseSqlite(dbConnection);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LogEventModelConfiguration).Assembly);
        }
    }
}
