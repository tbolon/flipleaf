using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using Fluid;

namespace FlipLeaf.Core.Text.FluidLiquid
{
    public class FluidParser
    {
        internal const string BodyAmbientValueKey = "body";

        private readonly ConcurrentDictionary<string, Task<ViewTemplate>> _layoutCache = new ConcurrentDictionary<string, Task<ViewTemplate>>();
        private readonly IWebSite _ctx;

        public FluidParser(IWebSite ctx)
        {
            _ctx = ctx;
        }

        public ViewTemplate ParseTemplate(string content)
        {
            if (!ViewTemplate.TryParse(content, out var template))
            {
                throw new ParseException();
            }

            return template;
        }

        public TemplateContext PrepareContext(InputItems pageContext)
        {
            TemplateContext context = null;

            context = new TemplateContext { MemberAccessStrategy = new IgnoreCaseMemberAccessStrategy() };
            context.Filters.AddAsyncFilter("relative_url", FlipLeafFilters.RelativeUrl);
            context.FileProvider = new FlipLeafFileProvider(_ctx);
            context.MemberAccessStrategy.Register<WebSiteConfiguration>();
            context.MemberAccessStrategy.Register<WebSite>();
            context.SetValue("page", pageContext);
            context.SetValue("site", _ctx.Runtime);


            return context;
        }

        public Task<string> ParseContextAsync(ViewTemplate template, TemplateContext templateContext) => template.RenderAsync(templateContext);

        public async Task<string> ApplyLayoutAsync(string source, TemplateContext context)
        {
            var page = context.GetValue("page");

            var layout = await page.GetValueAsync("layout", context).ConfigureAwait(false);

            var layoutFile = layout.ToStringValue();
            if (string.IsNullOrEmpty(layoutFile))
            {
                return source;
            }

            if (string.IsNullOrEmpty(Path.GetExtension(layoutFile)))
            {
                layoutFile += ".html";
            }

            var layoutTemplate = await LoadLayout(layoutFile);

            context.AmbientValues.Add(BodyAmbientValueKey, source);

            try
            {
                source = await layoutTemplate.RenderAsync(context).ConfigureAwait(false);
            }
            finally
            {
                context.AmbientValues.Remove(BodyAmbientValueKey);
            }

            return source;
        }

        private Task<ViewTemplate> LoadLayout(string fileName) => _layoutCache.GetOrAdd(fileName, CreateLayout);

        private async Task<ViewTemplate> CreateLayout(string fileName)
        {
            string layoutText;
            using (var reader = new StreamReader(Path.Combine(_ctx.GetFullRootPath(_ctx.Configuration.LayoutDir), fileName)))
            {
                layoutText = await reader.ReadToEndAsync().ConfigureAwait(false);
            }

            if (!ViewTemplate.TryParse(layoutText, out var layoutTemplate))
            {
                throw new ParseException();
            }

            return layoutTemplate;
        }
    }
}
