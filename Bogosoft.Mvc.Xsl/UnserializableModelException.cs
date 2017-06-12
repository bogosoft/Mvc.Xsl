using System;

namespace Bogosoft.Mvc.Xsl
{
    /// <summary>
    /// Represents an error arising from attempting to serialize an object to an XPath-navigable
    /// document when no serialization strategy for that object can be found.
    /// </summary>
    public class UnserializableModelException : Exception
    {
        /// <summary>
        /// Get or set the type of the object that could not be serialized.
        /// </summary>
        protected Type Type;

        /// <summary>
        /// Get the message associated with the current exception.
        /// </summary>
        public override string Message => $"Unable to serialize object of type, '{Type}', to XML.";

        /// <summary>
        /// Construct a new instance of the <see cref="UnserializableModelException"/>.
        /// </summary>
        /// <param name="type"></param>
        public UnserializableModelException(Type type)
        {
            Type = type;
        }
    }
}