namespace FlipLeaf.Core
{
    /// <summary>
    /// Defines a context specific to the processing of an <see cref="IInput"/>.
    /// </summary>
    public interface IInputContext
    {
        IInput Input { get; }

        /// <summary>
        /// Gets the full input path.
        /// </summary>
        string InputPath { get; }

        /// <summary>
        /// Gets or sets the path where the output must be written.
        /// </summary>
        string OutputPath { get; set; }

        /// <summary>
        /// Gets the global context of the site.
        /// </summary>
        IWebSite Site { get; }
    }
}
