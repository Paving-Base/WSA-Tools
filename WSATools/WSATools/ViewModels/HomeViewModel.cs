using APPXManager.Models;
using WSATools.Core.Helpers;

namespace WSATools.ViewModels
{
    public sealed class HomeViewModel : ViewModelBase
    {
        private PackageInfo _WSAInfo = new PackageInfo();
        public PackageInfo WSAInfo
        {
            get => _WSAInfo;
            set => SetProperty(ref _WSAInfo, value);
        }

        private bool _isWSAStart = WSAHelper.IsWSARunning;
        public bool IsWSAStart
        {
            get => _isWSAStart;
            set => SetProperty(ref _isWSAStart, value);
        }

        private bool _isWSAInstalled = false;
        public bool IsWSAInstalled
        {
            get => _isWSAInstalled;
            set => SetProperty(ref _isWSAInstalled, value);
        }

        public HomeViewModel()
        {
            (IsWSAInstalled, WSAInfo) = WSAHelper.GetWSAInfo();
            ADBHelper.InitilizeADB();
            ADBHelper.ConnectWSA();
            ADBHelper.GetStorageInfo();
        }

        public override void Dispose()
        {

        }
    }
}
