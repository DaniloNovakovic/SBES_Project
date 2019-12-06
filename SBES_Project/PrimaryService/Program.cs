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
                var binding = new NetTcpBinding();
                binding.Security.Mode = SecurityMode.Transport;
                binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;

                host.AddServiceEndpoint(typeof(IPrimaryService), binding, "net.tcp://localhost:15000/PrimaryService");
                host.Open();

                Console.WriteLine($"{nameof(PrimaryService)} is started.");
                Console.WriteLine("Press <enter> to stop service...");

                Console.ReadLine();
                host.Close();
            }
        }
    }
}
