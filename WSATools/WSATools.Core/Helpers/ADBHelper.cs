using AdvancedSharpAdbClient;
using AdvancedSharpAdbClient.DeviceCommands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WSATools.Core.Models;
using WSATools.Core.Receivers;

namespace WSATools.Core.Helpers
{
    public static class ADBHelper
    {
        private static IEnumerable<DeviceData> DeviceList;

        public static DeviceData WSA;
        public static bool IsConnectWSA = false;
        public static StartServerResult? ADBStatus;
        public static AdvancedAdbClient ADBClient = new AdvancedAdbClient();
        public static DeviceMonitor Monitor = new DeviceMonitor(new AdbSocket(new IPEndPoint(IPAddress.Loopback, AdvancedAdbClient.AdbServerPort)));

        public static void InitilizeADB() => InitilizeADB(null);
        public static void InitilizeADB(IProgress<double> progress)
        {
            Process[] processes = Process.GetProcessesByName("adb");
            if (processes != null && processes.Any())
            {
                ADBStatus = new AdbServer().StartServer(processes.First().MainModule.FileName, restartServerIfNewer: false);
            }
            if (progress != null)
            {
                progress.Report(50);
            }
            if (ADBStatus != null)
            {
                Monitor.DeviceChanged += OnDeviceChanged;
                DeviceList = new AdvancedAdbClient().GetDevices();
            }
            if (progress != null)
            {
                progress.Report(100);
            }
        }

        public static void ConnectWSA() => ConnectWSA(null);
        public static void ConnectWSA(IProgress<double> progress)
        {
            if (DeviceList != null && DeviceList.Any())
            {
                ConsoleOutputReceiver receiver = new ConsoleOutputReceiver();
                foreach (DeviceData device in DeviceList)
                {
                    if (progress != null)
                    {
                        progress.Report((double)(DeviceList.ToList().IndexOf(device) + 1) * 100 / DeviceList.Count());
                    }
                    if (device == null || device.State == DeviceState.Offline) { continue; }
                    ADBClient.ExecuteRemoteCommand("getprop ro.boot.hardware", device, receiver);
                    if (receiver.ToString().Contains("windows"))
                    {
                        WSA = device ?? WSA;
                        IsConnectWSA = true;
                        break;
                    }
                }
            }
            else
            {
                IsConnectWSA = false;
            }
        }

        public static ObservableCollection<APKInfo> GetAppsList()
        {
            ObservableCollection<APKInfo> Applications = new ObservableCollection<APKInfo>();
            AdvancedAdbClient client = new AdvancedAdbClient();
            PackageManager manager = new PackageManager(client, WSA);
            foreach (KeyValuePair<string, string> app in manager.Packages)
            {
                if (!string.IsNullOrEmpty(app.Key))
                {
                    ConsoleOutputReceiver receiver = new ConsoleOutputReceiver();
                    client.ExecuteRemoteCommand($"pidof {app.Key}", WSA, receiver);
                    bool isactive = !string.IsNullOrEmpty(receiver.ToString());
                    Applications.Add(new APKInfo()
                    {
                        Name = app.Key,
                        IsActive = isactive,
                        VersionInfo = manager.GetVersionInfo(app.Key)
                    });
                }
            }
            return Applications;
        }

        public static ObservableCollection<StorageInfo> GetStorageInfo()
        {
            StorageReceiver receiver = new StorageReceiver();
            if (WSA != null)
            {
                ADBClient.ExecuteRemoteCommand("df", WSA, receiver);
            }
            return receiver.Storages;
        }

        private static void OnDeviceChanged(object sender, DeviceDataEventArgs e)
        {
            DeviceList = new AdvancedAdbClient().GetDevices();
        }
    }
}
