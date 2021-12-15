using ModernWpf.Controls;
using System.Windows.Navigation;
using WSATools.ViewModels;

namespace WSATools.Pages.SettingsPages
{
    /// <summary>
    /// SettingsPage.xaml 的交互逻辑
    /// </summary>
    public partial class SettingsPage : Page
    {
        private SettingsViewModel ViewModel;

        public SettingsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (DataContext is SettingsViewModel viewModel)
            {
                ViewModel = viewModel;
            }
        }
    }
}
