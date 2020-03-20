using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html.Inlines;
using Markdig.Syntax.Inlines;

namespace FlipLeaf.Core.Text.MarkdigSpecifics
{
    public class CustomLinkInlineRendererExtension : IMarkdownExtension
    {
        private readonly IWebSite _site;

        public CustomLinkInlineRendererExtension(IWebSite site)
        {
            _site = site;
        }

        public void Setup(MarkdownPipelineBuilder pipeline) { }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            renderer.ObjectRenderers.Replace<LinkInlineRenderer>(new CustomLinkInlineRenderer(_site.Configuration.BaseUrl));
        }
    }

    public class CustomLinkInlineRenderer : LinkInlineRenderer
    {
        private string _baseUrl;
        private bool _hasBaseUrl;

        public CustomLinkInlineRenderer(string baseUrl)
        {
            _hasBaseUrl = !string.IsNullOrEmpty(baseUrl);
            _baseUrl = baseUrl;
        }

        protected override void Write(HtmlRenderer renderer, LinkInline link)
        {
            link.Url = PrependBasePath(link.Url);
            // quick hack to transform links into their html counterpart
            //
            // should be replaced with a more robust solution to handle extension transformation
            // (moreover if pretty urls must be supported)
            link.Url = link.Url.Replace(".md", ".html");

            base.Write(renderer, link);
        }

        internal string PrependBasePath(string url)
        {
            if (!_hasBaseUrl)
            {
                return url;
            }

            if (url.Length == 0 || url[0] != '/')
            {
                return url;
            }


            return _baseUrl + url;
        }
    }
}
