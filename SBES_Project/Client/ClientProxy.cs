using Common;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Security;

namespace Client
{
    public class ClientProxy : ChannelFactory<IPrimaryService>
    {
        private readonly IPrimaryService factory;

        public ClientProxy(NetTcpBinding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress)
        {
            factory = CreateChannel();
        }

        public bool SendAlarm(Alarm alarm)
        {
            try
            {
                factory.SendAlarm(alarm);
            }
            catch (SecurityAccessDeniedException e)
            {
                Console.WriteLine("Error while trying to send alarm: {0}", e.Message);
                return false;
            }
            return true;
        }

        public Tuple<List<Alarm>, bool> GetAllAlarms()
        {
            var alarms = new List<Alarm>();
            try
            {
                alarms = factory.GetAlarms();
            }
            catch (SecurityAccessDeniedException e)
            {
                Console.WriteLine("Error while trying to get all alarms: {0}", e.Message);
                return new Tuple<List<Alarm>, bool>(alarms, false);
            }

            return new Tuple<List<Alarm>, bool>(alarms, true);
        }

        public bool RemoveClientAlarms()
        {
            try
            {
                factory.RemoveClientAlarms();
            }
            catch (SecurityAccessDeniedException e)
            {
                Console.WriteLine("Error while trying to remove alarm: {0}", e.Message);
                return false;
            }
            return true;
        }

        public Tuple<List<string>, bool> GetClientRemovalRequests()
        {
            var requests = new List<string>();
            try
            {
                requests = factory.GetClientRemovalRequests();
            }
            catch (SecurityAccessDeniedException e)
            {
                Console.WriteLine("Error while trying to get requests: {0}", e.Message);
                return new Tuple<List<string>, bool>(requests, false);
            }

            return new Tuple<List<string>, bool>(requests, true);
        }

        public bool ApprovedRemoval(string clientName)
        {
            try
            {
                factory.ApprovedRemoval(clientName);
            }
            catch (SecurityAccessDeniedException e)
            {
                Console.WriteLine("Error while approving removal: {0}", e.Message);
                return false;
            }
            return true;
        }

        public bool DeniedRemoval(string clientName)
        {
            try
            {
                factory.DeniedRemoval(clientName);
            }
            catch (SecurityAccessDeniedException e)
            {
                Console.WriteLine("Error while denying removal: {0}", e.Message);
                return false;
            }
            return true;
        }

        public bool RemoveAllAlarms()
        {
            try
            {
                factory.RemoveAllAlarms();
            }
            catch (SecurityAccessDeniedException e)
            {
                Console.WriteLine("Error while trying to remove all alarms: {0}", e.Message);
                return false;
            }
            return true;
        }
    }
}