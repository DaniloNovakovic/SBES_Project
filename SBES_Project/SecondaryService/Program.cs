using Common;
using System;
using System.ServiceModel;

namespace SecondaryService
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(Replicator)))
            {
                // TODO: sertifikati autentifikacija (ChainTrust)

                host.AddServiceEndpoint(typeof(IReplicator), new NetTcpBinding(), "net.tcp://localhost:15001/Replicator");
                host.Open();

                Console.WriteLine($"{nameof(Replicator)} is started.");
                Console.WriteLine("Press <enter> to stop service...");

                Console.ReadLine();
                host.Close();
            }
        }
    }
}
