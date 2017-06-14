using System.Xml.Xsl;

namespace Bogosoft.Mvc.Xsl
{
    /// <summary>
    /// Indicates that an implementation is capable of providing <see cref="XslCompiledTransform"/> objects.
    /// </summary>
    public interface ITransformProvider
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
        XslCompiledTransform GetTransform(string filepath);
    }
}