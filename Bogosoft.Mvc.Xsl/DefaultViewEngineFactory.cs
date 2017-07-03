using Bogosoft.Hashing;
using Bogosoft.Hashing.Cryptography;
using Bogosoft.Xml;
using Bogosoft.Xml.Xhtml5;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bogosoft.Mvc.Xsl
{
    /// <summary>
    /// A default implementation of the <see cref="ViewEngineFactoryBase"/> class.
    /// </summary>
    public sealed class DefaultViewEngineFactory : ViewEngineFactoryBase
    {
        enum Key
        {
            BundleCss,
            BundledFilesCacheDirectory,
            BundleJavascript,
            CacheLocalTransforms,
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

            internal static string Get(Key key, string fallback = null)
            {
                return ConfigurationManager.AppSettings[$"Bogosoft.Mvc.Xsl.{key}"] ?? fallback;
            }

            internal static bool IsTrue(Key key)
            {
                return truths.Contains(Get(key, "0").ToLower());
            }
        }

        static string BundledFilesCachePath;

        static DefaultViewEngineFactory()
        {
            var directory = Setting.Get(Key.BundledFilesCacheDirectory);

            BundledFilesCachePath = Path.Combine(HttpRuntime.AppDomainAppPath, directory);

            if (!Directory.Exists(BundledFilesCachePath))
            {
                Directory.CreateDirectory(BundledFilesCachePath);
            }
        }

        static string AbsolutePathSelector(ControllerContext context)
        {
            return context.HttpContext.Request.Url.AbsolutePath;
        }

        static string LocalSharedViewPathFormatter(ControllerContext context)
        {
            return context.MapPath($"~/Views/Shared/{context.GetAction()}.xslt");
        }

        static string LocalViewPathFormatter(ControllerContext context)
        {
            return context.MapPath($"~/Views/{context.GetController()}/{context.GetAction()}.xslt");
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

        protected override IDictionary<string, object> DefaultParameters
        {
            get
            {
                var parameters = new Dictionary<string, object>();

                foreach(var x in Setting.All)
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

        protected override IEnumerable<IFilterXml> Filters
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

        protected override StandardXmlFormatter Formatter => new Xhtml5Formatter
        {
            Indent = Setting.Get(Key.Indent),
            LBreak = Setting.Get(Key.LBreak)
        };

        IEnumerable<ITransformProvider> LocalTransformProviders
        {
            get
            {
                yield return new LocalFileTransformProvider(LocalViewPathFormatter);
                yield return new LocalFileTransformProvider(LocalSharedViewPathFormatter);
            }
        }

        protected override ITransformProvider TransformProvider
        {
            get
            {
                ITransformProvider provider = new CompositeTransformProvider(LocalTransformProviders);

                if (Setting.IsTrue(Key.CacheLocalTransforms))
                {
                    provider = new MemoryCachedTransformProvider<string>(provider, AbsolutePathSelector);
                }

                return provider;
            }
        }
    }
}