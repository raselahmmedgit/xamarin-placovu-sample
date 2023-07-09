using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace OntrackHealthApp.ApiHelper.Extensions
{
    public static class TypeExtensions
    {
        public static Guid ToGuid(this object obj)
        {
            if (obj == null) return Guid.Empty;
            Guid outResult;
            bool result = Guid.TryParse(obj.ToString(), out outResult);
            if (result)
                return outResult;
            return Guid.Empty;
        }
        public static string ToGuidString(this Guid obj)
        {
            return obj.ToString("N").ToLower();
        }
        public static string ToGuidString(this string obj)
        {
            if (!string.IsNullOrEmpty(obj))
                return obj.ToGuid().ToString("N").ToLower();
            return null;
        }
        public static int ToInt(this object obj, int defaultValue)
        {
            return ToInt32(obj, defaultValue);
        }
        public static int ToInt(this object obj)
        {
            return ToInt32(obj, 0);
        }
        public static int ToInt32(this object obj, int defaultValue)
        {
            if (obj == null || Convert.ToString(obj) == string.Empty || obj.ToString() == "-99" || obj.ToString().ToLower() == "new" || obj.ToString().ToLower() == "false")
            {
                return defaultValue;
            }
            if (obj.ToString().ToLower() == "true")
            {
                return 1;
            }
            return Convert.ToInt32(obj);
        }
        public static Int64 ToInt64(this object obj)
        {
            var input = obj == null || Convert.ToString(obj).Trim() == string.Empty || obj.ToString() == "-99" || obj.ToString().ToLower() == "new" ? 0 : obj;
            long result = 0;
            long.TryParse(input.ToString(), out result);
            return result;
        }
        public static Int64 ToLong(this int obj)
        {
            return Convert.ToInt64(obj);
        }
        public static Int64 ToLong(this object obj)
        {
            return ToInt64(obj);
        }
        public static Decimal ToDecimal(this object obj)
        {
            return Convert.ToDecimal(obj == null || Convert.ToString(obj) == string.Empty || obj.ToString() == "-99" || obj.ToString().ToLower() == "new" ? 0 : obj);
        }
        public static Decimal ToRound(this object obj)
        {
            Decimal tmp = Convert.ToDecimal(obj == null || Convert.ToString(obj) == string.Empty || obj.ToString() == "-99" || obj.ToString().ToLower() == "new" ? 0 : obj);
            return Math.Round(tmp, 2);
        }

        public static string ToProperCase(this string str)
        {
            string formattedText = null;

            if (!string.IsNullOrEmpty(str))
            {
                formattedText = new System.Globalization.CultureInfo("en").TextInfo.ToTitleCase(str.ToLower());
            }
            return formattedText;
        }
        public static string ToStringIfNull(this object input)
        {
            if (input == null) return string.Empty;
            return input.ToString().Trim();
        }

        public static string ToFormatedPhoneNumber(this object input)
        {
            if (input == null) return string.Empty;
            var value = input.ToString().Trim();
            if (!string.IsNullOrEmpty(value) && value.Length >= 10 && !value.Contains("-"))
            {
                value = value.Replace("-", "");
                var valueN = "";
                valueN = value.Substring(0, 3) + "-";
                valueN += value.Substring(3, 3) + "-";
                valueN += value.Substring(6, value.Length - 6);
                return valueN;
            }
            return value;
        }

        public static string MakeSafeForAppStoreShortLinkUrl(this string value)
        {
            // Reference: https://developer.apple.com/library/content/qa/qa1633/_index.html

            var regex = new Regex(@"[©™®!¡""#$%'()*+,\\\-.\/:;<=>¿?@[\]^_`{|}~]*");

            var safeValue = regex.Replace(value, "")
                                 .Replace(" ", "")
                                 .Replace("&", "and")
                                 .ToLower();

            return safeValue;
        }

        /// <summary>
        /// Input: MM/DD/YYYY output:DD/MMM/YYYY 00:00:00.000
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime? ToFormatedDateFromStartWithMonth(this string date)
        {
            if (string.IsNullOrEmpty(date))
                return null;
            var formatInfoinfo = new DateTimeFormatInfo();
            date = date.Replace("/", "-");
            List<string> lst = date.Split('-').ToList<string>();
            string first = lst[1];
            lst[1] = lst[0];
            lst[0] = first;
            if (lst.Count == 3)
            {
                if (lst[1].Length != 3)
                {
                    lst[1] = formatInfoinfo.GetMonthName(lst[1].ToInt()).ToString();
                }
                return Convert.ToDateTime(lst.Aggregate((i, j) => i + "-" + j) + "  00:00:00.000");
            }
            else
                return null;
        }

    }
}
