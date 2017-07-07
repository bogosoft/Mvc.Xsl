using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Web.Mvc;
using System.Xml.Xsl;

namespace Bogosoft.Mvc.Xsl
{
    /// <summary>
    /// A memory-cached local filesystem implementation of the <see cref="ITransformProvider"/> contract.
    /// </summary>
    public sealed class CachedLocalFileTransformProvider : LocalFileTransformProvider, IDisposable
    {
        Dictionary<string, TransformSearchResult> cachedResults = new Dictionary<string, TransformSearchResult>();
        ReaderWriterLockSlim @lock = new ReaderWriterLockSlim();
        bool respondToSourceChanges;
        Dictionary<string, FileSystemWatcher> watchers = new Dictionary<string, FileSystemWatcher>();

        /// <summary>
        /// Create a new instance of the <see cref="CachedLocalFileTransformProvider"/> class.
        /// </summary>
        /// <param name="formatter">
        /// A strategy for converting a controller context to a local filepath.
        /// </param>
        /// <param name="respondToSourceChanges">
        /// A value indicating whether or not changes to XSLT files should be watched for
        /// so that cached transforms do not become stale.
        /// </param>
        public CachedLocalFileTransformProvider(PathFormatter formatter, bool respondToSourceChanges)
            : base(formatter)
        {
            this.respondToSourceChanges = respondToSourceChanges;
        }

        /// <summary>
        /// Dispose of any file system watchers associated with the current transform provider
        /// that have not already been disposed of, if any.
        /// </summary>
        public void Dispose()
        {
            foreach(var x in watchers.Values)
            {
                x.Dispose();
            }
        }

        void FileSystemWatcher_Changed(object sender, FileSystemEventArgs args)
        {
            @lock.EnterReadLock();

            try
            {
                if (cachedResults.ContainsKey(args.FullPath))
                {
                    @lock.EnterWriteLock();

                    try
                    {
                        cachedResults.Remove(args.FullPath);
                    }
                    finally
                    {
                        @lock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                @lock.ExitUpgradeableReadLock();
            }
        }

        /// <summary>
        /// Search for an <see cref="XslCompiledTransform"/> against a given controller context.
        /// </summary>
        /// <param name="context">A controller context.</param>
        /// <returns>
        /// The result of searching for an XSL transform.
        /// </returns>
        public override TransformSearchResult GetTransform(ControllerContext context)
        {
            var filepath = Formatter.Invoke(context);

            @lock.EnterUpgradeableReadLock();

            try
            {
                if (cachedResults.ContainsKey(filepath))
                {
                    return cachedResults[filepath];
                }

                @lock.EnterWriteLock();

                try
                {
                    var result = base.GetTransform(context);

                    if (result.HasTransform)
                    {
                        cachedResults[filepath] = result;

                        var directory = Path.GetDirectoryName(filepath);
                        var filename = Path.GetFileName(filepath);

                        if (respondToSourceChanges && !watchers.ContainsKey(filepath))
                        {
                            var watcher = new FileSystemWatcher(directory, filename);

                            watcher.Changed += FileSystemWatcher_Changed;

                            watcher.EnableRaisingEvents = true;
                        }
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