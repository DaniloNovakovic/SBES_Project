using Common;

namespace DAL
{
    public static class AlarmRepositoryFactory
    {
        public static AlarmRepository CreateNew(string nameOrConnectionString)
        {
            return new AlarmRepository(new ApplicationDbContext(nameOrConnectionString));
        }
    }
}