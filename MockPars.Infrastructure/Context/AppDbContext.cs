using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MockPars.Domain.Models;

namespace MockPars.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
          //  modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(AppDbContext)));
          
        }

        #region DbSet

        public DbSet<User> User { get; set; }
        public DbSet<Databases> Databases { get; set; }
        public DbSet<Columns> Columns { get; set; }
        public DbSet<Tables> Tables { get; set; }




        #endregion


    }
}
