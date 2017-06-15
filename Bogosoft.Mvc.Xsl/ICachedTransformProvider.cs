namespace Bogosoft.Mvc.Xsl
{
    /// <summary>
    /// Indicates that an implementation is capable of providing cached XSL transforms.
    /// </summary>
    public interface ICachedTransformProvider : ITransformProvider
    {
        /// <summary>
        /// Clear the current cache of all cached items.
        /// </summary>
        void Clear();
    }
}