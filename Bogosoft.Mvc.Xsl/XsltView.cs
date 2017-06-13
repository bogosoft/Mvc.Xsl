using Bogosoft.Xml;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Bogosoft.Mvc.Xsl
{
    class XsltView : IView
    {
        IDictionary<string, object> parameters;
        XmlFilterAsync[] filters;
        XmlFormatterAsync formatter;
        XslCompiledTransform processor;

        internal XsltView(
            XslCompiledTransform processor,
            XmlFilterAsync[] filters,
            XmlFormatterAsync formatter,
            IDictionary<string, object> parameters
            )
        {
            this.parameters = parameters;
            this.filters = filters;
            this.formatter = formatter;
            this.processor = processor;
        }

        public void Render(ViewContext context, TextWriter writer)
        {
            Task.Run(async () => await RenderAsync(context, writer)).Wait();
        }

        async Task RenderAsync(ViewContext context, TextWriter writer)
        {
            var token = context.HttpContext.Request.TimedOutToken;

            token.ThrowIfCancellationRequested();

            IXPathNavigable source;

            var model = context.ViewData.Model;

            if(ReferenceEquals(null, model))
            {
                source = new XmlDocument();
            }
            else if(model is IXmlSerializable)
            {
                source = (model as IXmlSerializable).Serialize();
            }
            else if(model is IEnumerable<IXmlSerializable>)
            {
                source = (model as IEnumerable<IXmlSerializable>).Serialize();
            }
            else
            {
                throw new UnserializableModelException(model.GetType());
            }

            foreach(var kv in context.ViewData)
            {
                parameters[kv.Key] = kv.Value;
            }

            var args = new XsltArgumentList();

            foreach (var kv in parameters)
            {
                args.AddParam(kv.Key, "", kv.Value);
            }

            if(formatter == null)
            {
                processor.Transform(source, args, writer);

                return;
            }

            XmlDocument transformed;

            using (var stream = new MemoryStream())
            {
                processor.Transform(source, args, stream);

                stream.Position = 0;

                transformed = new XmlDocument();

                transformed.Load(stream);
            }

            foreach(var x in filters)
            {
                await x.Invoke(transformed, token);
            }

            await formatter.Invoke(transformed, writer, token);
        }
    }
}