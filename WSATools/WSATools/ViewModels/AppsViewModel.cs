using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSATools.Core.Helpers;
using WSATools.Core.Models;

namespace WSATools.ViewModels
{
    public sealed class AppsViewModel : ViewModelBase
    {
        ObservableCollection<APKInfo>? _appList;
        public ObservableCollection<APKInfo>? AppList
        {
            get => _appList;
            set => SetProperty(ref _appList, value);
        }

        public override async Task Refresh()
        {
            AppList = ADBHelper.GetAppsList();
        }

        public override void Dispose()
        {

        }
    }
}
