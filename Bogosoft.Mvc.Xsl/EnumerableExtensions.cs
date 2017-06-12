using Bogosoft.Xml;
using System.Collections.Generic;
using System.Xml;

namespace Bogosoft.Mvc.Xsl
{
    static class EnumerableExtensions
    {
        internal static XmlDocument Serialize(this IEnumerable<IXmlSerializable> items)
        {
            var document = new XmlDocument();

            var root = document.CreateElement("collection");

            using (var enumerator = items.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    root.SetAttribute("of", enumerator.Current.GetType().Name);

                    enumerator.Current.SerializeTo(root);
                }

                while (enumerator.MoveNext())
                {
                    enumerator.Current.SerializeTo(root);
                }
            }

            return document;
        }
    }
}