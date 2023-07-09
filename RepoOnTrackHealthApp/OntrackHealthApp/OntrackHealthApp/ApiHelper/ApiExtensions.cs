using OntrackHealthApp.AppCore;
using System;
using System.Collections.Generic;

namespace OntrackHealthApp.ApiHelper
{
    public static class ApiExtensions
    {
        public static KeyValuePair<string, string> AsPair(this string key, string value)
        {
            return new KeyValuePair<string, string>(key, value);
        }
        public static string ToAbsoluteUrl(this string requestUri)
        {
            string url = AppConstant.BaseAddress + (AppConstant.BaseApiDirectory + requestUri).Replace("//", "/");
            return url;
        }
        public static Uri ToAbsoluteUri(this string requestUri)
        {
            string url = AppConstant.BaseAddress + (AppConstant.BaseApiDirectory + requestUri).Replace("//", "/");
            return new Uri(string.Format(url, string.Empty)); ;
        }
    }
}
