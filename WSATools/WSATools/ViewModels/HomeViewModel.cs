using APPXManager.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using WSATools.Core.Helpers;
using WSATools.Core.Models;
using WSATools.Helpers;

namespace WSATools.ViewModels
{
    public sealed class HomeViewModel : INotifyPropertyChanged
    {
        private PackageInfo _WSAInfo = new PackageInfo();
        public PackageInfo WSAInfo
        {
            get => _WSAInfo;
            set
            {
                if (_WSAInfo != value)
                {
                    _WSAInfo = value;
                    RaisePropertyChangedEvent();
                }
            }
        }

        private bool _isWSAStart = WSAHelper.IsWSARunning;
        public bool IsWSAStart
        {
            get => _isWSAStart;
            set
            {
                if (_isWSAStart != value)
                {
                    _isWSAStart = value;
                    RaisePropertyChangedEvent();
                }
            }
        }

        private bool _isWSAInstalled = false;
        public bool IsWSAInstalled
        {
            get => _isWSAInstalled;
            set
            {
                if (_isWSAInstalled != value)
                {
                    _isWSAInstalled = value;
                    RaisePropertyChangedEvent();
                }
            }
        }

        private double _storageUsage;
        public double StorageUsage
        {
            get => _storageUsage;
            set
            {
                if (_storageUsage != value)
                {
                    _storageUsage = value;
                    RaisePropertyChangedEvent();
                }
            }
        }

        private string _storageUsageText;
        public string StorageUsageText
        {
            get => _storageUsageText;
            set
            {
                if (_storageUsageText != value)
                {
                    _storageUsageText = value;
                    RaisePropertyChangedEvent();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChangedEvent([System.Runtime.CompilerServices.CallerMemberName] string name = null)
        {
            if (name != null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }
        }

        public HomeViewModel()
        {
            //(IsWSAInstalled, WSAInfo) = WSAHelper.GetWSAInfo();
            //ADBHelper.InitilizeADB();
            //ADBHelper.ConnectWSA();
        }

        public void Refresh()
        {
            ObservableCollection<StorageInfo> storages = ADBHelper.GetStorageInfo();
            int Size = 0, Used = 0, Available = 0;
            foreach (StorageInfo storage in storages)
            {
                Size += storage.Size;
                Used += storage.Used;
                Available += storage.Available;
            }
            StorageUsage = (double)Used / Size;
            StorageUsageText = $"一共 {((double)Size * 1024).GetSizeString()} 空余 {((double)Available * 1024).GetSizeString()}";
        }
    }
}
