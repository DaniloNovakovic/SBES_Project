using Common;
using System;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Security;

namespace PrimaryService
{
    public class ReplicatorProxy : ChannelFactory<IReplicator>
    {
        private readonly IReplicator factory;

        public ReplicatorProxy(NetTcpBinding binding, EndpointAddress remoteAddress, string cltCertCN = "replicatorclient") : base(binding, remoteAddress)
        {
            Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;
            Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            /// Get private (.pfx) certificate from LocalMachine\My (Personal) for Replicator Client
            Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);

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