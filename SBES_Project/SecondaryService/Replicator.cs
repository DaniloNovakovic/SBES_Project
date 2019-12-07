using Common;
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
            SaveToDatabase(alarm);

            Console.WriteLine($"Sending alarm: {alarm}");
        }

        private void SaveToDatabase(Alarm alarm)
        {
            using (var repo = AlarmRepositoryFactory.CreateNew(DefaultConnectionString))
            {
                repo.Add(_serviceId, alarm);
            }
        }

        public void CheckForConnection() { }
    }
}