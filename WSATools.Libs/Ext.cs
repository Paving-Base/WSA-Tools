using System;
using System.Collections.Generic;

namespace WSATools.Libs
{
    public static class Ext
    {
        public static bool Before(this string str, string start, string end)
        {
            var startIndex = str.IndexOf(start);
            var endIndex = str.IndexOf(end);
            if (startIndex != -1)
                return startIndex < endIndex;
            return false;
        }
        public static string[] Splits(this string str, string separator)
        {
            List<string> results = new List<string>();
            var array = str.Split(separator);
            if (array != null)
            {
                foreach (var item in array)
                {
                    var data = item.Trim();
                    if (!string.IsNullOrEmpty(data))
                        results.Add(data);
                }
            }
            return results.ToArray();
        }
        public static string[] Splits(this string str, params char[] separator)
        {
            List<string> results = new List<string>();
            var array = str.Split(separator);
            if (array != null)
            {
                foreach (var item in array)
                {
                    var data = item.Trim();
                    if (!string.IsNullOrEmpty(data))
                        results.Add(data);
                }
            }
            return results.ToArray();
        }
        public static string Substring(this string str, string startChars, string endChars = null)
        {
            var startIndex = str.IndexOf(startChars, StringComparison.CurrentCultureIgnoreCase);
            int leftPadding = startChars.Length, endIndex = str.Length;
            if (!string.IsNullOrEmpty(endChars))
                endIndex = str.IndexOf(endChars, StringComparison.CurrentCultureIgnoreCase);
            return str.Substring(startIndex + leftPadding, endIndex - startIndex - leftPadding);
        }
    }
}