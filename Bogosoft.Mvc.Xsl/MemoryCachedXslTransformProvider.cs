using System.Collections.Generic;
using System.Threading;
using System.Xml.Xsl;

namespace Bogosoft.Mvc.Xsl
{
    /// <summary>
    /// An in-memory caching implementation of the <see cref="IXslTransformProvider"/> contract.
    /// </summary>
    public sealed class MemoryCachedXslTransformProvider : IXslTransformProvider
    {
        readonly Dictionary<string, XslCompiledTransform> cache;
        ReaderWriterLockSlim @lock = new ReaderWriterLockSlim();
        IXslTransformProvider source;

        /// <summary>
        /// Create a new <see cref="MemoryCachedXslTransformProvider"/> instance.
        /// </summary>
        /// <param name="source">
        /// A source XSL transform provider to cache results from.
        /// </param>
        public MemoryCachedXslTransformProvider(IXslTransformProvider source)
        {
            this.source = source;

            cache = new Dictionary<string, XslCompiledTransform>();
        }

        /// <summary>
        /// Provision an XSL transform against a given filepath.
        /// </summary>
        /// <param name="filepath">
        /// A filepath corresponding to an XSLT.
        /// </param>
        /// <returns>
        /// An <see cref="XslCompiledTransform"/> object.
        /// </returns>
        public XslCompiledTransform Provision(string filepath)
        {
            @lock.EnterUpgradeableReadLock();

            try
            {
                if (!cache.ContainsKey(filepath))
                {
                    @lock.EnterWriteLock();

                    try
                    {
                        cache[filepath] = source.Provision(filepath);
                    }
                    finally
                    {
                        @lock.ExitWriteLock();
                    }
                }

                return cache[filepath];
            }
            finally
            {
                @lock.ExitUpgradeableReadLock();
            }
        }
    }
}