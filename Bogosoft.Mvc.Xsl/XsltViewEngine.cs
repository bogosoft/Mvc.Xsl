using Bogosoft.Xml;
using System.Collections.Generic;
using System.Web.Mvc;

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
        protected IDictionary<string, object> DefaultParameters = new Dictionary<string, object>();

        /// <summary>Get or set the current engine's associated formatter.</summary>
        protected XmlFormatterAsync Formatter;

        /// <summary>
        /// Get or set an array of XSL transform providers.
        /// </summary>
        public readonly TransformProvider TransformProvider;

        /// <summary>
        /// Occurs just before a copy of the current engine's default parameters
        /// are handed off to a newly created <see cref="IView"/> object.
        /// </summary>
        public event ParameterizingViewEventHandler ParameterizingView;

        /// <summary>
        /// Create a new instance of the <see cref="XsltViewEngine"/> class.
        /// </summary>
        /// <param name="transformProvider">A transform provider.</param>
        public XsltViewEngine(TransformProvider transformProvider)
            : this(transformProvider, (XmlFormatterAsync)null, null)
        {
        }

        /// <summary>
        /// Create a new instance of the <see cref="XsltViewEngine"/> class.
        /// </summary>
        /// <param name="transformProvider">A transform provider.</param>
        public XsltViewEngine(ITransformProvider transformProvider)
            : this(transformProvider.GetTransform, (XmlFormatterAsync)null, null)
        {
        }

        /// <summary>
        /// Create a new instance of the <see cref="XsltViewEngine"/> class.
        /// </summary>
        /// <param name="transformProvider">A transform provider.</param>
        /// <param name="formatter">An XML formatter to use when rendering to output.</param>
        public XsltViewEngine(TransformProvider transformProvider, XmlFormatterAsync formatter)
            : this(transformProvider, formatter, null)
        {
        }

        /// <summary>
        /// Create a new instance of the <see cref="XsltViewEngine"/> class.
        /// </summary>
        /// <param name="transformProvider">A transform provider.</param>
        /// <param name="formatter">An XML formatter to use when rendering to output.</param>
        public XsltViewEngine(ITransformProvider transformProvider, XmlFormatterAsync formatter)
            : this(transformProvider.GetTransform, formatter, null)
        {
        }

        /// <summary>
        /// Create a new instance of the <see cref="XsltViewEngine"/> class.
        /// </summary>
        /// <param name="transformProvider">A transform provider.</param>
        /// <param name="formatter">An XML formatter to use when rendering to output.</param>
        public XsltViewEngine(TransformProvider transformProvider, IFormatXml formatter)
            : this(transformProvider, formatter.FormatAsync, null)
        {
        }

        /// <summary>
        /// Create a new instance of the <see cref="XsltViewEngine"/> class.
        /// </summary>
        /// <param name="transformProvider">A transform provider.</param>
        /// <param name="formatter">An XML formatter to use when rendering to output.</param>
        public XsltViewEngine(ITransformProvider transformProvider, IFormatXml formatter)
            : this(transformProvider.GetTransform, formatter.FormatAsync, null)
        {
        }

        /// <summary>
        /// Create a new instance of the <see cref="XsltViewEngine"/> class.
        /// </summary>
        /// <param name="transformProvider">A transform provider.</param>
        /// <param name="defaultParameters">
        /// A collection of overridable parameters used in view rendering.
        /// </param>
        public XsltViewEngine(
            TransformProvider transformProvider,
            IDictionary<string, object> defaultParameters
            )
            : this(transformProvider, (XmlFormatterAsync)null, defaultParameters)
        {
        }

        /// <summary>
        /// Create a new instance of the <see cref="XsltViewEngine"/> class.
        /// </summary>
        /// <param name="transformProvider">A transform provider.</param>
        /// <param name="defaultParameters">
        /// A collection of overridable parameters used in view rendering.
        /// </param>
        public XsltViewEngine(
            ITransformProvider transformProvider,
            IDictionary<string, object> defaultParameters
            )
            : this(transformProvider.GetTransform, (XmlFormatterAsync)null, defaultParameters)
        {
        }

        /// <summary>
        /// Create a new instance of the <see cref="XsltViewEngine"/> class.
        /// </summary>
        /// <param name="transformProvider">A transform provider.</param>
        /// <param name="formatter">An XML formatter to use when rendering to output.</param>
        /// <param name="defaultParameters">
        /// A collection of overridable parameters used in view rendering.
        /// </param>
        public XsltViewEngine(
            TransformProvider transformProvider,
            XmlFormatterAsync formatter,
            IDictionary<string, object> defaultParameters
            )
        {
            DefaultParameters = defaultParameters ?? new Dictionary<string, object>();
            Formatter = formatter;
            TransformProvider = transformProvider;
        }

        /// <summary>
        /// Create a new instance of the <see cref="XsltViewEngine"/> class.
        /// </summary>
        /// <param name="transformProvider">A transform provider.</param>
        /// <param name="formatter">An XML formatter to use when rendering to output.</param>
        /// <param name="defaultParameters">
        /// A collection of overridable parameters used in view rendering.
        /// </param>
        public XsltViewEngine(
            ITransformProvider transformProvider,
            XmlFormatterAsync formatter,
            IDictionary<string, object> defaultParameters
            )
            : this(transformProvider.GetTransform, formatter, defaultParameters)
        {
        }

        /// <summary>
        /// Create a new instance of the <see cref="XsltViewEngine"/> class.
        /// </summary>
        /// <param name="transformProvider">A transform provider.</param>
        /// <param name="formatter">An XML formatter to use when rendering to output.</param>
        /// <param name="defaultParameters">
        /// A collection of overridable parameters used in view rendering.
        /// </param>
        public XsltViewEngine(
            TransformProvider transformProvider,
            IFormatXml formatter,
            IDictionary<string, object> defaultParameters
            )
            : this(transformProvider, formatter.FormatAsync, defaultParameters)
        {
        }

        /// <summary>
        /// Create a new instance of the <see cref="XsltViewEngine"/> class.
        /// </summary>
        /// <param name="transformProvider">A transform provider.</param>
        /// <param name="formatter">An XML formatter to use when rendering to output.</param>
        /// <param name="defaultParameters">
        /// A collection of overridable parameters used in view rendering.
        /// </param>
        public XsltViewEngine(
            ITransformProvider transformProvider,
            IFormatXml formatter,
            IDictionary<string, object> defaultParameters
            )
            : this(transformProvider.GetTransform, formatter.FormatAsync, defaultParameters)
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
            var result = TransformProvider.Invoke(context);

            if (result.HasTransform)
            {
                var parameters = DefaultParameters.Copy();

                if (ParameterizingView != null)
                {
                    ParameterizingView.Invoke(new ParameterizingViewEventArgs
                    {
                        Context = context,
                        Parameters = parameters
                    });
                }

                var view = new XsltView(result.Transform, Formatter, parameters);

                return new ViewEngineResult(view, this);
            }

            return new ViewEngineResult(result.SearchedPaths);
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