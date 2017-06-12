using System.Web.Mvc;

namespace Bogosoft.Mvc.Xsl
{
    /// <summary>
    /// Generates a filepath from a given controller context.
    /// </summary>
    /// <param name="context">
    /// A controller context to generate a filepath against.
    /// </param>
    /// <returns>
    /// A value corresponding to the path of a file.
    /// </returns>
    public delegate string PathFormatter(ControllerContext context);
}