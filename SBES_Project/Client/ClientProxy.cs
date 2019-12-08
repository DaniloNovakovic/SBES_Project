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
            // TODO: podesiti autentifikaciju sa PrimaryService
            // TODO: podesiti autorizaciju sa PrimaryService
            Credentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Impersonation;
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
                Console.WriteLine("Error: {0}", e.Message);
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
                Console.WriteLine("Error while trying to get all alarms. Error message: {0}", e.Message);
            }

            return new List<Alarm>();
        }

        public void RemoveAlarms()
        {
            try
            {
                factory.RemoveAllAlarms();
            }
            catch (SecurityAccessDeniedException e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
        }
    }
}