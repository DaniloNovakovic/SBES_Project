using Common;
using DAL;
using System;
using System.ServiceModel;

namespace PrimaryService
{
    public class PrimaryService : IPrimaryService
    {
        private readonly string _serviceId = "PrimaryService";
        public const string DefaultConnectionString = "DefaultConnection";

        public void SendAlarm(Alarm alarm)
        {
            // TODO:  provera ovlascenja klijenta

            SaveToDatabase(alarm);

            // TODO:  smestanje u buffer za repliciranje

            Console.WriteLine($"Sending alarm: {alarm}");

            var binding = new NetTcpBinding();

            using (var proxy = new ReplicatorProxy(binding, new EndpointAddress("net.tcp://localhost:15001/Replicator")))
            {
                proxy.SendToSecondary(alarm);
            }
        }

        private void SaveToDatabase(Alarm alarm)
        {
            using (var repo = AlarmRepositoryFactory.CreateNew(DefaultConnectionString))
            {
                repo.Add(_serviceId, alarm);
            }
        }
    }
}