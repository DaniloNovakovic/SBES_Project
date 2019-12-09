using Common;
using System;
using System.Collections.Generic;
using System.Security.Principal;
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

        public void SendAlarm(Alarm alarm)
        {
            try
            {
                factory.SendAlarm(alarm);
            }
            catch (SecurityAccessDeniedException e)
            {
                Console.WriteLine("Error while trying to send alarm: {0}", e.Message);
            }
        }

        public List<Alarm> GetAllAlarms()
        {
            try
            {
                return factory.GetAlarms();
            }
            catch (SecurityAccessDeniedException e)
            {
                Console.WriteLine("Error while trying to get all alarms: {0}", e.Message);
            }

            return new List<Alarm>();
        }

        public void RemoveAlarms()
        {
            try
            {
                factory.RemoveClientAlarms();
            }
            catch (SecurityAccessDeniedException e)
            {
                Console.WriteLine("Error while trying to remove alarm: {0}", e.Message);
            }
        }

        public List<string> GetClientRemovalRequests()
        {
            try
            {
                return factory.GetClientRemovalRequests();
            }
            catch (SecurityAccessDeniedException e)
            {
                Console.WriteLine("Error while trying to get requests: {0}", e.Message);
            }

            return new List<string>();
        }

        public void ApprovedRemoval(string clientName)
        {
            try
            {
                factory.ApprovedRemoval(clientName);
            }
            catch (SecurityAccessDeniedException e)
            {
                Console.WriteLine("Error while approving removal: {0}", e.Message);
            }
        }

        public void DeniedRemoval(string clientName)
        {
            try
            {
                factory.DeniedRemoval(clientName);
            }
            catch (SecurityAccessDeniedException e)
            {
                Console.WriteLine("Error while denying removal: {0}", e.Message);
            }
        }
    }
}