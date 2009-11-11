using System.Text;

namespace Inferis.Core.Extensions
{
    public static class StringExtensions
    {
        public static string ToUnderscoreCase(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return source;

            var result = new StringBuilder((int)(source.Length * 1.5));
            foreach (var c in source) {
                if (char.IsUpper(c)) {
                    if (result.Length > 0)
                        result.Append('_');
                    result.Append(char.ToLower(c));
                }
                else
                    result.Append(c);
            }
            return result.ToString();
        }

        public static string ToUpperFirst(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return source;

            return string.Concat(
                source.Substring(0, 1).ToUpper(),
                source.Substring(1));
        }

        public static string ToLowerFirst(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return source;

            return string.Concat(
                source.Substring(0, 1).ToLower(),
                source.Substring(1));
        }

        public static string StripAllTags(this string html)
        {
            var array = new char[html.Length];
            var arrayIndex = 0;
            var inside = false;

            foreach (var let in html) {
                if (let == '<') {
                    inside = true;
                    continue;
                }
                if (let == '>') {
                    inside = false;
                    continue;
                }
                if (inside) continue;
                array[arrayIndex] = let;
                arrayIndex++;
            }
            return new string(array, 0, arrayIndex);
        }
    }
}