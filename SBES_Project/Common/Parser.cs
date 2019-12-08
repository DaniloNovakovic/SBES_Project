using System.Security.Principal;

namespace Common
{
    public static class Parser
    {
        public static string Parse(IdentityReference name)
        {
            return name.Value.Contains("@") ? name.Value.Split('@')[0] : name.Value.Contains("\\") ? name.Value.Split('\\')[1] : name.Value;
        }
    }
}
