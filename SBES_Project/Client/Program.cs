using System.ServiceModel;
using System;
using Common;

namespace Client
{
    internal static class Program
    {
        private static void Main()
        {
            var binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;

            var identity = EndpointIdentity.CreateUpnIdentity("user2@user2");
            var uri = new Uri("net.tcp://localhost:15000/PrimaryService");
            var remoteAddress = new EndpointAddress(uri, identity);
            
            using (var proxy = new ClientProxy(binding, remoteAddress))
            {
                for (int i = 0; i < 5; i++)
                {
                    proxy.SendAlarm(new Alarm(new TimeSpan(5 + i, 32 + i, 14 + i), $"Message {i}"));
                }
                Console.WriteLine();
                Console.WriteLine();
                foreach (var item in proxy.GetAllAlarms())
                {
                    Console.WriteLine(item);
                }
                Console.WriteLine();
                Console.WriteLine();

                proxy.RemoveAlarms();
            }

            Console.ReadKey();
        }
    }
}