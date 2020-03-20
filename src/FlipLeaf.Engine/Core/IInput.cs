using System.IO;

namespace FlipLeaf.Core
{
    /// <summary>
    /// Defines an input, a source of content which will be transformed into a static resource.
    /// Typical implementation is a <see cref="Inputs.FileInput"/>, which represent a physical file.
    /// </summary>
    public interface IInput
    {
        string Extension { get; }

        /// <summary>
        /// Gets the name of the input, relative to the site root, including directories.
        /// </summary>
        /// <remarks>
        /// The relative name can be different from the physical location of the source (in the case of the file).
        /// The <see cref="Open"/> method will ensure that the content can be accessed.
        /// </remarks>
        string RelativeName { get; }

        /// <summary>
        /// Gets the absolute physical path of the input, when available.
        /// </summary>
        /// <remarks>
        /// This property can return null when there is no physical location for the input (in the case of a generated file for example).
        /// </remarks>
        string Path { get; }

        /// <summary>
        /// Opens the input to read the content.
        /// </summary>
        Stream Open(IWebSite ctx);
    }

    public static class InputExtensions
    {
        public static string GetFullInputPath(this IInput @this, IWebSite context) => context.GetFullInputPath(@this.Path ?? @this.RelativeName);

        public static string GetFullOuputPath(this IInput @this, IWebSite context) => context.GetFullOutputPath(@this.RelativeName);
    }
}
