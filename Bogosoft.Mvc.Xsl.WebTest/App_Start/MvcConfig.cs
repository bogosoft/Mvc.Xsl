using Bogosoft.Hashing;
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
        static string BundledFilesCachePath => Path.Combine(ApplicationPath, "Content", "cached");

        static IDictionary<string, object> DefaultViewParameters = new Dictionary<string, object>()
        {
            { "page-title", "XSL Views Demo" }
        };

        static IEnumerable<IFilterXml> Filters
        {
            get
            {
                yield return new CssBundlingFilter(ToRelativeUri, ToPhysicalPath, ToBundledCssFilepath);
                yield return new JavascriptBundlingFilter(
                    ToRelativeUri,
                    ToPhysicalPath,
                    ToBundledJavascriptFilepath,
                    "/html/body"
                    );
            }
        }

        static string ApplicationPath = HttpRuntime.AppDomainAppPath.TrimEnd('/', '\\');

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

            var engine = new XsltViewEngine(TransformProviders).Using(formatter).With(DefaultViewParameters);

            engine.ParameterizingView += ViewEngine_ParameterizingView;

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

        static string ToBundledCssFilepath(IEnumerable<string> uris)
        {
            return ToBundledFilepath(uris, "css");
        }

        static string ToBundledFilepath(IEnumerable<string> uris, string extension)
        {
            var hash = CryptoHashStrategy.MD5.Compute(uris).ToHexString();

            return Path.Combine(HttpRuntime.AppDomainAppPath, "content", "cached", $"{hash}.{extension}");
        }

        static string ToBundledJavascriptFilepath(IEnumerable<string> uris)
        {
            return ToBundledFilepath(uris, "js");
        }

        static string ToPhysicalPath(string uri)
        {
            return Path.Combine(HttpRuntime.AppDomainAppPath, uri);
        }

        static string ToRelativeUri(string filepath)
        {
            return $"content/cached/{Path.GetFileName(filepath)}";
        }

        static void ViewEngine_ParameterizingView(ParameterizingViewEventArgs args)
        {
            args.Parameters["action"] = args.Context.RouteData.GetAction();
            args.Parameters["controller"] = args.Context.RouteData.GetController();
        }
    }
}