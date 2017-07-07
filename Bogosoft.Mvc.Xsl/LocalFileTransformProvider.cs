using System.IO;
using System.Web.Mvc;
using System.Xml.Xsl;

namespace Bogosoft.Mvc.Xsl
{
    /// <summary>
    /// A local filesystem-based implementation of the <see cref="ITransformProvider"/> contract.
    /// </summary>
    public sealed class LocalFileTransformProvider : ITransformProvider
    {
        PathFormatter formatter;

        /// <summary>
        /// Create a new instance of the <see cref="LocalFileTransformProvider"/> class.
        /// </summary>
        /// <param name="formatter">
        /// A strategy for converting a controller context to a local filepath.
        /// </param>
        public LocalFileTransformProvider(PathFormatter formatter)
        {
            this.formatter = formatter;
        }

        /// <summary>
        /// Search for an <see cref="XslCompiledTransform"/> against a given controller context.
        /// </summary>
        /// <param name="context">A controller context.</param>
        /// <returns>
        /// The result of searching for an XSL transform.
        /// </returns>
        public TransformSearchResult GetTransform(ControllerContext context)
        {
            var filepath = formatter.Invoke(context);

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