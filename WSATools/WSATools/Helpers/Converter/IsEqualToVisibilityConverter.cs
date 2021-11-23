using System.Windows;

namespace WSATools.Helpers.Converter
{
    public partial class IsEqualToVisibilityConverter : IsEqualToObjectConverter
    {
        public IsEqualToVisibilityConverter()
        {
            TrueValue = Visibility.Visible;
            FalseValue = Visibility.Collapsed;
        }
    }
}
