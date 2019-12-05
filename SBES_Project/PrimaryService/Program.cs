using Common;
using System;
using System.ServiceModel;

namespace PrimaryService
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(PrimaryService)))
            {
                host.AddServiceEndpoint(typeof(IPrimaryService), new NetTcpBinding(), "net.tcp://localhost:15000/PrimaryService");
                host.Open();

                Console.WriteLine($"{nameof(PrimaryService)} is started.");
                Console.WriteLine("Press <enter> to stop service...");

                Console.ReadLine();
                host.Close();
            }
        }
    }
}
