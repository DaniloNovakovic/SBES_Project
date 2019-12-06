using System.Data.Entity.ModelConfiguration;

namespace DAL.Configurations
{
    public class AlarmConfiguration : EntityTypeConfiguration<AlarmEntity>
    {
        public AlarmConfiguration()
        {
            Property(a => a.ServiceId).IsRequired();
            Property(a => a.ClientName).IsRequired();
            Property(a => a.Message).IsRequired();
        }
    }
}