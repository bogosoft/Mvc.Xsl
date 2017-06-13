using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Bogosoft.Mvc.Xsl
{
    /// <summary>
    /// Filter a given node and its descendants if any.
    /// </summary>
    /// <param name="node">A node to begin filtering on.</param>
    /// <param name="token">A <see cref="CancellationToken"/> object.</param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    public delegate Task XmlFilterAsync(XmlNode node, CancellationToken token);
}