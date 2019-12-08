using Common;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Security.Principal;
using System.ServiceModel;
using System.Threading;
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

        [PrincipalPermission(SecurityAction.Demand, Role = "Add")]
        public void SendAlarm(Alarm alarm)
        {
            alarm.NamoOfClient = Parser.Parse((Thread.CurrentPrincipal.Identity as WindowsIdentity).User.Translate(typeof(NTAccount)));

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

        [PrincipalPermission(SecurityAction.Demand, Role = "Delete")]
        //[OperationBehavior(Impersonation = ImpersonationOption.Required)]
        public void RemoveAllAlarms()
        {
            using (var repo = AlarmRepositoryFactory.CreateNew(DefaultConnectionString))
            {
                var identity = WindowsIdentity.GetCurrent();

                using (identity.Impersonate())
                {
                    var identity2 = Thread.CurrentPrincipal.Identity as WindowsIdentity;
                    Console.WriteLine(identity2.Name);

                    var a = WindowsIdentity.GetCurrent().Name;
                    Console.WriteLine(a);
                }

                //var identity = Thread.CurrentPrincipal.Identity as WindowsIdentity;
                //using (identity.Impersonate())
                //{
                //    var identity2 = Thread.CurrentPrincipal.Identity as WindowsIdentity;
                //    Console.WriteLine(identity2.Name);

                //    var a = WindowsIdentity.GetCurrent().Name;
                //    Console.WriteLine(a);
                //}

                //Console.WriteLine(identity.Name);


                //var client = identity.User.Translate(typeof(NTAccount));
                //var clientName = Parser.Parse(client);
                //Console.WriteLine(clientName);

                //repo.DeleteAllByClientName(clientName);
            }
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