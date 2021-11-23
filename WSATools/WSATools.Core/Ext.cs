using System;
using System.Collections.Generic;

namespace WSATools.Core
{
    public static class Ext
    {
        public static bool Before(this string str, string start, string end)
        {
            int startIndex = str.IndexOf(start);
            int endIndex = str.IndexOf(end);
            if (startIndex != -1)
            {
                return startIndex < endIndex;
            }

            return false;
        }
        public static string[] Splits(this string str, string separator)
        {
            List<string> results = new List<string>();
            string[]? array = str.Split(separator);
            if (array != null)
            {
                foreach (string? item in array)
                {
                    string? data = item.Trim();
                    if (!string.IsNullOrEmpty(data))
                    {
                        results.Add(data);
                    }
                }
            }
            return results.ToArray();
        }
        public static string[] Splits(this string str, params char[] separator)
        {
            List<string> results = new List<string>();
            string[]? array = str.Split(separator);
            if (array != null)
            {
                foreach (string? item in array)
                {
                    string? data = item.Trim();
                    if (!string.IsNullOrEmpty(data))
                    {
                        results.Add(data);
                    }
                }
            }
            return results.ToArray();
        }
        public static string Substring(this string str, string startChars, string endChars = null)
        {
            int startIndex = str.IndexOf(startChars, StringComparison.CurrentCultureIgnoreCase);
            int leftPadding = startChars.Length, endIndex = str.Length;
            if (!string.IsNullOrEmpty(endChars))
            {
                endIndex = str.IndexOf(endChars, StringComparison.CurrentCultureIgnoreCase);
            }

            return str.Substring(startIndex + leftPadding, endIndex - startIndex - leftPadding);
        }
    }
}