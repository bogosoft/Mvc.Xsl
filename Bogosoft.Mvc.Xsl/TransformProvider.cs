using System.Web.Mvc;
using System.Xml.Xsl;

namespace Bogosoft.Mvc.Xsl
{
    /// <summary>
    /// Search for an <see cref="XslCompiledTransform"/> against a given controller context.
    /// </summary>
    /// <param name="context">A controller context.</param>
    /// <returns>
    /// The result of searching for an XSL transform.
    /// </returns>
    public delegate TransformSearchResult TransformProvider(ControllerContext context);
}