using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Encuentrame.Support
{
    public static class StringExtensions
    {
        public static string NotCompletedStringIfEmpty(this string text)
        {
            return (string.IsNullOrEmpty(text)) ? "-" : text;
        }

        public static int AsInt(this string text)
        {
            int value;
            int.TryParse(text, out value);
            return value;
        }

        public static byte AsByte(this string text)
        {
            byte value;
            byte.TryParse(text, out value);
            return value;
        }
        

        public static long AsLong(this string text)
        {
            long value;
            long.TryParse(text, out value);
            return value;
        }

        public static bool AsBool(this string text)
        {
            bool value;
            bool.TryParse(text, out value);
            return value;
        }

        public static decimal AsDecimal(this string text)
        {
            decimal value;
            decimal.TryParse(text, out value);
            return value;
        }

        public static T AsEnum<T>(this string text)
        {
            return (T)Enum.Parse(typeof(T), text);
        }

        public static T AsEnumOrDefault<T>(this string text, T @default)
        {
            try { return text.AsEnum<T>(); }
            catch { return @default; }
        }

        public static string From(this string text, string start)
        {
            var index = text.IndexOf(start);
            return index == -1 ? string.Empty : text.Substring(index + start.Length);
        }
        public static string Until(this string text, params string[] ends)
        {
            var value = text;
            foreach (var end in ends)
            {
                var index = text.IndexOf(end);
                if (index <= -1) continue;

                var tryValue = text.Substring(0, index);
                if (tryValue.Length < value.Length)
                    value = tryValue;
            }
            return value;
        }
        public static string Remove(this string text, string textToRemove)
        {
            return text.Replace(textToRemove, string.Empty);
        }

        public static string FormatWith(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        public static bool IsNullOrEmpty(this string stringToTest)
        {
            return string.IsNullOrEmpty(stringToTest);
        }

        public static bool NotIsNullOrEmpty(this string stringToTest)
        {
            return !stringToTest.IsNullOrEmpty();
        }

        private static readonly string[] WordSpeparators = new[] { ",", ":", ".", "$", "(", ")", " ", "#", Environment.NewLine, "\"" };
        
        public static IEnumerable<string> Words(this string text)
        {
            return text.Split(WordSpeparators, StringSplitOptions.RemoveEmptyEntries);
        }
        /// <summary>
        /// splitting a string using space when not surrounded by single or double quotes
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static IEnumerable<string> WordsAndPhrases(this string text)
        {
            return Regex.Matches(text, @"(?<match>\w+)|\""(?<match>[\w\s]*)""")
                    .Cast<Match>().Select(m => m.Groups["match"].Value).ToList();
        } 
        public static IEnumerable<int> Numbers(this string text,params char[] separator)
        {
            return text.Split(separator, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
        }

        public static string Concatenate(this string text, string moreText)
        {
            return string.Concat(text, moreText);
        }

        public static string ReduceTextAccordingTo(this string text, int maxLength)
        {
            return text.Length > maxLength ? text.Remove(maxLength - 3).Concatenate("...") : text;
        }

        public static bool InsensitiveLike(this string text, string value)
        {
            return text.ToLower().Contains(value.ToLower());
        }

       public static string RemoveDiacritics(this string text)
        {
            var normalizedText = text.Normalize(NormalizationForm.FormD);
            var builder = new StringBuilder();

            foreach (var t in normalizedText)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(t);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    builder.Append(t);
                }
            }

            return (builder.ToString().Normalize(NormalizationForm.FormC));
        }

        public static string ToCssClass(this string text)
        {
            return text.ToLower().Replace(" ", "").RemoveDiacritics();
        }
        public static bool CompareIgnore(this string text1, string text2)
        {
            return
                text1.ToUpper().RemoveDiacritics().Equals(text2.ToUpper().RemoveDiacritics());
        }
        public static bool IsNumber(this string str)
        {
            double temp;
            return double.TryParse(str, out temp);
        }

        public static string GetSubstring(this string text, int indexStart, int length)
        {
            if (text.Length > length)
            {
                return text.Substring(indexStart, length);
            }
            return text;
        }

    }
}
