using System.Web.Mvc;

namespace Bogosoft.Mvc.Xsl
{
    static class ControllerContextExtensions
    {
        internal static string GetAction(this ControllerContext context)
        {
            return context.RouteData.Values["action"] as string;
        }

        internal static string GetController(this ControllerContext context)
        {
            return context.RouteData.Values["controller"] as string;
        }

        internal static string MapPath(this ControllerContext context, string path)
        {
            return context.HttpContext.Server.MapPath(path);
        }
    }
}