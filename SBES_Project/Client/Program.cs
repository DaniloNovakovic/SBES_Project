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

            using (var proxy = new ClientProxy(binding, new EndpointAddress("net.tcp://localhost:15000/PrimaryService")))
            {
                SendAlarmsTest(proxy);
                Console.WriteLine();

                GetAllAlarmsTest(proxy);
                Console.WriteLine();

                RemoveAlarmsTest(proxy);
                Console.WriteLine();

                GetClientRemovalRequestsTest(proxy);
                Console.WriteLine();
            }

            Console.ReadKey();
        }

        private static void GetClientRemovalRequestsTest(ClientProxy proxy)
        {
            foreach (var item in proxy.GetClientRemovalRequests())
            {
                Console.WriteLine($"Request from client: {item}");
                string response;
                do
                {
                    Console.Write($"Allow request? (Y/N): ");
                    response = Console.ReadLine();
                } while (!(response.Equals("Y", StringComparison.OrdinalIgnoreCase) || response.Equals("N", StringComparison.OrdinalIgnoreCase)));

                if (response.Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    proxy.ApprovedRemoval(item);
                }
                if (response.Equals("N", StringComparison.OrdinalIgnoreCase))
                {
                    proxy.DeniedRemoval(item);
                }
            }
        }

        private static void RemoveAlarmsTest(ClientProxy proxy)
        {
            proxy.RemoveAlarms();
        }

        private static void GetAllAlarmsTest(ClientProxy proxy)
        {
            foreach (var item in proxy.GetAllAlarms())
            {
                Console.WriteLine(item);
            }
        }

        private static void SendAlarmsTest(ClientProxy proxy)
        {
            for (int i = 0; i < 5; i++)
            {
                proxy.SendAlarm(new Alarm(new TimeSpan(5 + i, 32 + i, 14 + i), $"Message {i}"));
            }
        }
    }
}