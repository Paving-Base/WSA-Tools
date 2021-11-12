using System.Windows;

namespace WSATools.Core
{
    public delegate void CloseHandler(object sender, bool? result);
    public delegate void EnableHandler(object sender, bool state);
    public delegate void LoadingHandler(object sender, Visibility result);
}