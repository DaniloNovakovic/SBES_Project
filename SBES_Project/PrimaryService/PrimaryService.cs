using Common;
using DAL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Linq;
using System.Security.Permissions;
using System.Security.Principal;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using Common.Auditing;

namespace PrimaryService
{
    public class PrimaryService : IPrimaryService
    {
        private readonly string _serviceId = "PrimaryService";
        public const string DefaultConnectionString = "DefaultConnection";

        private readonly Queue<Alarm> replicationBuffer = new Queue<Alarm>();

        public PrimaryService()
        {
            new Task(async() => await TrySendToSecondary(), TaskCreationOptions.LongRunning).Start();
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Add")]
        public void SendAlarm(Alarm alarm)
        {
            alarm.NamoOfClient = Formatter.ParseName((Thread.CurrentPrincipal.Identity as WindowsIdentity).User.Translate(typeof(NTAccount)).Value);

            SaveToDatabase(alarm);

            replicationBuffer.Enqueue(alarm);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Read")]
        public List<Alarm> GetAlarms()
        {
            using (var repo = AlarmRepositoryFactory.CreateNew(DefaultConnectionString))
            {
                return repo.GetAll().ToList();
            }
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "DeleteAll")]
        public void RemoveAllAlarms()
        {
            using (var repo = AlarmRepositoryFactory.CreateNew(DefaultConnectionString))
            {
                repo.DeleteAll();
            }
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Delete")]
        public void RemoveClientAlarms()
        {
            var clientName = Formatter.ParseName((Thread.CurrentPrincipal.Identity as WindowsIdentity).User.Translate(typeof(NTAccount)).Value);

            AddForRemovalEvaluation(clientName);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "DeleteAll")]
        public List<string> GetClientRemovalRequests()
        {
            using (var repo = AlarmRepositoryFactory.CreateNew(DefaultConnectionString))
            {
                return repo.GetAllClientRequests().ToList();
            }
        }

        private void AddForRemovalEvaluation(string clientName)
        {
            using (var repo = AlarmRepositoryFactory.CreateNew(DefaultConnectionString))
            {
                repo.AddClientRequest(clientName);
            }
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "DeleteAll")]
        public void ApprovedRemoval(string clientName)
        {
            using (var repo = AlarmRepositoryFactory.CreateNew(DefaultConnectionString))
            {
                repo.DeleteAllByClientName(clientName);
                repo.RemoveClientRequest(clientName);
            }
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "DeleteAll")]
        public void DeniedRemoval(string clientName)
        {
            using (var repo = AlarmRepositoryFactory.CreateNew(DefaultConnectionString))
            {
                repo.RemoveClientRequest(clientName);
            }
        }

        private async Task TrySendToSecondary(string srvCertCN = "replicatorservice")
        {
            while (true)
            {
                if (replicationBuffer.Count == 0)
                {
                    continue;
                }

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
                                Audit.ReplicationInitiated(); // throws error for some reason.
                                var alarm = replicationBuffer.Dequeue();
                                proxy.SendToSecondary(alarm);
                                Console.WriteLine($"Sent alarm: {alarm}");
                            }
                        }
                    }
                }
                catch (CommunicationObjectFaultedException) { }
                catch (Exception ex)
                {
                    Trace.TraceError(ex.Message);
                }

                await Task.Delay(500);
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