using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows;
using MessageBox = HandyControl.Controls.MessageBox;

namespace WSATools.Libs
{
    public sealed class Downloader
    {
        public static event ProgressHandler ProcessChange;
        public delegate void ProgressHandler(int receiveSize, long totalSize);
        private static readonly List<string> array = new List<string>();
        public static async Task<bool> Create(string url, string path, int timeout = 30)
        {
            try
            {
                DateTime startTime = DateTime.UtcNow;
                WebRequest request = WebRequest.Create(url);
                using (WebResponse response = request.GetResponse())
                {
                    if (File.Exists(path) && MessageBoxResult.Yes == MessageBox.Show($"已存在文件{Path.GetFileName(path)},是否重新下载？",
                        "提示", MessageBoxButton.YesNo, MessageBoxImage.Question))
                        File.Delete(path);
                    using Stream responseStream = response.GetResponseStream();
                    using Stream fileStream = new FileStream(path, FileMode.CreateNew);
                    byte[] buffer = new byte[20480];
                    int sumSchedule = 0;
                    int bytesRead = await responseStream.ReadAsync(buffer, 0, 20480);
                    while (bytesRead > 0)
                    {
                        fileStream.Write(buffer, 0, bytesRead);
                        DateTime nowTime = DateTime.UtcNow;
                        if ((nowTime - startTime).TotalMinutes > timeout)
                            return false;
                        bytesRead = await responseStream.ReadAsync(buffer, 0, 20480);
                        sumSchedule += 20480;
                        ProcessChange?.Invoke(sumSchedule, response.ContentLength);
                    }
                    array.Add(path);
                }
                return true;
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogError($"Create {url} download", ex);
                return false;
            }
        }
        public static void Clear()
        {
            foreach (var path in array)
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
        }
    }
}