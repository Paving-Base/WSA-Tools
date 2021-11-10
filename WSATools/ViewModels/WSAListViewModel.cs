using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WSATools.Libs;

namespace WSATools.ViewModels
{
    public sealed class WSAListViewModel : ViewModelBase
    {
        public event CloseHandler Close;
        public IAsyncRelayCommand CloseCommand { get; }
        public IAsyncRelayCommand RreshCommand { get; }
        public IAsyncRelayCommand InstallCommand { get; }
        public WSAListViewModel()
        {
            CloseCommand = new AsyncRelayCommand(CloseAsync);
            RreshCommand = new AsyncRelayCommand(RreshAsync);
            InstallCommand = new AsyncRelayCommand(InstallAsync);
        }
        private ObservableCollection<ListItem> packages = new ObservableCollection<ListItem>();
        public ObservableCollection<ListItem> Packages
        {
            get => packages;
            set => SetProperty(ref packages, value);
        }
        private bool timeoutEnable = true;
        public bool TimeoutEnable
        {
            get => timeoutEnable;
            set => SetProperty(ref timeoutEnable, value);
        }
        private decimal processVal = 0;
        public decimal ProcessVal
        {
            get => processVal;
            set => SetProperty(ref processVal, value);
        }
        private string timeout = "30";
        public string Timeout
        {
            get => timeout;
            set => SetProperty(ref timeout, value);
        }
        private bool installEnable = true;
        public bool InstallEnable
        {
            get => installEnable;
            set => SetProperty(ref installEnable, value);
        }
        private Task RreshAsync()
        {
            RunOnUIThread(async () =>
            {
                LoadVisable = Visibility.Visible;
                await GetList();
                LoadVisable = Visibility.Collapsed;
            });
            return Task.CompletedTask;
        }
        private Task CloseAsync()
        {
            Close?.Invoke(this, false);
            return Task.CompletedTask;
        }
        private Task InstallAsync()
        {
            RunOnUIThread(async () =>
            {
                InstallEnable = false;
                LoadVisable = Visibility.Visible;
                try
                {
                    TimeoutEnable = false;
                    Dictionary<string, string> urls = new Dictionary<string, string>();
                    foreach (var package in Packages)
                        urls.Add(package.Content, package.Tag.ToString());
                    var timeout = int.Parse(Timeout);
                    if (await AppX.PepairAsync(urls, timeout))
                    {
                        StringBuilder shellBuilder = new StringBuilder();
                        foreach (var url in urls)
                        {
                            var path = Path.Combine(Environment.CurrentDirectory, url.Key);
                            shellBuilder.AppendLine($"Add-AppxPackage {path} -ForceApplicationShutdown");
                        }
                        ExcuteCommand(shellBuilder);
                        Close?.Invoke(this, true);
                    }
                    else
                    {
                        Close?.Invoke(this, false);
                        MessageBox.Show("获取WSA环境包到本地失败，请重试！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    InstallEnable = true;
                    TimeoutEnable = true;
                }
                catch (Exception ex)
                {
                    LogManager.Instance.LogError("InstallAsync", ex);
                    MessageBox.Show("出现异常，安装失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                LoadVisable = Visibility.Collapsed;
            });
            return Task.CompletedTask;
        }
        private void ExcuteCommand(StringBuilder shellBuilder)
        {
            try
            {
                Command.Instance.Shell("Set-ExecutionPolicy RemoteSigned", out _);
                Command.Instance.Shell("Set-ExecutionPolicy -ExecutionPolicy Unrestricted", out _);
                var file = "install.ps1";
                if (File.Exists(file))
                    File.Delete(file);
                File.WriteAllText(file, shellBuilder.ToString());
                var shellFile = Path.Combine(Environment.CurrentDirectory, file);
                Command.Instance.Shell(shellFile, out string message);
                LogManager.Instance.LogInfo("Install WSA Script:" + message);
                File.Delete(shellFile);
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogError("ExcuteCommand", ex);
            }
        }
        public async void LoadAsync(object sender, EventArgs e)
        {
            Dispatcher = (sender as WSAList).Dispatcher;
            Downloader.ProcessChange += Downloader_ProcessChange;
            await GetList();
        }
        private void Downloader_ProcessChange(int receiveSize, long totalSize)
        {
            ProcessVal = Math.Round((decimal)receiveSize / totalSize * 100, 2);
        }
        private async Task GetList()
        {
            LoadVisable = Visibility.Visible;
            try
            {
                if (Packages == null || Packages.Count == 0)
                {
                    var pairs = await AppX.GetFilePath();
                    if (pairs != null && pairs.Count > 0)
                    {
                        foreach (var pair in pairs)
                        {
                            var item = new ListItem(pair.Key, pair.Value);
                            Dispatcher.Invoke(() =>
                            {
                                Packages.Add(item);
                            });
                        }
                    }
                    else
                    {
                        MessageBox.Show("获取WSA环境包失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogError("GetList", ex);
                MessageBox.Show("出现异常，获取依赖包失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            LoadVisable = Visibility.Collapsed;
        }
        public override void Dispose()
        {
            Downloader.ProcessChange -= Downloader_ProcessChange;
        }
    }
}