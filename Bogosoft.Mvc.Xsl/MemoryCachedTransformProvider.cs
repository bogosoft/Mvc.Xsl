using Result = Bogosoft.Mvc.Xsl.TransformSearchResult;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Web.Mvc;

namespace Bogosoft.Mvc.Xsl
{
    /// <summary>
    /// A concurrent, in-memory caching implementation of the <see cref="ITransformProvider"/> contract.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the key that search results will be cached against.
    /// </typeparam>
    public sealed class MemoryCachedTransformProvider<T> : ITransformProvider
    {
        Dictionary<T, Result> cachedResults = new Dictionary<T, Result>();
        ReaderWriterLockSlim @lock = new ReaderWriterLockSlim();
        Func<ControllerContext, T> selector;
        TransformProvider source;

        /// <summary>
        /// Create a new instance of the <see cref="MemoryCachedTransformProvider{T}"/> class.
        /// </summary>
        /// <param name="source">A transform provider to cache.</param>
        /// <param name="selector">
        /// A strategy for selecting a key of the specified type against a controller context.
        /// </param>
        public MemoryCachedTransformProvider(TransformProvider source, Func<ControllerContext, T> selector)
        {
            this.selector = selector;
            this.source = source;
        }

        /// <summary>
        /// Create a new instance of the <see cref="MemoryCachedTransformProvider{T}"/> class.
        /// </summary>
        /// <param name="source">A transform provider to cache.</param>
        /// <param name="selector">
        /// A strategy for selecting a key of the specified type against a controller context.
        /// </param>
        public MemoryCachedTransformProvider(ITransformProvider source, Func<ControllerContext, T> selector)
        {
            this.selector = selector;
            this.source = source.GetTransform;
        }

        /// <summary>
        /// Search for an <see cref="XslCompiledTransform"/> against a given controller context.
        /// </summary>
        /// <param name="context">A controller context.</param>
        /// <returns>
        /// The result of searching for an XSL transform.
        /// </returns>
        public Result GetTransform(ControllerContext context)
        {
            var key = selector.Invoke(context);

            @lock.EnterUpgradeableReadLock();

            try
            {
                if (cachedResults.ContainsKey(key))
                {
                    return cachedResults[key];
                }

                @lock.EnterWriteLock();

                try
                {
                    var result = source.Invoke(context);

                    if (result.HasTransform)
                    {
                        cachedResults[key] = result;
                    }

                    return result;
                }
                finally
                {
                    @lock.ExitWriteLock();
                }
            }
            finally
            {
                @lock.ExitUpgradeableReadLock();
            }
        }
    }
}