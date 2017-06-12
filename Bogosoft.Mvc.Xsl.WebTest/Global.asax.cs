using Bogosoft.Xml.Xhtml5;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Bogosoft.Mvc.Xsl.WebTest
{
    public class MvcApplication : System.Web.HttpApplication
    {
        static readonly Dictionary<string, object> DefaultViewParameters = new Dictionary<string, object>()
        {
            { "page-title", "XSL Views Demo" }
        };

        static string SharedViewPathFormatter(ControllerContext context)
        {
            var data = context.RouteData.Values;

            return context.HttpContext.Server.MapPath($"~/Views/Shared/{data["action"]}.xslt");
        }

        static string StandardViewPathFormatter(ControllerContext context)
        {
            var data = context.RouteData.Values;

            return context.HttpContext.Server.MapPath($"~/Views/{data["controller"]}/{data["action"]}.xslt");
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ViewEngines.Engines.Clear();

            var formatter = new Xhtml5Formatter { Indent = "\t", LBreak = "\r\n" };

            var xslEngine = new XsltViewEngine(GetViewLocations(), formatter.FormatAsync, DefaultViewParameters);

            ViewEngines.Engines.Add(xslEngine);
        }

        static IEnumerable<PathFormatter> GetViewLocations()
        {
            yield return StandardViewPathFormatter;
            yield return SharedViewPathFormatter;
        }
    }
}