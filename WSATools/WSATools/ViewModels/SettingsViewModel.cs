using APKInstaller.Helpers;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using WSATools.Models;

namespace WSATools.ViewModels
{
    public sealed class SettingsViewModel : ViewModelBase
    {
        public IAsyncRelayCommand GotoUpdateCommand { get; }
        public IAsyncRelayCommand CheackUpdateCommand { get; }

        private DateTime _updateDate = new DateTime();
        public DateTime UpdateDate
        {
            get => _updateDate;
            set => SetProperty(ref _updateDate, value);
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
                string ver = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                string name = "WSA Tools";
                return $"{name} v{ver}";
            }
        }

        public SettingsViewModel()
        {
            GotoUpdateCommand = new AsyncRelayCommand(GotoUpdate);
            CheackUpdateCommand = new AsyncRelayCommand(CheckUpdate);
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

        private async Task GotoUpdate()
        {
            _ = Launcher.LaunchUriAsync(new Uri(UpdateInfo.ReleaseUrl));
        }
    }
}
