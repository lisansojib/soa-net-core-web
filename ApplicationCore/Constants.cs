using System;

namespace ApplicationCore
{
    public static class Constants
    {
        public static string SYMMETRIC_SECURITY_KEY => Guid.NewGuid().ToString();
    }

    public static class UserRoles
    {
        public const string GENERAL = "General";
        public const string ADMIN = "Admin";
    }
}
