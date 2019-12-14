using Common;
using Common.Auditing;
using DAL;
using System;

namespace SecondaryService
{
    public class Replicator : IReplicator
    {
        private readonly string _serviceId = "SecondaryService";
        public const string DefaultConnectionString = "DefaultConnection";

        public void SendAlarm(Alarm alarm)
        {
            try
            {
                SaveToDatabase(alarm);

                Console.WriteLine($"Alarm saved: {alarm}");
                Audit.ReplicationSuccess(alarm);
            }
            catch (Exception ex)
            {
                Audit.ReplicationFailure(alarm, ex.Message);
                throw;
            }
        }

        private void SaveToDatabase(Alarm alarm)
        {
            using (var repo = AlarmRepositoryFactory.CreateNew(DefaultConnectionString))
            {
                repo.Add(_serviceId, alarm);
            }
        }

        public void CheckForConnection()
        {
        }
    }
}