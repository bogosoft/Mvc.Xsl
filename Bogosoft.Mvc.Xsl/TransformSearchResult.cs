using System;
using System.Collections.Generic;
using System.Xml.Xsl;

namespace Bogosoft.Mvc.Xsl
{
    /// <summary>
    /// Represents an attempt to search for an XSL transform.
    /// </summary>
    public struct TransformSearchResult
    {
        IEnumerable<string> searchedPaths;
        XslCompiledTransform transform;

        /// <summary>
        /// Get a value indicating whether or not the search successfully returned an XSL transform.
        /// </summary>
        public bool HasTransform => transform != null;

        /// <summary>
        /// Get a value corresponding to the path searched for an XSL transform.
        /// </summary>
        public IEnumerable<string> SearchedPaths => searchedPaths;

        /// <summary>
        /// Attempt to get the XSL transform associated with the current search result.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown in the event that this method is called after a failed search.
        /// </exception>
        public XslCompiledTransform Transform
        {
            get
            {
                if(transform == null)
                {
                    throw new InvalidOperationException("This search result contains no XSL transform.");
                }

                return transform;
            }
        }

        /// <summary>
        /// Create a new <see cref="TransformSearchResult"/> with an optional XSL transform.
        /// </summary>
        /// <param name="searchedPath">
        /// A value corresponding to the path searched for an XSL transform.
        /// </param>
        /// <param name="transform">An XSL transform.</param>
        public TransformSearchResult(string searchedPath, XslCompiledTransform transform = null)
        {
            searchedPaths = new string[] { searchedPath };

            this.transform = transform;
        }

        /// <summary>
        /// Create a new <see cref="TransformSearchResult"/> with an optional XSL transform.
        /// </summary>
        /// <param name="searchedPaths">
        /// A collection of paths searched for an XSL transform.
        /// </param>
        /// <param name="transform">An XSL transform.</param>
        public TransformSearchResult(IEnumerable<string> searchedPaths, XslCompiledTransform transform = null)
        {
            this.searchedPaths = searchedPaths;
            this.transform = transform;
        }
    }
}