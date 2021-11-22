using HtmlAgilityPack;
using System.Net;
using System.Text;

namespace APPXManager.DeviceCommands
{
    public static class DownloadManager
    {
        private const string API = "https://store.rg-adguard.net/api/GetFiles";
        private const string content = "type={0}&url={1}&ring={2}&lang=zh-CN";

        public enum UrlType
        {
            url,
            ProductId,
            PackageFamilyName,
            CategoryId
        }

        public enum Ring
        {
            WIF,
            WIS,
            RP,
            Retail,
        }

        public static async Task<Dictionary<string, string>> GetPackageFromStore(UrlType type, string uri, Ring ring)
        {
            Dictionary<string, string> list = new Dictionary<string, string>();
            var handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip
            };
            HttpClient httpClient = new HttpClient(handler);
            var stringContent = new StringContent(string.Format(content, type, uri, ring), Encoding.UTF8, "application/x-www-form-urlencoded");
            var respone = await httpClient.PostAsync(API, stringContent);
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
                            var value = a.Attributes["href"].Value;
                            list.Add(key, value);
                        }
                    }
                }
            }
            return list;
        }
    }
}
