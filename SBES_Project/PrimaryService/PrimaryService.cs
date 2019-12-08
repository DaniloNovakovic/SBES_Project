using Common;
using DAL;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Threading.Tasks;

namespace PrimaryService
{
    public class PrimaryService : IPrimaryService
    {
        private readonly string _serviceId = "PrimaryService";
        public const string DefaultConnectionString = "DefaultConnection";

        private readonly Queue<Alarm> replicationBuffer = new Queue<Alarm>();

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
                    binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

                    /// Define the expected service certificate. It is required to establish
                    /// communication using certificates.
                    const string srvCertCN = "SecondaryService";

                    /// Use CertManager class to obtain the certificate based on the "srvCertCN"
                    /// representing the expected service identity.
                    var srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);
                    var address = new EndpointAddress(
                        new Uri("net.tcp://localhost:15001/Replicator"),
                        new X509CertificateEndpointIdentity(srvCert)
                    );

                    using (var proxy = new ReplicatorProxy(binding, address))
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