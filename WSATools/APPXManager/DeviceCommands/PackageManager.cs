using APPXManager.Helpers;
using APPXManager.Models;
using APPXManager.Receivers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
