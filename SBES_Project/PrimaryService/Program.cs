using Common;
using Common.Security;
using System;
using System.Collections.Generic;
using System.IdentityModel.Policy;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace PrimaryService
{
    internal static class Program
    {
        private static void Main()
        {
            using (var host = new ServiceHost(typeof(PrimaryService)))
            {
                var binding = new NetTcpBinding();
                binding.Security.Mode = SecurityMode.Transport;
                binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;

                host.AddServiceEndpoint(typeof(IPrimaryService), binding, "net.tcp://localhost:15000/PrimaryService");
                host.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
                host.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });

                host.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;
                var policies = new List<IAuthorizationPolicy>
                {
                    new CustomAuthorizationPolicy()
                };
                host.Authorization.ExternalAuthorizationPolicies = policies.AsReadOnly();
                host.Authorization.ServiceAuthorizationManager = new CustomServiceAuthorizationManager();

                host.Open();

                Console.WriteLine($"{nameof(PrimaryService)} is started.");
                Console.WriteLine("Press <enter> to stop service...");

                Console.ReadLine();
            }
        }
    }
}