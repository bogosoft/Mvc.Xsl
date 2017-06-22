using System.Collections.Generic;
using System.Web.Mvc;

namespace Bogosoft.Mvc.Xsl
{
    /// <summary>
    /// Contains properties relating to view parameterizing events.
    /// </summary>
    public sealed class ParameterizingViewEventArgs
    {
        /// <summary>
        /// Get or set the controller context associated with the current event.
        /// </summary>
        public ControllerContext Context;

        /// <summary>
        /// Get or set the collection of parameters associated with the current event.
        /// </summary>
        public IDictionary<string, object> Parameters;
    }
}