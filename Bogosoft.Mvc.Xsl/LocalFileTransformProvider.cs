using System.IO;
using System.Web.Mvc;
using System.Xml.Xsl;

namespace Bogosoft.Mvc.Xsl
{
    /// <summary>
    /// A local filesystem-based implementation of the <see cref="ITransformProvider"/> contract.
    /// </summary>
    public class LocalFileTransformProvider : ITransformProvider
    {
        /// <summary>
        /// Get or set a filepath formatter associated with the current transform provider.
        /// </summary>
        protected PathFormatter Formatter;

        /// <summary>
        /// Create a new instance of the <see cref="LocalFileTransformProvider"/> class.
        /// </summary>
        /// <param name="formatter">
        /// A strategy for converting a controller context to a local filepath.
        /// </param>
        public LocalFileTransformProvider(PathFormatter formatter)
        {
            Formatter = formatter;
        }

        /// <summary>
        /// Search for an <see cref="XslCompiledTransform"/> against a given controller context.
        /// </summary>
        /// <param name="context">A controller context.</param>
        /// <returns>
        /// The result of searching for an XSL transform.
        /// </returns>
        public virtual TransformSearchResult GetTransform(ControllerContext context)
        {
            var filepath = Formatter.Invoke(context);

            XslCompiledTransform transform = null;

            if (File.Exists(filepath))
            {
                transform = new XslCompiledTransform();

                transform.Load(filepath);
            }

            return new TransformSearchResult(filepath, transform);
        }
    }
}