using DAL.Configurations;
using System.Data.Entity;

namespace DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base()
        {
        }

        public ApplicationDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        public DbSet<AlarmEntity> Alarms { get; set; }

        public DbSet<ClientRequestsEntity> ClientRequests { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.AddFromAssembly(typeof(AlarmConfiguration).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}