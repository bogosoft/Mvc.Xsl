using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Bogosoft.Mvc.Xsl
{
    /// <summary>
    /// Indicates that an implementation is capable of filtering XML nodes.
    /// </summary>
    public interface IXmlFilter
    {
        /// <summary>
        /// Filter a given XML document.
        /// </summary>
        /// <param name="document">A document to filter.</param>
        /// <param name="token">A <see cref="CancellationToken"/> object.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        Task FilterAsync(XmlDocument document, CancellationToken token);
    }
}