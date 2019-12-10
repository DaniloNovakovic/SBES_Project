using Common;
using DAL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private void TrySendToSecondary(string srvCertCN = "replicatorservice")
        {
            while (true)
            {
                try
                {
                    var binding = new NetTcpBinding();
                    binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

                    // Get public certificate (.cer) from Replicator Service located in LocalMachine\TrustedPeople
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
                catch (CommunicationObjectFaultedException cex)
                {
                    Trace.TraceWarning(cex.Message);
                }
                catch (Exception ex)
                {
                    Trace.TraceError(ex.Message);
                }

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