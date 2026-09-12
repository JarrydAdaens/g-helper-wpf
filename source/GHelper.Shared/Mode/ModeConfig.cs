namespace GHelper.Mode
{
    /// <summary>
    /// The performance-mode identifiers and the config-backed lookup of the mode
    /// currently in effect. This is the part of mode handling that is pure configuration,
    /// so it can live in the shared layer; naming, enumeration and applying a mode stay
    /// with the executable head, which needs localized strings and live ACPI access.
    /// </summary>
    public static class ModeConfig
    {
        public const int PerformanceBalanced = 0;
        public const int PerformanceTurbo = 1;
        public const int PerformanceSilent = 2;
        public const int PerformanceFullSpeed = 3;
        public const int PerformanceManual = 4;

        /// <summary>The mode the user last selected, custom modes included.</summary>
        public static int GetCurrent()
        {
            return AppConfig.Get("performance_mode");
        }

        /// <summary>
        /// The built-in mode a mode is derived from. Modes 0-2 are their own base;
        /// a custom mode stores the base it was created from.
        /// </summary>
        public static int GetBase(int in_mode)
        {
            if (in_mode >= 0 && in_mode <= 2)
            {
                return in_mode;
            }

            return AppConfig.Get("mode_base_" + in_mode);
        }

        public static int GetCurrentBase()
        {
            return GetBase(GetCurrent());
        }
    }
}
