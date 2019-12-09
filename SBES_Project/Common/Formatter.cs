using System.Security.Principal;

namespace Common
{
    public static class Formatter
    {
        public static string Format(IdentityReference name)
        {
            return name.Value.Contains("@") ? name.Value.Split('@')[0] : name.Value.Contains("\\") ? name.Value.Split('\\')[1] : name.Value;
        }
    }
}
