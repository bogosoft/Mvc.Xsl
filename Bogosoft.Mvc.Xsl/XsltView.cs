using Bogosoft.Mapping;
using Bogosoft.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Xsl;

namespace Bogosoft.Mvc.Xsl
{
    internal class XsltView : IView
    {
        protected IDictionary<String, Object> Parameters;
        protected XmlFormatter Formatter;
        protected XslCompiledTransform Processor;

        internal XsltView(
            XslCompiledTransform processor,
            XmlFormatter formatter,
            IDictionary<String, Object> parameters
            )
        {
            this.Parameters = parameters;
            this.Formatter = formatter;
            this.Processor = processor;
        }

        public void Render(ViewContext context, TextWriter writer)
        {
            XmlDocument source, transformed;

            // Prepare model
            var model = context.ViewData.Model;

            if(model is XmlDocument)
            {
                source = (model as XmlDocument);
            }
            else if(model is IMap<Object, XmlDocument>)
            {
                source = (model as IMap<Object, XmlDocument>).Map(model);
            }
            else
            {
                source = new XmlDocument();
            }

            // Prepare parameters
            foreach(var kv in context.ViewData)
            {
                this.Parameters[kv.Key] = kv.Value;
            }

            var args = new XsltArgumentList();

            foreach(var kv in this.Parameters)
            {
                args.AddParam(kv.Key, "", kv.Value);
            }

            using (var stream = new MemoryStream())
            {
                this.Processor.Transform(source, args, stream);

                stream.Position = 0;

                transformed = new XmlDocument();

                transformed.Load(stream);
            }

            this.Formatter.Format(transformed, writer);
        }
    }
}