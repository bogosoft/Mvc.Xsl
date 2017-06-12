using Bogosoft.Xml.Xhtml5;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Bogosoft.Mvc.Xsl.WebTest
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ViewEngines.Engines.Clear();

            var formatter = new Xhtml5Formatter { Indent = "\t", LBreak = "\r\n" };

            var xslEngine = new XsltViewEngine(GetViewLocations(), formatter.FormatAsync);

            ViewEngines.Engines.Add(xslEngine);
        }

        static IEnumerable<string> GetViewLocations()
        {
            yield return "~/Views/Shared/{0}.xslt";
            yield return "~/Views/{1}/{0}.xslt";
        }
    }
}