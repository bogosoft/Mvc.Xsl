using Bogosoft.Hashing;
using Bogosoft.Hashing.Cryptography;
using Bogosoft.Mvc.Xsl;
using Bogosoft.Xml;
using Bogosoft.Xml.Xhtml5;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace $rootnamespace$
{
    static class ViewEnginesConfig
    {
        #region Infrastructure

        enum Key
        {
            BundleCss,
            BundledFilesCacheDirectory,
            BundleJavascript,
            CacheLocalTransforms,
            ClearViewEngines,
            Indent,
            LBreak
        }

        class Setting
        {
            static string[] truths = new[] { "1", "on", "true", "yes" };

            internal static IEnumerable<KeyValuePair<string, object>> All
            {
                get
                {
                    var settings = ConfigurationManager.AppSettings;

                    foreach (var x in settings.AllKeys)
                    {
                        if (x.StartsWith("Bogosoft.Mvc.Xsl"))
                        {
                            yield return new KeyValuePair<string, object>(x, settings[x]);
                        }
                    }
                }
            }

            internal static string Get(Key key)
            {
                return ConfigurationManager.AppSettings[$"Bogosoft.Mvc.Xsl.{key}"];
            }

            internal static bool IsTrue(Key key)
            {
                return truths.Contains(Get(key).ToLower());
            }
        }

        static IEnumerable<IFilterXml> Filters
        {
            get
            {
                if (Setting.IsTrue(Key.BundleCss))
                {
                    yield return new CssBundlingFilter(
                        ToRelativeUri,
                        ToPhysicalPath,
                        ToBundledCssFilepath
                        );
                }

                if (Setting.IsTrue(Key.BundleJavascript))
                {
                    yield return new JavascriptBundlingFilter(
                        ToRelativeUri,
                        ToPhysicalPath,
                        ToBundledJavascriptFilepath,
                        "/html/body"
                        );
                }
            }
        }

        static string BundledFilesCachePath;

        static IDictionary<string, object> DefaultViewParameters
        {
            get
            {
                var parameters = new Dictionary<string, object>();

                foreach (var x in Setting.All)
                {
                    if (!x.Key.StartsWith("Bogosoft.Mvc.Xsl.Parameter."))
                    {
                        continue;
                    }

                    parameters.Add(x.Key.Replace("Bogosoft.Mvc.Xsl.Parameter.", ""), x.Value);
                }

                return parameters;
            }
        }

        static StandardXmlFormatter Formatter => new Xhtml5Formatter
        {
            Indent = Setting.Get(Key.Indent),
            LBreak = Setting.Get(Key.LBreak)
        };

        static IEnumerable<TransformProvider> SourceTransformProviders
        {
            get
            {
                yield return new LocalFileTransformProvider(LocalViewPathFormatter).GetTransform;
                yield return new LocalFileTransformProvider(LocalSharedViewPathFormatter).GetTransform;
            }
        }

        static ITransformProvider TransformProvider
        {
            get
            {
                ITransformProvider provider = new CompositeTransformProvider(SourceTransformProviders);

                if (Setting.IsTrue(Key.CacheLocalTransforms))
                {
                    provider = new MemoryCachedTransformProvider<string>(provider, AbsolutePathSelector);
                }

                return provider;
            }
        }

        static string AbsolutePathSelector(ControllerContext context)
        {
            return context.HttpContext.Request.Url.AbsolutePath;
        }

        static string Action(ControllerContext context)
        {
            return context.RouteData.Values["action"] as string;
        }

        static bool BundlingEnabled => Setting.IsTrue(Key.BundleCss)
                                    || Setting.IsTrue(Key.BundleJavascript);

        static string Controller(ControllerContext context)
        {
            return context.RouteData.Values["controller"] as string;
        }

        static string LocalSharedViewPathFormatter(ControllerContext context)
        {
            return Resolve(context, $"~/Views/Shared/{Action(context)}.xslt");
        }

        static string LocalViewPathFormatter(ControllerContext context)
        {
            return Resolve(context, $"~/Views/{Controller(context)}/{Action(context)}.xslt");
        }

        static IEnumerable<ITransformProvider> GetSourceTransformProviders()
        {
            yield return new LocalFileTransformProvider(LocalViewPathFormatter);
            yield return new LocalFileTransformProvider(LocalSharedViewPathFormatter);
        }

        static string Resolve(ControllerContext context, string path)
        {
            return context.HttpContext.Server.MapPath(path);
        }

        static string ToBundledCssFilepath(IEnumerable<string> uris)
        {
            return ToBundledFilepath(uris, "css");
        }

        static string ToBundledFilepath(IEnumerable<string> uris, string extension)
        {
            var hash = CryptoHashStrategy.MD5.Compute(uris).ToHexString();

            return Path.Combine(BundledFilesCachePath, $"{hash}.{extension}");
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

        #endregion

        internal static IEnumerable<object> Configure()
        {
            if (Setting.IsTrue(Key.ClearViewEngines))
            {
                ViewEngines.Engines.Clear();
            }

            if (BundlingEnabled)
            {
                var directory = Setting.Get(Key.BundledFilesCacheDirectory);

                BundledFilesCachePath = Path.Combine(HttpRuntime.AppDomainAppPath, directory);

                if (!Directory.Exists(BundledFilesCachePath))
                {
                    Directory.CreateDirectory(BundledFilesCachePath);
                }
            }

            ITransformProvider provider;

            yield return provider = TransformProvider;

            var engine = new XsltViewEngine(provider, Formatter.With(Filters), DefaultViewParameters);

            ViewEngines.Engines.Add(engine);

            engine.ParameterizingView += ViewEngine_ParameterizingView;
        }

        static void ViewEngine_ParameterizingView(ParameterizingViewEventArgs args)
        {
            args.Parameters["action"] = args.Context.RouteData.Values["action"];
            args.Parameters["controller"] = args.Context.RouteData.Values["controller"];
        }
    }
}