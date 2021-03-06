using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;

namespace WSATools.Core
{
    public sealed class Zipper
    {
        public static bool UnZip(string zipFileName, string targetDirectory)
        {
            try
            {
                FastZip? zip = new FastZip();
                zip.ExtractZip(zipFileName, targetDirectory, "");
                File.Delete(zipFileName);
                return Adb.Instance.HasBrige;
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogError("UnZip", ex);
            }
            return false;
        }
    }
}