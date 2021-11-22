using ModernWpf.Media.Animation;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Threading.Tasks;
using WSATools.Pages;

namespace WSATools.Helpers
{
    internal static class UIHelper
    {
        public const string Error = "";
        public const string Seccess = "";
        public const string Message = "";
        public const string Warnning = "";
        public const string AppTitle = "Universal-like Minecraft Launcher";

        public static float DpiX, DpiY;
        public static MainPage MainPage;
        public static MainWindow MainWindow;
        public static bool IsShowingProgressRing, IsShowingProgressBar, IsShowingMessage;
        private static readonly ObservableCollection<(string message, string info, MainPage.MessageColor color)> MessageList = new();

        public enum NavigationThemeTransition
        {
            Default,
            Entrance,
            DrillIn,
            Suppress
        }

        static UIHelper()
        {
            Graphics graphics = Graphics.FromHwnd(IntPtr.Zero);
            DpiX = graphics.DpiX / 96;
            DpiY = graphics.DpiY / 96;
        }

        public static void Navigate(Type pageType, object e = null, NavigationThemeTransition Type = NavigationThemeTransition.Default)
        {
            _ = Type switch
            {
                NavigationThemeTransition.DrillIn => MainPage?.NavigationViewFrame.Navigate(pageType, e, new DrillInNavigationTransitionInfo()),
                NavigationThemeTransition.Entrance => MainPage?.NavigationViewFrame.Navigate(pageType, e, new EntranceNavigationTransitionInfo()),
                NavigationThemeTransition.Suppress => MainPage?.NavigationViewFrame.Navigate(pageType, e, new SuppressNavigationTransitionInfo()),
                NavigationThemeTransition.Default => MainPage?.NavigationViewFrame.Navigate(pageType, e),
                _ => MainPage?.NavigationViewFrame.Navigate(pageType, e),
            };
        }

        /// <summary>
        /// 显示进度环
        /// </summary>
        public static void ShowProgressRing()
        {
            IsShowingProgressRing = true;
            MainPage.ShowProgressRing();
        }

        /// <summary>
        /// 隐藏进度环
        /// </summary>
        public static void HideProgressRing()
        {
            IsShowingProgressRing = false;
            MainPage.HideProgressRing();
        }

        /// <summary>
        /// 显示进度条-正常
        /// </summary>
        public static void ShowProgressBar()
        {
            IsShowingProgressBar = true;
            MainPage.ShowProgressBar();
        }

        /// <summary>
        /// 显示进度条-暂停
        /// </summary>
        public static void PausedProgressBar()
        {
            IsShowingProgressBar = true;
            MainPage.PausedProgressBar();
        }

        /// <summary>
        /// 显示进度条-错误
        /// </summary>
        public static void ErrorProgressBar()
        {
            IsShowingProgressBar = true;
            MainPage.ErrorProgressBar();
        }

        /// <summary>
        /// 隐藏进度条
        /// </summary>
        public static void HideProgressBar()
        {
            IsShowingProgressBar = false;
            MainPage.HideProgressBar();
        }

        /// <summary>
        /// 展示应用内通知
        /// </summary>
        /// <param name="message">要展示的消息</param>
        /// <param name="info">消息前的图标</param>
        /// <param name="color">消息前图标的颜色</param>
        public static async void ShowMessage(string message, string info = Message, MainPage.MessageColor color = MainPage.MessageColor.Blue)
        {
            MessageList.Add((message, info, color));
            if (!IsShowingMessage)
            {
                IsShowingMessage = true;
                while (MessageList.Count > 0)
                {
                    if (!string.IsNullOrEmpty(MessageList[0].message))
                    {
                        string messages = $"{MessageList[0].message.Replace("\n", " ")}";
                        MainPage.ShowMessage(messages, MessageList[0].info, MessageList[0].color);
                        await Task.Delay(3000);
                    }
                    MessageList.RemoveAt(0);
                    if (MessageList.Count == 0)
                    {
                        MainPage.HideMessage();
                    }
                }
                IsShowingMessage = false;
            }
        }

        /// <summary>
        /// 获取时间内的问候语
        /// </summary>
        /// <returns>问候语</returns>
        public static string GetGreetings()
        {
            string str = "";
            DateTime now = DateTime.Now;
            int times = now.Hour;
            if (times is >= 0 and < 6) { str = "熬夜对身体不好哦"; }
            if (times is >= 6 and < 9) { str = "早安"; }
            if (times is >= 9 and < 11) { str = "上午好"; }
            if (times is >= 11 and < 13) { str = "中午好"; }
            if (times is >= 13 and < 17) { str = "下午好"; }
            if (times is >= 17 and < 19) { str = "吃过晚饭了吗"; }
            if (times is >= 19 and < 24) { str = "晚安"; }
            return str;
        }

        /// <summary>
        /// 将字节数转换为可读数据
        /// </summary>
        /// <param name="size">字节数</param>
        /// <returns>xx.xx xB</returns>
        public static string GetSizeString(this double size)
        {
            int index = 0;
            while (true)
            {
                index++;
                size /= 1024;
                if (size is > 0.7 and < 716.8) { break; }
                else if (size >= 716.8) { continue; }
                else if (size <= 0.7)
                {
                    size *= 1024;
                    index--;
                    break;
                }
            }
            string str = string.Empty;
            switch (index)
            {
                case 0: str = "B"; break;
                case 1: str = "KB"; break;
                case 2: str = "MB"; break;
                case 3: str = "GB"; break;
                case 4: str = "TB"; break;
                default:
                    break;
            }
            return $"{size:N2}{str}";
        }
    }
}
