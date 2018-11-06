namespace FlipLeaf.Core.Text.ScrewturnWiki
{
    /// <summary>
    /// Lists formatting contexts.
    /// </summary>
    public enum FormattingContext
    {
        /// <summary>
        /// The overall header.
        /// </summary>
        Header,
        /// <summary>
        /// The overall footer.
        /// </summary>
        Footer,
        /// <summary>
        /// The sidebar.
        /// </summary>
        Sidebar,
        /// <summary>
        /// The page header.
        /// </summary>
        PageHeader,
        /// <summary>
        /// The page footer.
        /// </summary>
        PageFooter,
        /// <summary>
        /// The page content.
        /// </summary>
        PageContent,
        /// <summary>
        /// Transcluded page content.
        /// </summary>
        TranscludedPageContent,
        /// <summary>
        /// The body of a message.
        /// </summary>
        MessageBody,
        /// <summary>
        /// Any other context.
        /// </summary>
        Other,
        /// <summary>
        /// No know context.
        /// </summary>
        Unknown
    }
}
