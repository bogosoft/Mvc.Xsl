using System.Xml.Xsl;

namespace Bogosoft.Mvc.Xsl
{
    /// <summary>
    /// Gets an <see cref="XslCompiledTransform"/> based on a given filepath.
    /// </summary>
    /// <param name="filepath">
    /// A filepath corresponding to an XSLT.
    /// </param>
    /// <returns>
    /// An <see cref="XslCompiledTransform"/> object.
    /// </returns>
    public delegate XslCompiledTransform XslTransformProvider(string filepath);
}