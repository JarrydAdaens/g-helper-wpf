using System.Security.Principal;

namespace GHelper.Helpers
{
    /// <summary>
    /// Windows identity checks that are independent of any UI framework, so both
    /// executable heads and the shared layer itself can ask them.
    /// </summary>
    public static class UserIdentity
    {
        private static readonly Lazy<bool> _isSystem = new Lazy<bool>(() =>
        {
            using var identity = WindowsIdentity.GetCurrent();
            return identity.IsSystem;
        }, LazyThreadSafetyMode.ExecutionAndPublication);

        public static bool IsRunningAsSystem() => _isSystem.Value;

        public static bool IsUserAdministrator()
        {
            using WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
