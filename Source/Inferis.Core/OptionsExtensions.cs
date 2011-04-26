using System;
using System.Collections.Generic;
using System.Linq;

namespace Inferis.Core
{
    public static class OptionsExtensions
    {
        public static IDictionary<string, string> Merge<TOptions>(this IEnumerable<Action<TOptions>> options)
            where TOptions : Options, new()
        {
            return Options.Merge(options);
        }
    }
}
