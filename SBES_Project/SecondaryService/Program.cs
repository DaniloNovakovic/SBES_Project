using Common;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Security;

namespace SecondaryService
{
    internal static class Program
    {
        private static void Main()
        {
            try
            {
                using (var host = new ServiceHost(typeof(Replicator)))
                {
                    SetupHost(host);
                    host.Open();

                    Console.WriteLine($"{nameof(Replicator)} is started.");
                    Console.WriteLine("Press <enter> to stop service...");

                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

        private static void SetupHost(ServiceHost host)
        {
            var binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            host.AddServiceEndpoint(typeof(IReplicator), binding, "net.tcp://localhost:15001/Replicator");

            host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;

            ///If CA doesn't have a CRL associated, WCF blocks every client because it cannot be validated
            host.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            string srvCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            ///Set appropriate service's certificate on the host. Use CertManager class to obtain the certificate based on the "srvCertCN"
            host.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My,
                StoreLocation.LocalMachine, srvCertCN);
        }
    }
}