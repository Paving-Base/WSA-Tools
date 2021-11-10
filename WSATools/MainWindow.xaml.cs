using SourceChord.FluentWPF;
using System.Windows;
using WSATools.Pages;

namespace WSATools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainPage MainPage = new();
            Content = MainPage;
        }
    }
}
