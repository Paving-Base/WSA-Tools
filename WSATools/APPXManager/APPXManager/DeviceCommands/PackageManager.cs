using APPXManager.Helpers;
using APPXManager.Models;
using APPXManager.Receivers;
using System.Collections.ObjectModel;

namespace APPXManager.DeviceCommands
{
    /// <summary>
    /// Allows you to get information about packages that are installed on a device.
    /// </summary>
    public static class PackageManager
    {
        private const string ListFull = "Get-AppxPackage ";

        private const string FindByName = "Get-AppxPackage -Name ";

        static PackageManager()
        {

        }

        /// <summary>
        /// Refreshes the packages.
        /// </summary>
        public static ObservableCollection<PackageInfo> GetPackages(string? arg = null)
        {
            PackageManagerReceiver pmr = new PackageManagerReceiver();
            CommandHelper.ExecuteShellCommand($"{ListFull}{arg}", pmr);
            return pmr.Packages ?? new ObservableCollection<PackageInfo>();
        }

        /// <summary>
        /// Find the packages.
        /// </summary>
        public static (bool isfound, PackageInfo info) FindPackageByName(string name)
        {
            PackageManagerReceiver pmr = new PackageManagerReceiver();
            CommandHelper.ExecuteShellCommand($"{FindByName}{name}", pmr);
            if (pmr.Packages.Count <= 0) { return (false, new PackageInfo()); }
            else { return (true, pmr.Packages.First()); }
        }

        /// <summary>
        /// Launch Applicaton.
        /// </summary>
        /// <param name="packagename">Package Family Name</param>
        /// <param name="appname">Application ID</param>
        public static void LaunchPackage(string packagename, string appname = "App")
        {
            ConsoleOutputReceiver receiver = new ConsoleOutputReceiver();
            CommandHelper.ExecuteShellCommand($@"explorer.exe shell:appsFolder\{packagename}!{appname}", receiver);
        }
    }
}