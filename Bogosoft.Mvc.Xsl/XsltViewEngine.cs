using Bogosoft.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Xsl;

namespace Bogosoft.Mvc.Xsl
{
    /// <summary>
    /// An implementation of <see cref="IViewEngine"/> that uses XSLT to render model projections.
    /// </summary>
    public class XsltViewEngine : VirtualPathProviderViewEngine
    {
        /// <summary>
        /// Get the default argument dictionary of the current view engine.
        /// These are populated on application start and can be overridden with the contents
        /// of a <see cref="ViewDataDictionary"/> object on view creation.
        /// </summary>
        protected readonly IDictionary<string, object> DefaultParameters;

        /// <summary>Get or set the current engine's associated formatter.</summary>
        protected XmlFormatterAsync Formatter;

        /// <summary>Create a new instance of an <see cref="XsltViewEngine"/>.</summary>
        /// <param name="locations">A collection of location formats.</param>
        /// <param name="formatter">An XML to use when rendering.</param>
        /// <param name="settings">
        /// A collection of settings which will be used to populate a collection of default arguments.
        /// This could come from the AppSettings property of the ConfigurationManager class, for instance. 
        /// </param>
        public XsltViewEngine(
            IEnumerable<string> locations,
            XmlFormatterAsync formatter,
            IDictionary<string, object> settings = null
            )
        {
            DefaultParameters = settings ?? new Dictionary<string, object>();

            Formatter = formatter;

            PartialViewLocationFormats = ViewLocationFormats = locations.ToArray();
        }

        /// <summary>Creates the specified partial view by using the specified controller context.</summary>
        /// <param name="context">A controller context.</param>
        /// <param name="path">A partial path for the new partial view.</param>
        /// <returns>A new instance of <see cref="IView"/>.</returns>
        /// <remarks>
        /// This method simply redirects to
        /// <see cref="CreateView(ControllerContext, String, String)"/>.
        /// </remarks>
        protected override IView CreatePartialView(ControllerContext context, String path)
        {
            return CreateView(context, path, null);
        }

        /// <summary>
        /// Creates the specified view by using a <see cref="ControllerContext"/> and the path to the view.
        /// </summary>
        /// <param name="context">A <see cref="ControllerContext"/>.</param>
        /// <param name="path">A path to a view.</param>
        /// <param name="ignored">This parameter is not used.</param>
        /// <returns>A new instance of <see cref="IView"/>.</returns>
        protected override IView CreateView(ControllerContext context, String path, String ignored)
        {
            if (path.StartsWith("~"))
            {
                path = context.HttpContext.Server.MapPath(path);
            }

            var processor = new XslCompiledTransform();

            processor.Load(path);

            return new XsltView(processor, Formatter, DefaultParameters.Copy());
        }
    }
}