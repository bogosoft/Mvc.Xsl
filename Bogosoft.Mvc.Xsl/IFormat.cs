using System.IO;
using System.Xml;

namespace Bogosoft.Mvc.Xsl
{
    /// <summary>
    /// Provides a means of formatting an <see cref="XmlDocument"/>
    /// while outputting to a <see cref="TextWriter"/>.
    /// </summary>
    public interface IFormat
    {
        /// <summary>Format an <see cref="XmlDocument"/> to a <see cref="TextWriter"/>.</summary>
        /// <param name="document">An <see cref="XmlDocument"/> to format.</param>
        /// <param name="writer">A <see cref="TextWriter"/> to format to.</param>
        void Format(XmlDocument document, TextWriter writer);
    }
}