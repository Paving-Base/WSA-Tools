using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSATools.Core.Helpers;
using WSATools.Core.Models;

namespace WSATools.ViewModels
{
    public sealed class AppsViewModel : INotifyPropertyChanged
    {
        ObservableCollection<APKInfo> _appList;
        public ObservableCollection<APKInfo> AppList
        {
            get => _appList;
            set
            {
                if (_appList != value)
                {
                    _appList = value;
                    RaisePropertyChangedEvent();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChangedEvent([System.Runtime.CompilerServices.CallerMemberName] string name = null)
        {
            if (name != null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }
        }

        public void Refresh()
        {
            AppList = ADBHelper.GetAppsList();
        }
    }
}
