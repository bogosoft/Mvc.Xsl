using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Xsl;

namespace Bogosoft.Mvc.Xsl
{
    /// <summary>
    /// A composite implementation of the <see cref="ITransformProvider"/> contract which wraps
    /// zero of more transfrom providers.
    /// </summary>
    public sealed class CompositeTransformProvider : ITransformProvider
    {
        TransformProvider[] providers;

        /// <summary>
        /// Create a new instance of the <see cref="CompositeTransformProvider"/> class.
        /// </summary>
        /// <param name="provider">A transfrom provider.</param>
        public CompositeTransformProvider(TransformProvider provider)
        {
            providers = new TransformProvider[] { provider };
        }

        /// <summary>
        /// Create a new instance of the <see cref="CompositeTransformProvider"/> class.
        /// </summary>
        /// <param name="provider">A transfrom provider.</param>
        public CompositeTransformProvider(ITransformProvider provider)
        {
            providers = new TransformProvider[] { provider.GetTransform };
        }

        /// <summary>
        /// Create a new instance of the <see cref="CompositeTransformProvider"/> class.
        /// </summary>
        /// <param name="providers">A collection of transfrom providers.</param>
        public CompositeTransformProvider(IEnumerable<TransformProvider> providers)
        {
            this.providers = providers.ToArray();
        }

        /// <summary>
        /// Create a new instance of the <see cref="CompositeTransformProvider"/> class.
        /// </summary>
        /// <param name="providers">A collection of transfrom providers.</param>
        public CompositeTransformProvider(IEnumerable<ITransformProvider> providers)
        {
            this.providers = providers.Select<ITransformProvider, TransformProvider>(x => x.GetTransform).ToArray();
        }

        /// <summary>
        /// Search for an <see cref="XslCompiledTransform"/> against a given controller context.
        /// </summary>
        /// <param name="context">A controller context.</param>
        /// <returns>
        /// The result of searching for an XSL transform.
        /// </returns>
        public TransformSearchResult GetTransform(ControllerContext context)
        {
            var searched = new List<string>();

            XslCompiledTransform transform = null;

            foreach (var x in providers)
            {
                var result = x.Invoke(context);

                searched.Add(result.SearchedPaths);

                if (result.HasTransform)
                {
                    transform = result.Transform;

                    break;
                }
            }

            return new TransformSearchResult(searched, transform);
        }
    }
}