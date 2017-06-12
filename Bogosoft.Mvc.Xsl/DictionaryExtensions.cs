using System.Collections.Generic;
using System.Web.Mvc;

namespace Bogosoft.Mvc.Xsl
{
    static class DictionaryExtensions
    {
        internal static void Combine(this Dictionary<string, object> source, ViewDataDictionary extra)
        {
            foreach(var x in extra)
            {
                source[x.Key] = x.Value;
            }
        }

        internal static Dictionary<string, object> Copy(this IDictionary<string, object> source)
        {
            var copy = new Dictionary<string, object>();

            foreach(var x in source)
            {
                copy.Add(x.Key, x.Value);
            }

            return copy;
        }
    }
}