using APPXManager.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WSATools.Core.Helpers;
using WSATools.Core.Models;
using WSATools.Helpers;

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

        private double _storageUsage;
        public double StorageUsage
        {
            get => _storageUsage;
            set => SetProperty(ref _storageUsage, value);
        }

        private string _storageUsageText;
        public string StorageUsageText
        {
            get => _storageUsageText;
            set => SetProperty(ref _storageUsageText, value);
        }

        public HomeViewModel()
        {
            (IsWSAInstalled, WSAInfo) = WSAHelper.GetWSAInfo();
            ADBHelper.InitilizeADB();
            ADBHelper.ConnectWSA();
        }

        public override async Task Refresh()
        {
            ObservableCollection<StorageInfo>? storages = ADBHelper.GetStorageInfo();
            int Size = 0, Used = 0, Available = 0;
            foreach (StorageInfo? storage in storages)
            {
                Size += storage.Size;
                Used += storage.Used;
                Available += storage.Available;
            }
            StorageUsage = (double)Used / Size;
            StorageUsageText = $"一共 {((double)Size * 1024).GetSizeString()} 空余 {((double)Available * 1024).GetSizeString()}";
        }

        public override void Dispose()
        {

        }
    }
}
