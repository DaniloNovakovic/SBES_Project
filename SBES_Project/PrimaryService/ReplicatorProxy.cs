using Common;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Security;

namespace PrimaryService
{
    public class ReplicatorProxy : ChannelFactory<IReplicator>
    {
        private readonly IReplicator factory;

        public ReplicatorProxy(NetTcpBinding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress)
        {
            Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;
            Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            /// cltCertCN.SubjectName should be set to the client's username. .NET WindowsIdentity
            /// class provides information about Windows user running the given process
			string cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            /// Set appropriate client's certificate on the channel. Use CertManager class to obtain
            /// the certificate based on the "cltCertCN"
            Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My,
                StoreLocation.LocalMachine, cltCertCN);

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