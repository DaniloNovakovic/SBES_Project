using Common;
using System;
using System.ServiceModel;

namespace Client
{
    internal static class Program
    {
        private static bool exitFlag;

        private static void Main()
        {
            var binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;


            using (var proxy = new ClientProxy(binding, new EndpointAddress("net.tcp://localhost:15000/PrimaryService")))
            {
                while (!exitFlag)
                {
                    PrintMenu();
                    int input = GetInput();

                    ProcessAction(proxy, input); 
                }
            }

            Console.WriteLine("Press <enter> to exit...");
            Console.ReadKey();
        }

        private static void PrintMenu()
        {
            Console.WriteLine("=================================");
            Console.WriteLine("Available actions:");
            Console.WriteLine("  1. Send alarm");
            Console.WriteLine("  2. List all alarms");
            Console.WriteLine("  3. Remove my alarms");
            Console.WriteLine("  4. Remove all alarms");
            Console.WriteLine("  5. Check alarm removal requests");
            Console.WriteLine("  6. Exit");
        }

        private static int GetInput()
        {
            string input;
            int actionNum;
            do
            {
                Console.Write("Your action: ");
                input = Console.ReadLine();
            }
            while (!int.TryParse(input, out actionNum) || actionNum < 1 || actionNum > 6);

            return actionNum;
        }

        private static void ProcessAction(ClientProxy proxy, int actionNum)
        {
            switch (actionNum)
            {
                case 1:
                    SendAlarm(proxy);
                    break;
                case 2:
                    ListAllAlarms(proxy);
                    break;
                case 3:
                    RemoveClientAlarms(proxy);
                    break;
                case 4:
                    RemoveAllAlarms(proxy);
                    break;
                case 5:
                    AlarmRemovealRequests(proxy);
                    break;
                case 6:
                    exitFlag = true;
                    break;
                default:
                    Console.WriteLine("Invalid action.");
                    break;
            }
        }

        private static void RemoveAllAlarms(ClientProxy proxy)
        {
            if (proxy.RemoveAllAlarms())
            {
                Console.WriteLine("Successfully removed all alarms.");
            }
        }

        private static void RemoveClientAlarms(ClientProxy proxy)
        {
            if (proxy.RemoveClientAlarms())
            {
                Console.WriteLine("CLient alarms waiting to be approved for removal.");
            }
        }

        private static void ListAllAlarms(ClientProxy proxy)
        {
            var allAlarms = proxy.GetAllAlarms();
            if (allAlarms.Item2)
            {
                Console.WriteLine("Listing all alarms ...");
                foreach (var item in allAlarms.Item1)
                {
                    Console.WriteLine(item);
                }
            }
        }

        private static void SendAlarm(ClientProxy proxy)
        {
            if (proxy.SendAlarm(CreateAlarm()))
            {
                Console.WriteLine("Alarm sent.");
            }
        }

        private static Alarm CreateAlarm()
        {
            Console.Write("Enter message for alarm: ");
            var alarm = new Alarm(DateTime.Now.TimeOfDay, Console.ReadLine());
            return alarm;
        }

        private static void AlarmRemovealRequests(ClientProxy proxy)
        {
            var requests = proxy.GetClientRemovalRequests();
            if (requests.Item2)
            {
                Console.WriteLine("Getting clients removal requests...");
                foreach (string item in requests.Item1)
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
                        if (proxy.ApprovedRemoval(item))
                        {
                            Console.WriteLine("Request approved.");
                        }
                    }
                    if (response.Equals("N", StringComparison.OrdinalIgnoreCase))
                    {
                        if (proxy.DeniedRemoval(item))
                        {
                            Console.WriteLine("Request denied.");
                        }
                    }
                }
            }
        }
    }
}