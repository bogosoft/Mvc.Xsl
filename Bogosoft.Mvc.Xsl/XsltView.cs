using Bogosoft.Mapping;
using Bogosoft.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Xsl;

namespace Bogosoft.Mvc.Xsl
{
    internal class XsltView : IView
    {
        protected IDictionary<string, object> Parameters;
        protected XmlFormatterAsync Formatter;
        protected XslCompiledTransform Processor;

        internal XsltView(
            XslCompiledTransform processor,
            XmlFormatterAsync formatter,
            IDictionary<string, object> parameters
            )
        {
            Parameters = parameters;
            Formatter = formatter;
            Processor = processor;
        }

        public void Render(ViewContext context, TextWriter writer)
        {
            Task.Run(async () => await RenderAsync(context, writer)).Wait();
        }

        async Task RenderAsync(ViewContext context, TextWriter writer)
        {
            var token = context.HttpContext.Request.TimedOutToken;

            token.ThrowIfCancellationRequested();

            XmlDocument source, transformed;

            // Prepare model
            var model = context.ViewData.Model;

            if (model is XmlDocument)
            {
                source = (model as XmlDocument);
            }
            else if (model is IMap<Object, XmlDocument>)
            {
                source = (model as IMap<Object, XmlDocument>).Map(model);
            }
            else
            {
                source = new XmlDocument();
            }

            // Prepare parameters
            foreach (var kv in context.ViewData)
            {
                Parameters[kv.Key] = kv.Value;
            }

            var args = new XsltArgumentList();

            foreach (var kv in this.Parameters)
            {
                args.AddParam(kv.Key, "", kv.Value);
            }

            using (var stream = new MemoryStream())
            {
                Processor.Transform(source, args, stream);

                stream.Position = 0;

                transformed = new XmlDocument();

                transformed.Load(stream);
            }

            await Formatter.Invoke(transformed, writer, token);
        }
    }
}