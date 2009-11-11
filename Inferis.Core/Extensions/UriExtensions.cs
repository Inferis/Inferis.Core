using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Inferis.Core.Extensions
{
    public static class UriExtensions
    {
        public static Uri AddParameters(this Uri uri, IDictionary<string, string> parameters)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");

            var result = new StringBuilder();
            result.Append(uri.ToString());
            result.Append(parameters.EncodeForUri(true));
            return new Uri(result.ToString());
        }

        public static string EncodeForUri(this IDictionary<string, string> parameters, bool includeInitialQuestionMark)
        {
            var result = new StringBuilder();
            if (includeInitialQuestionMark)
                result.Append("?");

            if (parameters != null) {
                var sep = "";
                foreach (var key in parameters.Keys) {
                    result.Append(sep);
                    result.Append(SafeEncode(key));
                    result.Append("=");
                    result.Append(SafeEncode(parameters[key] ?? ""));

                    sep = "&";
                }
            }

            return result.ToString();
        }

        public static string SafeEncode(this string source)
        {
            var result = new StringBuilder();
            source.SafeEncodeTo(result);
            return result.ToString();
        }

        public static void SafeEncodeTo(this string source, StringBuilder target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            if (string.IsNullOrEmpty(source))
                return;

            const string reserved = @"!*'();:@&=+$,/?%#[]"" ";
            foreach (var c in source) {
                if (reserved.IndexOf(c) >= 0 || char.IsControl(c)) {
                    target.AppendFormat("%{0:X2}", (int)c);
                }
                else
                    target.Append(c);
            }
        }
    }
}