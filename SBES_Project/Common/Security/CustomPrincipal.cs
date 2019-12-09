using System.Collections.Generic;
using System.Security.Principal;

namespace Common.Security
{
    public class CustomPrincipal : IPrincipal
    {
        private WindowsIdentity windowsIdentity;

        public CustomPrincipal(WindowsIdentity windowsIdentity)
        {
            this.windowsIdentity = windowsIdentity;
        }

        public IIdentity Identity => windowsIdentity;

        public bool IsInRole(string role)
        {
            foreach (var item in windowsIdentity.Groups)
            {
                var name = ((SecurityIdentifier)item.Translate(typeof(SecurityIdentifier))).Translate(typeof(NTAccount));
                var groupName = Formatter.Format(name);

                var permision = ClientRoleConfigFile.ResourceManager.GetObject(groupName);
                if (permision != null)
                {
                    var permissions = permision.ToString().Split('|');
                    foreach (var perm in permissions)
                    {
                        if (role == perm)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}