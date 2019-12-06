using System.ServiceModel;
using System;
using Common;

namespace Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            
            using (var proxy = new ClientProxy(binding, new EndpointAddress("net.tcp://localhost:15000/PrimaryService")))
            {
                for (int i = 0; i < 5; i++)
                {
                    proxy.SendAlarm(new Alarm(new TimeSpan(5 + i, 32 + i, 14 + i), $"Client {i}", $"Message {i}")); ;
                }
            }

            Console.ReadKey();
        }
    }
}
