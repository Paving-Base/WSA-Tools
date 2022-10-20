using ModernWpf;
using ModernWpf.Controls;
using ModernWpf.Media.Animation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;
using WSATools.Controls;
using WSATools.Helpers;
using WSATools.Pages.SettingsPages;

namespace WSATools.Pages
{
    /// <summary>
    /// MainPage.xaml 的交互逻辑
    /// </summary>
    public partial class MainPage : Page
    {
        public PageHeader PageHeader => NavigationView.FindDescendant<PageHeader>();

        private readonly List<(string Tag, Type Page)> _pages = new()
        {
            ("Home", typeof(HomePage)),
            ("Apps", typeof(AppsPage)),
        };

        public MainPage()
        {
            InitializeComponent();
            UIHelper.MainPage = this;
            NavigationView.PaneDisplayMode = NavigationViewPaneDisplayMode.Left;
        }

        private void NavigationView_Loaded(object sender, RoutedEventArgs e)
        {
            // Add handler for ContentFrame navigation.
            NavigationViewFrame.Navigated += On_Navigated;
            NavigationView.SelectedItem = NavigationView.MenuItems[0];
            NavigationView.PaneDisplayMode = NavigationViewPaneDisplayMode.Auto;
        }

        private void NavigationView_Navigate(string NavItemTag, NavigationTransitionInfo TransitionInfo)
        {
            Type _page = null;
            if (NavItemTag == "settings")
            {
                _page = typeof(SettingsPage);
            }
            else
            {
                (string Tag, Type Page) item = _pages.FirstOrDefault(p => p.Tag.Equals(NavItemTag, StringComparison.Ordinal));
                _page = item.Page;
            }
            // Get the page type before navigation so you can prevent duplicate
            // entries in the backstack.
            Type PreNavPageType = NavigationViewFrame.CurrentSourcePageType;

            // Only navigate if the selected page isn't currently loaded.
            if (!(_page is null) && !Equals(PreNavPageType, _page))
            {
                _ = NavigationViewFrame.Navigate(_page, null, TransitionInfo);
            }
        }

        private void NavigationView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args) => _ = TryGoBack();

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                _ = NavigationViewFrame.Navigate(typeof(SettingsPage), null, args.RecommendedNavigationTransitionInfo);
            }
            else if (args.SelectedItemContainer != null)
            {
                string NavItemTag = args.SelectedItemContainer.Tag.ToString();
                NavigationView_Navigate(NavItemTag, args.RecommendedNavigationTransitionInfo);
            }
        }

        private bool TryGoBack()
        {
            if (!NavigationViewFrame.CanGoBack)
            { return false; }

            // Don't go back if the nav pane is overlayed.
            if (NavigationView.IsPaneOpen &&
                (NavigationView.DisplayMode == NavigationViewDisplayMode.Compact ||
                 NavigationView.DisplayMode == NavigationViewDisplayMode.Minimal))
            { return false; }

            NavigationViewFrame.GoBack();
            return true;
        }

        private void On_Navigated(object _, NavigationEventArgs e)
        {
            NavigationView.IsBackEnabled = NavigationViewFrame.CanGoBack;
            if (NavigationViewFrame.SourcePageType == typeof(SettingsPage))
            {
                // SettingsItem is not part of NavView.MenuItems, and doesn't have a Tag.
                NavigationView.SelectedItem = (NavigationViewItem)NavigationView.SettingsItem;
                NavigationView.Header = "设置";
            }
            else if (NavigationViewFrame.SourcePageType == typeof(TestPage))
            {
                NavigationView.Header = "测试";
            }
            else if (NavigationViewFrame.SourcePageType != null)
            {
                (string Tag, Type Page) item = _pages.FirstOrDefault(p => p.Page == e.Content.GetType());

                try
                {
                    NavigationView.SelectedItem = NavigationView.MenuItems
                        .OfType<NavigationViewItem>()
                        .First(n => n.Tag.Equals(item.Tag));
                }
                catch
                {
                    try
                    {
                        NavigationView.SelectedItem = NavigationView.FooterMenuItems
                            .OfType<NavigationViewItem>()
                            .First(n => n.Tag.Equals(item.Tag));
                    }
                    catch { }
                }

                NavigationView.Header = (((NavigationViewItem)NavigationView.SelectedItem)?.Content?.ToString());
            }
        }

        private void NavigationViewControl_PaneClosing(NavigationView sender, NavigationViewPaneClosingEventArgs args)
        {
            UpdateAppTitleMargin(sender);
        }

        private void NavigationViewControl_PaneOpening(NavigationView sender, object args)
        {
            UpdateAppTitleMargin(sender);
        }

        private void NavigationViewControl_DisplayModeChanged(NavigationView sender, NavigationViewDisplayModeChangedEventArgs args)
        {
            Thickness currMargin = AppTitleBar.Margin;

            AppTitleBar.Margin = sender.DisplayMode == NavigationViewDisplayMode.Minimal &&
                         sender.IsBackButtonVisible != NavigationViewBackButtonVisible.Collapsed
                ? new Thickness((sender.CompactPaneLength * 2) - 8, currMargin.Top, currMargin.Right, currMargin.Bottom)
                : new Thickness(sender.CompactPaneLength, currMargin.Top, currMargin.Right, currMargin.Bottom);

            UpdateAppTitleMargin(sender);
            UpdateHeaderMargin(sender);
        }

        private void UpdateAppTitleMargin(NavigationView sender)
        {
            const int smallLeftIndent = 4, largeLeftIndent = 24;

            Thickness currMargin = AppTitle.Margin;

            AppTitle.Margin = (sender.DisplayMode == NavigationViewDisplayMode.Expanded && sender.IsPaneOpen) ||
                     (sender.DisplayMode == NavigationViewDisplayMode.Compact && sender.IsPaneOpen) ||
                     sender.DisplayMode == NavigationViewDisplayMode.Minimal
                ? new Thickness(smallLeftIndent, currMargin.Top, currMargin.Right, currMargin.Bottom)
                : new Thickness(largeLeftIndent, currMargin.Top, currMargin.Right, currMargin.Bottom);
        }

        private void UpdateHeaderMargin(NavigationView sender)
        {
            if (PageHeader != null)
            {
                PageHeader.HeaderPadding = sender.DisplayMode == NavigationViewDisplayMode.Minimal
                    ? (Thickness)Application.Current.Resources["PageHeaderMinimalPadding"]
                    : (Thickness)Application.Current.Resources["PageHeaderDefaultPadding"];

                Thickness currMargin = PageHeader.Margin;

                PageHeader.Margin = sender.DisplayMode == NavigationViewDisplayMode.Minimal &&
                         sender.IsBackButtonVisible != NavigationViewBackButtonVisible.Collapsed
                    ? new Thickness(-sender.CompactPaneLength + 8, currMargin.Top, currMargin.Right, currMargin.Bottom)
                    : new Thickness(0, currMargin.Top, currMargin.Right, currMargin.Bottom);
            }
        }

        #region 状态栏
        public enum MessageColor
        {
            Red,
            Blue,
            Green,
            Yellow,
        }

        public void ShowProgressRing()
        {
            if (PageHeader != null)
            {
                PageHeader.ProgressRing.Visibility = Visibility.Visible;
                PageHeader.ProgressRing.IsActive = true;
            }
        }

        public void HideProgressRing()
        {
            if (PageHeader != null)
            {
                PageHeader.ProgressRing.IsActive = false;
                PageHeader.ProgressRing.Visibility = Visibility.Collapsed;
            }
        }

        public void ShowProgressBar()
        {
            ProgressBar.Visibility = Visibility.Visible;
            ProgressBar.IsIndeterminate = true;
            ProgressBar.ShowError = false;
            ProgressBar.ShowPaused = false;
        }

        public void PausedProgressBar()
        {
            ProgressBar.Visibility = Visibility.Visible;
            ProgressBar.IsIndeterminate = true;
            ProgressBar.ShowError = false;
            ProgressBar.ShowPaused = true;
        }

        public void ErrorProgressBar()
        {
            ProgressBar.Visibility = Visibility.Visible;
            ProgressBar.IsIndeterminate = true;
            ProgressBar.ShowPaused = false;
            ProgressBar.ShowError = true;
        }

        public void HideProgressBar()
        {
            ProgressBar.Visibility = Visibility.Collapsed;
            ProgressBar.IsIndeterminate = false;
            ProgressBar.ShowError = false;
            ProgressBar.ShowPaused = false;
        }

        public void ShowMessage(string message, string info, MessageColor color)
        {
            if (PageHeader != null)
            {
                PageHeader.Message.Text = message;
                PageHeader.MessageInfo.Glyph = info;
                PageHeader.MessageInfo.Foreground = color switch
                {
                    MessageColor.Red => new SolidColorBrush(Color.FromArgb(255, 245, 88, 98)),
                    MessageColor.Blue => new SolidColorBrush(Color.FromArgb(255, 119, 220, 255)),
                    MessageColor.Green => new SolidColorBrush(Color.FromArgb(255, 155, 230, 155)),
                    MessageColor.Yellow => new SolidColorBrush(Color.FromArgb(255, 254, 228, 160)),
                    _ => new SolidColorBrush(Colors.Yellow),
                };
                PageHeader?.RectanglePointerEntered();
            }
        }

        public void HideMessage()
        {
            PageHeader?.RectanglePointerExited();
        }

        #endregion
    }
}
