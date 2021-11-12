using System;
using System.Windows;
using WSATools.Core;
using WSATools.ViewModels;

namespace WSATools
{
    public partial class WSAList : Window
    {
        private WSAListViewModel ViewModel;
        public WSAList()
        {
            InitializeComponent();
        }
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            if (DataContext is WSAListViewModel viewModel)
            {
                ViewModel = viewModel;
                ViewModel.Close += ViewModel_Close;
                ViewModel.Loading += ViewModel_Loading;
            }
        }
        private void ViewModel_Loading(object sender, Visibility result)
        {
            switch (result)
            {
                case Visibility.Collapsed:
                    break;
                case Visibility.Visible:
                    break;
            }
        }
        private void ViewModel_Close(object sender, bool? result)
        {
            try
            {
                DialogResult = result;
                ViewModel.Dispose();
                Close();
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogError("WSAList Close", ex);
            }
        }
    }
}