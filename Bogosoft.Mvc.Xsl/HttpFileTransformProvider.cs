using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Xsl;

namespace Bogosoft.Mvc.Xsl
{
    /// <summary>
    /// An HTTP-based implementation of the <see cref="ITransformProvider"/> contract.
    /// </summary>
    public sealed class HttpFileTransformProvider : ITransformProvider
    {
        PathFormatter formatter;

        /// <summary>
        /// Create a new instance of the <see cref="HttpFileTransformProvider"/> class.
        /// </summary>
        /// <param name="formatter">
        /// A strategy for converting a controller context to a URL.
        /// </param>
        public HttpFileTransformProvider(PathFormatter formatter)
        {
            this.formatter = formatter;
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
            Uri uri;

            XslCompiledTransform transform = null;

            if(Uri.TryCreate(formatter.Invoke(context), UriKind.Absolute, out uri))
            {
                transform = GetTransformAsync(uri).GetAwaiter().GetResult();
            }

            return new TransformSearchResult(uri.ToString(), transform);
        }

        static async Task<XslCompiledTransform> GetTransformAsync(Uri uri)
        {
            using (var client = new HttpClient())
            using (var response = await client.GetAsync(uri))
            {
                if(response.StatusCode == HttpStatusCode.OK)
                {
                    using (var source = await response.Content.ReadAsStreamAsync())
                    using (var reader = XmlReader.Create(source))
                    {
                        var transform = new XslCompiledTransform();

                        transform.Load(reader);

                        return transform;
                    }
                }
                else
                {
                    return null;
                }
            }
        }
    }
}