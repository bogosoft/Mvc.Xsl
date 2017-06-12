using Bogosoft.Xml;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Xsl;

namespace Bogosoft.Mvc.Xsl
{
    /// <summary>
    /// An implementation of <see cref="IViewEngine"/> that uses XSLT to render model projections.
    /// </summary>
    public class XsltViewEngine : IViewEngine
    {
        /// <summary>
        /// Get the default argument dictionary of the current view engine.
        /// These are populated on application start and can be overridden with the contents
        /// of a <see cref="ViewDataDictionary"/> object on view creation.
        /// </summary>
        protected readonly IDictionary<string, object> DefaultParameters;

        /// <summary>Get or set the current engine's associated formatter.</summary>
        protected XmlFormatterAsync Formatter;

        /// <summary>
        /// Get or set a collection of locations as filepath formatters.
        /// </summary>
        protected PathFormatter[] Locations;

        /// <summary>Create a new instance of an <see cref="XsltViewEngine"/>.</summary>
        /// <param name="locations">A collection of filepath formatters.</param>
        /// <param name="formatter">An XML formatter to use when rendering.</param>
        /// <param name="defaultParameters">
        /// A collection of default parameters which will be provided to any view that is created.
        /// These parameters are overridden by any parameters set in the controller.
        /// </param>
        public XsltViewEngine(
            IEnumerable<PathFormatter> locations,
            XmlFormatterAsync formatter = null,
            IDictionary<string, object> defaultParameters = null
            )
            : this(locations.ToArray(), formatter, defaultParameters)
        {
        }

        /// <summary>Create a new instance of an <see cref="XsltViewEngine"/>.</summary>
        /// <param name="locations">A collection of filepath formatters.</param>
        /// <param name="formatter">An XML formatter to use when rendering.</param>
        /// <param name="defaultParameters">
        /// A collection of default parameters which will be provided to any view that is created.
        /// These parameters are overridden by any parameters set in the controller.
        /// </param>
        public XsltViewEngine(
            PathFormatter[] locations,
            XmlFormatterAsync formatter = null,
            IDictionary<string, object> defaultParameters = null
            )
        {
            DefaultParameters = defaultParameters ?? new Dictionary<string, object>();
            Formatter = formatter;
            Locations = locations;
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
                    var processor = new XslCompiledTransform();

                    processor.Load(path);

                    var view = new XsltView(
                        processor,
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
    }
}