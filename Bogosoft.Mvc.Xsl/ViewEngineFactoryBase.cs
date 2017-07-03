using Bogosoft.Xml;
using System.Collections.Generic;

namespace Bogosoft.Mvc.Xsl
{
    /// <summary>
    /// Base functionality for configuring an XSLT view engine for rendering.
    /// </summary>
    public abstract class ViewEngineFactoryBase
    {
        /// <summary>
        /// Get a collection of default parameters as key-value pairs.
        /// </summary>
        protected abstract IDictionary<string, object> DefaultParameters { get; }

        /// <summary>
        /// Get a collection of filters to be applied to an XML document before rendering.
        /// </summary>
        protected abstract IEnumerable<IFilterXml> Filters { get; }

        /// <summary>
        /// Get a formatter to be used when rendering the result of the XSL transform to output.
        /// </summary>
        protected abstract StandardXmlFormatter Formatter { get; }

        /// <summary>
        /// Get an XSL transform provider.
        /// </summary>
        protected abstract ITransformProvider TransformProvider { get; }

        /// <summary>
        /// Configure a given collection of view engines.
        /// </summary>
        /// <returns>
        /// A collection of objects used by the XSLT view infrastructure that may also be
        /// of importance outside of this configuration process.
        /// </returns>
        public virtual XsltViewEngine Build()
        {
            return new XsltViewEngine(TransformProvider, Formatter.With(Filters), DefaultParameters);
        }
    }
}