using System.ServiceModel;

namespace Common.Security
{
    public class CustomServiceAuthorizationManager : ServiceAuthorizationManager
    {
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            return (operationContext.ServiceSecurityContext.AuthorizationContext.Properties["Principal"] as CustomPrincipal).IsInRole("Delete");
        }
    }
}
