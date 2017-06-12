﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Xsl;

namespace Bogosoft.Mvc.Xsl
{
    /// <summary>
    /// An in-memory caching implementation of the <see cref="IXslTransformProvider"/> contract.
    /// </summary>
    public sealed class MemoryCachedXslTransformProvider : IDisposable, IXslTransformProvider
    {
        readonly Dictionary<string, XslCompiledTransform> cache;
        ReaderWriterLockSlim @lock = new ReaderWriterLockSlim();
        IXslTransformProvider source;
        Dictionary<string, FileSystemWatcher> watchers;
        bool watchForChanges;

        /// <summary>
        /// Create a new <see cref="MemoryCachedXslTransformProvider"/> instance.
        /// </summary>
        /// <param name="source">
        /// A source XSL transform provider to cache results from.
        /// </param>
        /// <param name="watchForChanges">
        /// A value indicating whether or not the current provider is to watch
        /// for changes in files that correspond to cached XSL transforms.
        /// </param>
        public MemoryCachedXslTransformProvider(IXslTransformProvider source, bool watchForChanges = false)
        {
            this.source = source;

            cache = new Dictionary<string, XslCompiledTransform>();

            watchers = new Dictionary<string, FileSystemWatcher>();

            this.watchForChanges = watchForChanges;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing,
        /// or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            foreach(var x in watchers.Select(x => x.Value))
            {
                x.Dispose();
            }
        }

        void FileSystemWatcher_OnChanged(object sender, FileSystemEventArgs args)
        {
            @lock.EnterWriteLock();

            try
            {
                watchers[args.FullPath].Dispose();

                cache.Remove(args.FullPath);
            }
            finally
            {
                @lock.ExitWriteLock();
            }
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

                        if(watchForChanges && File.Exists(filepath))
                        {
                            Watch(filepath);
                        }
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

        void Watch(string filepath)
        {
            var watcher = new FileSystemWatcher();

            watcher.Changed += FileSystemWatcher_OnChanged;
            watcher.Filter = Path.GetFileName(filepath);
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Path = Path.GetDirectoryName(filepath);

            watcher.EnableRaisingEvents = true;

            watchers[filepath] = watcher;
        }
    }
}