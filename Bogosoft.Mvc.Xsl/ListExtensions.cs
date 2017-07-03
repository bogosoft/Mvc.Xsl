using System.Collections.Generic;

namespace Bogosoft.Mvc.Xsl
{
    static class ListExtensions
    {
        internal static void Add<T>(this List<T> list, IEnumerable<T> items)
        {
            foreach(var item in items)
            {
                list.Add(item);
            }
        }
    }
}