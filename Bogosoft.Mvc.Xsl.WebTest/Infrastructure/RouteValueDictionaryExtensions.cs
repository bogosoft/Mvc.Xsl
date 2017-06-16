using System.Web.Routing;

namespace Bogosoft.Mvc.Xsl.WebTest
{
    static class RouteValueDictionaryExtensions
    {
        internal static string GetAction(this RouteData data)
        {
            return data.Values["action"] as string;
        }

        internal static string GetController(this RouteData data)
        {
            return data.Values["controller"] as string;
        }
    }
}