using System.Xml.Xsl;

namespace Bogosoft.Mvc.Xsl
{
    /// <summary>
    /// A filesystem-based implementation of the <see cref="ITransformProvider"/> contract.
    /// </summary>
    public class FileTransformProvider : ITransformProvider
    {
        /// <summary>
        /// Provision an XSL transform against a given filepath.
        /// </summary>
        /// <param name="filepath">
        /// A filepath corresponding to an XSLT.
        /// </param>
        /// <returns>
        /// An <see cref="XslCompiledTransform"/> object.
        /// </returns>
        public XslCompiledTransform GetTransform(string filepath)
        {
            var processor = new XslCompiledTransform();

            processor.Load(filepath);

            return processor;
        }
    }
}