using System.Web.Mvc;

namespace Bogosoft.Mvc.Xsl.WebTest
{
    static class ControllerContextExtensions
    {
        internal static string MapPath(this ControllerContext context, string path)
        {
            return context.HttpContext.Server.MapPath(path);
        }
    }
}