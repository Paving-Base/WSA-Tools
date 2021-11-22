using APKInstaller.Helpers;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Windows.System;
using WSATools.Helpers;
using WSATools.Models;
using WSATools.Pages.SettingsPages;
using WSATools.Properties;

namespace WSATools.ViewModels
{
    public sealed class SettingsViewModel : ViewModelBase
    {
        public IAsyncRelayCommand GotoUpdateCommand { get; }
        public IAsyncRelayCommand CheackUpdateCommand { get; }
        public IAsyncRelayCommand GotoTestPageCommand { get; }

        private DateTime _updateDate = Settings.Default.UpdateDate;
        public DateTime UpdateDate
        {
            get => _updateDate;
            set
            {
                Settings.Default.UpdateDate = value;
                Settings.Default.Save();
                value = Settings.Default.UpdateDate;
                SetProperty(ref _updateDate, value);
            }
        }

        private UpdateInfo _updateInfo = new UpdateInfo();
        public UpdateInfo UpdateInfo
        {
            get => _updateInfo;
            set => SetProperty(ref _updateInfo, value);
        }

        private string _updateErrorMessage = string.Empty;
        public string UpdateErrorMessage
        {
            get => _updateErrorMessage;
            set => SetProperty(ref _updateErrorMessage, value);
        }

        private bool _checkingUpdate = false;
        public bool CheckingUpdate
        {
            get => _checkingUpdate;
            set => SetProperty(ref _checkingUpdate, value);
        }

        public string VersionText
        {
            get
            {
                string ver = $"{Assembly.GetExecutingAssembly().GetName().Version.Build}.{Assembly.GetExecutingAssembly().GetName().Version.Major}.{Assembly.GetExecutingAssembly().GetName().Version.Minor}";
                string name = "WSA Tools";
                return $"{name} v{ver}";
            }
        }

        public SettingsViewModel()
        {
            GotoUpdateCommand = new AsyncRelayCommand(GotoUpdate);
            CheackUpdateCommand = new AsyncRelayCommand(CheckUpdate);
            GotoTestPageCommand = new AsyncRelayCommand(GotoTestPage);
            if (UpdateDate == DateTime.MinValue) { _ = CheckUpdate(); }
        }

        public override void Dispose()
        {

        }

        private async Task CheckUpdate()
        {
            CheckingUpdate = true;
            try
            {
                UpdateInfo = await UpdateHelper.CheckUpdateAsync("Paving-Base", "WSA-Tools");
            }
            catch (Exception ex)
            {
                UpdateErrorMessage = ex.Message;
                UpdateInfo = new UpdateInfo()
                {
                    UpdateState = UpdateState.CheckFailed
                };
            }
            UpdateDate = DateTime.Now;
            CheckingUpdate = false;
        }

        private async Task GotoTestPage() => UIHelper.Navigate(typeof(TestPage));

        private async Task GotoUpdate() => _ = Launcher.LaunchUriAsync(new Uri(UpdateInfo.ReleaseUrl));
    }
}
