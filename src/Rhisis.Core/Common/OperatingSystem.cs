using System.Runtime.InteropServices;

namespace Rhisis.Core.Common
{
    public static class OperatingSystem
    {
        /// <summary>
        /// Check if the current operating system is a Windows.
        /// </summary>
        /// <returns></returns>
        public static bool IsWindows() => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        /// <summary>
        /// Check if the current operating system is a mac.
        /// </summary>
        /// <returns></returns>
        public static bool IsMacOS() => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        /// <summary>
        /// Check if the current operating system is a Linux.
        /// </summary>
        /// <returns></returns>
        public static bool IsLinux() => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
    }
}
