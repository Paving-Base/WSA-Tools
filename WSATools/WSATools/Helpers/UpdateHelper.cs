using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using WSATools.Models;

namespace APKInstaller.Helpers
{
    public static class UpdateHelper
    {
        private const string KKPP_API = "https://v2.kkpp.cc/repos/{0}/{1}/releases/latest";
        private const string GITHUB_API = "https://api.github.com/repos/{0}/{1}/releases/latest";

        public static async Task<UpdateInfo> CheckUpdateAsync(string username, string repository, Version currentVersion = null)
        {
            UpdateInfo Info = new UpdateInfo()
            {
                UpdateState = UpdateState.CheckingUpdate
            };

            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            if (string.IsNullOrEmpty(repository))
            {
                throw new ArgumentNullException(nameof(repository));
            }

            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", username);
            string url = string.Format(GITHUB_API, username, repository);
            HttpResponseMessage response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                url = string.Format(KKPP_API, username, repository);
                response = await client.GetAsync(url);
            }
            string responseBody = await response.Content.ReadAsStringAsync();
            UpdateInfo result = JsonSerializer.Deserialize<UpdateInfo>(responseBody);

            if (result != null)
            {
                if (currentVersion == null)
                {
                    currentVersion = Assembly.GetEntryAssembly().GetName().Version;
                }

                SystemVersionInfo newVersionInfo = GetAsVersionInfo(result.TagName);
                int major = currentVersion.Major == -1 ? 0 : currentVersion.Major;
                int minor = currentVersion.Minor == -1 ? 0 : currentVersion.Minor;
                int build = currentVersion.Build == -1 ? 0 : currentVersion.Build;
                int revision = currentVersion.Revision == -1 ? 0 : currentVersion.Revision;

                SystemVersionInfo currentVersionInfo = new SystemVersionInfo(major, minor, build, revision);

                Info.Changelog = result?.Changelog;
                Info.CreatedAt = Convert.ToDateTime(result?.CreatedAt);
                Info.Assets = result.Assets;
                Info.IsPreRelease = result.IsPreRelease;
                Info.PublishedAt = Convert.ToDateTime(result?.PublishedAt);
                Info.TagName = result.TagName;
                Info.ApiUrl = result?.ApiUrl;
                Info.ReleaseUrl = result?.ReleaseUrl;
                Info.IsExistNewVersion = newVersionInfo > currentVersionInfo;
                Info.UpdateState = Info.IsExistNewVersion ? UpdateState.ReadyToDownload : UpdateState.UpToDate;
            }
            else
            {
                Info.UpdateState = UpdateState.UpToDate;
            }
            return Info;
        }

        private static SystemVersionInfo GetAsVersionInfo(string version)
        {
            System.Collections.Generic.List<int> nums = GetVersionNumbers(version).Split('.').Select(int.Parse).ToList();

            if (nums.Count <= 1)
            {
                return new SystemVersionInfo(nums[0], 0, 0, 0);
            }
            else if (nums.Count <= 2)
            {
                return new SystemVersionInfo(nums[0], nums[1], 0, 0);
            }
            else if (nums.Count <= 3)
            {
                return new SystemVersionInfo(nums[0], nums[1], nums[2], 0);
            }
            else
            {
                return new SystemVersionInfo(nums[0], nums[1], nums[2], nums[3]);
            }
        }

        private static string GetVersionNumbers(string version)
        {
            string allowedChars = "01234567890.";
            return new string(version.Where(c => allowedChars.Contains(c)).ToArray());
        }
    }
}
