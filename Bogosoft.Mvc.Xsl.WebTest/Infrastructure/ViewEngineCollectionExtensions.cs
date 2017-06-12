using System.Collections.Generic;
using System.Web.Mvc;

namespace Bogosoft.Mvc.Xsl.WebTest.Infrastructure
{
    static class ViewEngineCollectionExtensions
    {
        internal static void Add(this ViewEngineCollection collection, IEnumerable<IViewEngine> engines)
        {
            foreach(var engine in engines)
            {
                collection.Add(engine);
            }
        }
    }
}