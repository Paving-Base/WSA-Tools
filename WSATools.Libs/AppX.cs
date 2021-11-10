using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WSATools.Libs
{
    public sealed class AppX
    {
        private static readonly string[] array;
        static AppX()
        {
            array = new string[2];
            switch (RuntimeInformation.ProcessArchitecture)
            {
                case Architecture.Arm:
                case Architecture.Arm64:
                    array[0] = "x86";
                    array[1] = "x64";
                    break;
                case Architecture.X64:
                    array[0] = "arm";
                    array[1] = "x86";
                    break;
                case Architecture.X86:
                    array[0] = "arm";
                    array[1] = "x64";
                    break;
            }
        }
        public static async Task<Dictionary<string, string>> GetFilePath()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();
            var url = "https://store.rg-adguard.net/api/GetFiles";
            var content = "type=ProductId&url=9p3395vx91nr&ring=WIS&lang=zh-CN";
            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
            HttpClient httpClient = new HttpClient(handler);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded");
            var respone = await httpClient.PostAsync(url, stringContent);
            if (respone.StatusCode == HttpStatusCode.OK)
            {
                var html = await respone.Content.ReadAsStringAsync();
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);
                var table = doc.DocumentNode.SelectSingleNode("table");
                if (table != null)
                {
                    foreach (var tr in table.SelectNodes("tr"))
                    {
                        var a = tr.SelectSingleNode("td").SelectSingleNode("a");
                        if (a != null)
                        {
                            var key = a.InnerHtml.ToString();
                            if (IsSupport(key))
                            {
                                var value = a.Attributes["href"].Value;
                                list.Add(key, value);
                            }
                        }
                    }
                }
            }
            return list;
        }
        private static bool IsSupport(string key)
        {
            if (key.EndsWith("appx") || key.EndsWith("msixbundle"))
            {
                var count = 0;
                foreach (var type in array)
                {
                    if (key.Contains(type))
                        count++;
                }
                return count == 0;
            }
            return false;
        }
        public static async Task<bool> PepairAsync(Dictionary<string, string> urls, int timeout)
        {
            try
            {
                int count = 0, total = urls.Count;
                foreach (var url in urls)
                {
                    var path = Path.Combine(Environment.CurrentDirectory, url.Key);
                    if (File.Exists(path))
                        count++;
                    else if (await Downloader.Create(url.Value, path, timeout))
                        count++;
                }
                return count == total;
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogError("PepairAsync", ex);
                return false;
            }
        }
    }
}