using Bogosoft.Xml;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Bogosoft.Mvc.Xsl
{
    /// <summary>
    /// An implementation of <see cref="IViewEngine"/> that uses XSLT to render model projections.
    /// </summary>
    public class XsltViewEngine : IViewEngine
    {
        /// <summary>
        /// Create a new instance of an <see cref="XsltViewEngine"/>.
        /// </summary>
        /// <param name="locations">A collection of filepath formatters.</param>
        /// <param name="transformProvider">An XSL transform provider.</param>
        /// <returns>A new XSLT view engine.</returns>
        public static XsltViewEngine Create(
            IEnumerable<PathFormatter> locations,
            XslTransformProvider transformProvider
            )
        {
            return new XsltViewEngine
            {
                Locations = locations.ToArray(),
                TransformProvider = transformProvider
            };
        }

        /// <summary>
        /// Create a new instance of an <see cref="XsltViewEngine"/>.
        /// </summary>
        /// <param name="locations">A collection of filepath formatters.</param>
        /// <param name="transformProvider">An XSL transform provider.</param>
        /// <returns>A new XSLT view engine.</returns>
        public static XsltViewEngine Create(
            IEnumerable<PathFormatter> locations,
            IXslTransformProvider transformProvider
            )
        {
            return new XsltViewEngine
            {
                Locations = locations.ToArray(),
                TransformProvider = transformProvider.Provision
            };
        }

        /// <summary>
        /// Get the default argument dictionary of the current view engine.
        /// These are populated on application start and can be overridden with the contents
        /// of a <see cref="ViewDataDictionary"/> object on view creation.
        /// </summary>
        protected IDictionary<string, object> DefaultParameters = new Dictionary<string, object>();

        /// <summary>
        /// Get or set an array of XML filters associated with the current view engine.
        /// </summary>
        protected XmlFilterAsync[] Filters;

        /// <summary>Get or set the current engine's associated formatter.</summary>
        protected XmlFormatterAsync Formatter;

        /// <summary>
        /// Get or set a collection of locations as filepath formatters.
        /// </summary>
        protected PathFormatter[] Locations;

        /// <summary>
        /// Get or set the XSL transform provider for the current view engine.
        /// </summary>
        protected XslTransformProvider TransformProvider;

        /// <summary>
        /// Create a new instance of the <see cref="XsltViewEngine"/> class.
        /// </summary>
        protected XsltViewEngine()
        {
        }

        /// <summary>
        /// Finds the specified partial view by using the specified controller context.
        /// </summary>
        /// <param name="context">A controller context.</param>
        /// <param name="name">The name of a partial view.</param>
        /// <param name="useCache">Unused.</param>
        /// <returns>
        /// A successful result that either contains a view, or failed result which
        /// contains a list of all locations searched.
        /// </returns>
        public ViewEngineResult FindPartialView(ControllerContext context, string name, bool useCache)
        {
            return FindView(context, name, null, useCache);
        }

        /// <summary>
        /// Finds the specified partial view by using the specified controller context.
        /// </summary>
        /// <param name="context">A controller context.</param>
        /// <param name="name">The name of a partial view.</param>
        /// <param name="ignored">Unused.</param>
        /// <param name="useCache">Unused.</param>
        /// <returns>
        /// A successful result that either contains a view, or failed result which
        /// contains a list of all locations searched.
        /// </returns>
        public ViewEngineResult FindView(
            ControllerContext context,
            string name,
            string ignored,
            bool useCache
            )
        {
            string path;

            List<string> searched = new List<string>();

            foreach (var x in Locations)
            {
                path = x.Invoke(context);

                searched.Add(path);

                if (File.Exists(path))
                {
                    var view = new XsltView(
                        TransformProvider.Invoke(path),
                        Filters ?? new XmlFilterAsync[0],
                        Formatter,
                        DefaultParameters.Copy()
                        );

                    return new ViewEngineResult(view, this);
                }
            }

            return new ViewEngineResult(searched);
        }

        /// <summary>
        /// Releases the specified view by using the specified controller context.
        /// Not used in the current implementation.
        /// </summary>
        /// <param name="context">A controller context.</param>
        /// <param name="view">A view to release.</param>
        public void ReleaseView(ControllerContext context, IView view)
        {
        }

        /// <summary>
        /// Instruct the current view engine to use a given XML filter before formatting.
        /// </summary>
        /// <param name="filter">An XML filter.</param>
        /// <returns>The current view engine.</returns>
        public XsltViewEngine Using(IXmlFilter filter)
        {
            Filters = new XmlFilterAsync[] { filter.FilterAsync };

            return this;
        }

        /// <summary>
        /// Instruct the current view engine to use a given XML filter before formatting.
        /// </summary>
        /// <param name="filter">An XML filter.</param>
        /// <returns>The current view engine.</returns>
        public XsltViewEngine Using(XmlFilterAsync filter)
        {
            Filters = new XmlFilterAsync[] { filter };

            return this;
        }

        /// <summary>
        /// Instruct the current view engine to use a given set of XML filters before formatting.
        /// </summary>
        /// <param name="filters">A collection of XML filters.</param>
        /// <returns>The current view engine.</returns>
        public XsltViewEngine Using(IEnumerable<IXmlFilter> filters)
        {
            Filters = filters.Select<IXmlFilter, XmlFilterAsync>(x => x.FilterAsync).ToArray();

            return this;
        }

        /// <summary>
        /// Instruct the current view engine to use a given set of XML filters before formatting.
        /// </summary>
        /// <param name="filters">A collection of XML filters.</param>
        /// <returns>The current view engine.</returns>
        public XsltViewEngine Using(IXmlFilter[] filters)
        {
            Filters = filters.Select<IXmlFilter, XmlFilterAsync>(x => x.FilterAsync).ToArray();

            return this;
        }

        /// <summary>
        /// Instruct the current view engine to use a given set of XML filters before formatting.
        /// </summary>
        /// <param name="filters">A collection of XML filters.</param>
        /// <returns>The current view engine.</returns>
        public XsltViewEngine Using(IEnumerable<XmlFilterAsync> filters)
        {
            Filters = filters.ToArray();

            return this;
        }

        /// <summary>
        /// Instruct the current view engine to use a given set of XML filters before formatting.
        /// </summary>
        /// <param name="filters">A collection of XML filters.</param>
        /// <returns>The current view engine.</returns>
        public XsltViewEngine Using(XmlFilterAsync[] filters)
        {
            Filters = filters;

            return this;
        }

        /// <summary>
        /// Instruct the current view engine to use a given XML formatter.
        /// </summary>
        /// <param name="formatter">An XML formatter.</param>
        /// <returns>The current view engine.</returns>
        public XsltViewEngine Using(IFormatXml formatter)
        {
            Formatter = formatter.FormatAsync;

            return this;
        }

        /// <summary>
        /// Instruct the current view engine to use a given XML formatter.
        /// </summary>
        /// <param name="formatter">An XML formatter.</param>
        /// <returns>The current view engine.</returns>
        public XsltViewEngine Using(XmlFormatterAsync formatter)
        {
            Formatter = formatter;

            return this;
        }

        /// <summary>
        /// Instruct the current view engine to pass along a collection of overridable
        /// parameters to any views that it creates.
        /// </summary>
        /// <param name="defaultParameters">A collection of overridable parameters.</param>
        /// <returns>The current view engine.</returns>
        public XsltViewEngine With(IDictionary<string, object> defaultParameters)
        {
            DefaultParameters = defaultParameters;

            return this;
        }
    }
}