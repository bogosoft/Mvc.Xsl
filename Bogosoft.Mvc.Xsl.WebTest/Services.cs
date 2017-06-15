using Bogosoft.Hashing;
using Bogosoft.Hashing.Cryptography;
using Bogosoft.Xml;
using Bogosoft.Xml.Xhtml5;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Bogosoft.Mvc.Xsl.WebTest
{
    static class Services
    {
        static string PhysicalApplicationPath = HttpRuntime.AppDomainAppPath.TrimEnd('/', '\\');

        static string BundledFilesCachePath => Path.Combine(PhysicalApplicationPath, "Content", "cached");

        internal static IDictionary<string, object> DefaultViewParameters = new Dictionary<string, object>()
        {
            { "page-title", "XSL Views Demo" }
        };

        public static IEnumerable<IDisposable> Disposables
        {
            get
            {
                var flags = BindingFlags.NonPublic | BindingFlags.Static;

                foreach(var fi in typeof(Services).GetFields(flags))
                {
                    if (fi.FieldType.GetInterfaces().Contains(typeof(IDisposable)))
                    {
                        yield return fi.GetValue(null) as IDisposable;
                    }
                }
            }
        }

        internal static IEnumerable<XmlFilterAsync> Filters
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

        internal static IEnumerable<PathFormatter> ViewLocations
        {
            get
            {
                yield return StandardViewPathFormatter;
                yield return SharedViewPathFormatter;
            }
        }

        internal static IFormatXml XmlFormatter => new Xhtml5Formatter
        {
            Indent = "\t",
            LBreak = "\r\n"
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
    }
}