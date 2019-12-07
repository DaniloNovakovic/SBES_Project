﻿using Common;
using System;
using System.ServiceModel;

namespace PrimaryService
{
    public class ReplicatorProxy : ChannelFactory<IReplicator>
    {
        private readonly IReplicator factory;

        public ReplicatorProxy(NetTcpBinding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress)
        {
            factory = CreateChannel();
        }

        public void SendToSecondary(Alarm alarm)
        {
            try
            {
                factory.SendAlarm(alarm);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
        }

        public bool CheckForReplicator()
        {
            try
            {
                factory.CheckForConnection();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}