using DAL.Configurations;
using System.Data.Entity;

namespace DAL
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<AlarmEntity> Alarms { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.AddFromAssembly(typeof(AlarmConfiguration).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}