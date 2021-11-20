using APPXManager.Models;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using WSATools.Core;
using WSATools.Core.Helpers;
using WSATools.Pages;

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
            IsWSAStart = WSAHelper.IsWSARunning;
            (IsWSAInstalled,WSAInfo) = WSAHelper.GetWSAInfo();
        }

        public override void Dispose()
        {

        }
    }
}
