using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Bogosoft.Mvc.Xsl
{
    internal static class NameValueCollectionExtensions
    {
        internal static IDictionary<String, Object> ToDictionary(this NameValueCollection values)
        {
            var dictionary = new Dictionary<String, Object>();

            foreach(var k in values.AllKeys)
            {
                dictionary[k] = values[k];
            }

            return dictionary;
        }
    }
}