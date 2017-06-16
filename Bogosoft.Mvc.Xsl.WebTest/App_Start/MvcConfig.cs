using Bogosoft.Hashing.Cryptography;
using Bogosoft.Xml;
using Bogosoft.Xml.Xhtml5;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Bogosoft.Mvc.Xsl.WebTest
{
    static class MvcConfig
    {
        static string BundledFilesCachePath => Path.Combine(PhysicalApplicationPath, "Content", "cached");

        static IDictionary<string, object> DefaultViewParameters = new Dictionary<string, object>()
        {
            { "page-title", "XSL Views Demo" }
        };

        static IEnumerable<AsyncXmlFilter> Filters
        {
            get
            {
                yield return new CssBundlingFilter(
                    PhysicalApplicationPath,
                    "content/cached",
                    BundledFilesCachePath,
                    CryptoHashStrategy.MD5
                    ).FilterAsync;
            }
        }

        static string PhysicalApplicationPath = HttpRuntime.AppDomainAppPath.TrimEnd('/', '\\');

        static IEnumerable<ITransformProvider> TransformProviders
        {
            get
            {
                yield return new LocalFileTransformProvider(LocalViewPathFormatter);
                yield return new LocalFileTransformProvider(LocalSharedViewPathFormatter);
            }
        }

        static IEnumerable<PathFormatter> ViewLocations
        {
            get
            {
                yield return LocalViewPathFormatter;
                yield return LocalSharedViewPathFormatter;
            }
        }

        internal static IEnumerable<object> Configure()
        {
            var formatter = new Xhtml5Formatter { Indent = "\t", LBreak = "\r\n" }.With(Filters);

            var engine = XsltViewEngine.Create(TransformProviders)
                                       .Using(formatter)
                                       .With(DefaultViewParameters);

            ViewEngines.Engines.Clear();

            ViewEngines.Engines.Add(engine);

            yield break;
        }

        static string LocalSharedViewPathFormatter(ControllerContext context)
        {
            var data = context.RouteData;

            return context.MapPath($"~/Views/Shared/{data.GetAction()}.xslt");
        }

        static string LocalViewPathFormatter(ControllerContext context)
        {
            var data = context.RouteData;

            return context.MapPath($"~/Views/{data.GetController()}/{data.GetAction()}.xslt");
        }
    }
}