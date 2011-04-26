using System;
using System.Collections.Generic;
using System.Linq;
using Inferis.Core.Extensions;

namespace Inferis.Core
{
    public class Options
    {
        public static IDictionary<string, string> Build(object from)
        {
            var result = new Dictionary<string, string>();
            foreach (var prop in from.GetType().GetProperties()) {
                var attr = prop.GetCustomAttributes(typeof(ApiNameAttribute), true).FirstOrDefault() as ApiNameAttribute;
                var name = attr != null ? attr.Name : prop.Name.ToUnderscoreCase();
                result[name] = prop.GetValue(from, null).ToString();
            }
            return result;
        }

        public static IDictionary<string, string> Merge<TOptions>(params Action<TOptions>[] options)
            where TOptions : Options, new()
        {
            return Merge((IEnumerable<Action<TOptions>>)options);
        }

        public static IDictionary<string, string> Merge<TOptions>(IEnumerable<Action<TOptions>> options)
            where TOptions : Options, new()
        {
            var target = new TOptions();
            foreach (var opt in options) {
                opt(target);
            }

            var result = new Dictionary<string, string>();
            foreach (var prop in target.GetType().GetProperties()) {
                var attr = prop.GetCustomAttributes(typeof(ApiNameAttribute), true).FirstOrDefault() as ApiNameAttribute;
                if (attr == null) continue;

                var value = prop.GetValue(target, null);
                if (value == null) continue;

                if (value is Enum)
                    result[attr.Name] = ((Enum)value).GetApiValue();
                else
                    result[attr.Name] = value.ToString();
            }
            return result;
        }
    }
}
