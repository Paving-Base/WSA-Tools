using APKInstaller.Helpers;
using CommunityToolkit.Mvvm.Input;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using Windows.System;
using WSATools.Helpers;
using WSATools.Models;
using WSATools.Pages.SettingsPages;
using WSATools.Properties;

namespace WSATools.ViewModels
{
    public sealed class SettingsViewModel : INotifyPropertyChanged
    {
        public IRelayCommand GotoUpdateCommand { get; }
        public IRelayCommand GotoTestPageCommand { get; }
        public IAsyncRelayCommand CheackUpdateCommand { get; }

        private DateTime _updateDate = Settings.Default.UpdateDate;
        public DateTime UpdateDate
        {
            get => _updateDate;
            set
            {
                if (_updateDate != value)
                {
                    Settings.Default.UpdateDate = value;
                    Settings.Default.Save();
                    _updateDate = Settings.Default.UpdateDate;
                    RaisePropertyChangedEvent();
                }
            }
        }

        private UpdateInfo _updateInfo = new UpdateInfo();
        public UpdateInfo UpdateInfo
        {
            get => _updateInfo;
            set
            {
                if (_updateInfo != value)
                {
                    _updateInfo = value;
                    RaisePropertyChangedEvent();
                }
            }
        }

        private string _updateErrorMessage = string.Empty;
        public string UpdateErrorMessage
        {
            get => _updateErrorMessage;
            set
            {
                if (_updateErrorMessage != value)
                {
                    _updateErrorMessage = value;
                    RaisePropertyChangedEvent();
                }
            }
        }

        private bool _checkingUpdate = false;
        public bool CheckingUpdate
        {
            get => _checkingUpdate;
            set
            {
                if (_checkingUpdate != value)
                {
                    _checkingUpdate = value;
                    RaisePropertyChangedEvent();
                }
            }
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

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChangedEvent([System.Runtime.CompilerServices.CallerMemberName] string name = null)
        {
            if (name != null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }
        }

        public SettingsViewModel()
        {
            GotoUpdateCommand = new RelayCommand(GotoUpdate);
            GotoTestPageCommand = new RelayCommand(GotoTestPage);
            CheackUpdateCommand = new AsyncRelayCommand(CheckUpdate);
            if (UpdateDate == DateTime.MinValue) { _ = CheckUpdate(); }
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

        private void GotoTestPage() => UIHelper.Navigate(typeof(TestPage));

        private void GotoUpdate() => _ = Launcher.LaunchUriAsync(new Uri(UpdateInfo.ReleaseUrl));
    }
}
