using Common;
using DAL;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace PrimaryService
{
    public class PrimaryService : IPrimaryService
    {
        private readonly string _serviceId = "PrimaryService";
        public const string DefaultConnectionString = "DefaultConnection";

        private Queue<Alarm> replicationBuffer = new Queue<Alarm>();

        public PrimaryService()
        {
            new Task(() => TrySendToSecondary(), TaskCreationOptions.LongRunning).Start();
        }

        public void SendAlarm(Alarm alarm)
        {
            // TODO:  provera ovlascenja klijenta

            SaveToDatabase(alarm);

            // TODO:  smestanje u buffer za repliciranje
            replicationBuffer.Enqueue(alarm);
        }

        private void TrySendToSecondary()
        {
            while (true)
            {
                try
                {
                    var binding = new NetTcpBinding();

                    using (var proxy = new ReplicatorProxy(binding, new EndpointAddress("net.tcp://localhost:15001/Replicator")))
                    {
                        if (proxy.CheckForReplicator())
                        {
                            while (replicationBuffer.Count > 0)
                            {
                                var alarm = replicationBuffer.Dequeue();
                                proxy.SendToSecondary(alarm);
                                Console.WriteLine($"Sent alarm: {alarm}");
                            }
                        }
                    }
                }
                catch (CommunicationObjectFaultedException) { }

                Task.Delay(500);
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