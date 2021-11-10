using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using MessageBox = HandyControl.Controls.MessageBox;

namespace WSATools.Libs
{
    public sealed class WSA
    {
        public static IEnumerable<string> PackageList { get; }
        static WSA()
        {
            PackageList = new[] { "Microsoft-Hyper-V", "HypervisorPlatform", "VirtualMachinePlatform" };
        }
        public static void Start()
        {
            var cmd = @"explorer.exe shell:appsFolder\MicrosoftCorporationII.WindowsSubsystemForAndroid_8wekyb3d8bbwe!App";
            Command.Instance.Excute(cmd, out _);
        }
        public static (bool VM, bool WSA, bool Run) State()
        {
            var count = 0;
            foreach (var package in PackageList)
            {
                if (Check(package))
                    count++;
            }
            return (count == 3, Pepair(), Running);
        }
        public static int Init()
        {
            int count = 0;
            foreach (var package in PackageList)
            {
                if (!Check(package))
                    Install(package);
                else
                    count++;
            }
            if (count < 3)
            {
                if (MessageBox.Show("需要重启系统安装对应组件后进行安装！(确定后5s内重启系统，请保存好你的数据后进行重启！！！)", "提示",
                    MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                    Command.Instance.Excute("shutdown -r -t 5", out _);
                return 0;
            }
            return 1;
        }
        public static bool Running
        {
            get
            {
                var ps = Process.GetProcessesByName("vmmemWSA");
                return ps != null && ps.Length > 0;
            }
        }
        private static void Install(string packageName)
        {
            Command.Instance.Excute($"DISM /Online /Enable-Feature /All /FeatureName:{packageName} /NoRestart", out string message);
            LogManager.Instance.LogInfo("Install WSA:" + message);
        }
        private static bool Check(string packageName)
        {
            Command.Instance.Excute($"DISM /Online /Get-FeatureInfo:{packageName}", out string message);
            LogManager.Instance.LogInfo("Check VM:" + message);
            return message.Before("状态", "已启用");
        }
        public static bool Pepair()
        {
            Command.Instance.Shell("Get-AppxPackage|findstr WindowsSubsystemForAndroid", out string message);
            LogManager.Instance.LogInfo("Pepair WSA:" + message);
            return !string.IsNullOrEmpty(message);
        }
        public static void Clear()
        {
            Command.Instance.Shell("Get-AppxPackage|findstr WindowsSubsystemForAndroid", out string message);
            var packageName = message.Split("\r\n").ElementAt(1).Split(":").LastOrDefault().Trim();
            Command.Instance.Shell($"Remove-AppxPackage {packageName}", out string packageMessage);
            LogManager.Instance.LogInfo("Clear WSA:" + packageMessage);
            foreach (var package in PackageList)
            {
                Command.Instance.Excute($"DISM /Online /Disable-Feature /All /FeatureName:{package} /NoRestart", out string resultMessage);
                LogManager.Instance.LogInfo("Clear VM WSA:" + resultMessage);
            }
        }
    }
}