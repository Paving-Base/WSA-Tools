using APPXManager.DeviceCommands;
using APPXManager.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSATools.Core.Helpers
{
    public static class WSAHelper
    {
        public static bool IsWSARunning
        {
            get
            {
                var ps = Process.GetProcessesByName("vmmemWSA");
                return ps != null && ps.Length > 0;
            }
        }

        private const string StartCMD = @"explorer.exe shell:appsFolder\MicrosoftCorporationII.WindowsSubsystemForAndroid_8wekyb3d8bbwe!App";

        public static (bool, PackageInfo) GetWSAInfo() => PackageManager.FindPackageByName("MicrosoftCorporationII.WindowsSubsystemForAndroid");

        public static void StartWSA()
        {
            Command.Instance.Excute(StartCMD, out _);
            while (IsWSARunning) ;
        }

    }
}
