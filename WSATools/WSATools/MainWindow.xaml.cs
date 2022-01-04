using MicaWPF.Controls;
using System.Windows;
using WSATools.Helpers;
using WSATools.Pages;

namespace WSATools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MicaWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            UIHelper.MainWindow = this;
            MainPage MainPage = new();
            Content = MainPage;
        }
    }
}
