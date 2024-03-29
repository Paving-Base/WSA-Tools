﻿using ModernWpf.Controls;
using System.Security.Principal;
using System.Windows;
using System.Windows.Navigation;
using WSATools.ViewModels;

namespace WSATools.Pages
{
    /// <summary>
    /// HomePage.xaml 的交互逻辑
    /// </summary>
    public partial class HomePage : Page
    {
        private HomeViewModel Provider;

        public HomePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (DataContext is HomeViewModel ViewModel)
            {
                Provider = ViewModel;
            }
            //获得当前登录的Windows用户标示
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            InfoBar.Visibility = principal.IsInRole(WindowsBuiltInRole.Administrator) ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
