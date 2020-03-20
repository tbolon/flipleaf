
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

using Serilog;

namespace FlipLeaf
{
    internal class Program
    {
        private static Task<int> Main(string[] args)
        {
            // setup commandline app
            var app = new CommandLineApplication();
            app.HelpOption("-? | -h | --help");
            var inputDir = app.Option("-i | --input", "Path to root site directory. By default current directory is used", CommandOptionType.SingleValue);

            // default action : generate static site
            app.OnExecuteAsync(async c =>
            {
                // load config
                var site = inputDir.HasValue() ? new WebSite(inputDir.Value()) : new WebSite();
                site.LoadConfiguration();

                // todo apply command-line args to runtime

                // configure content source
                var contentSource = new Core.Inputs.FileInputSource(site.InputDirectory) { Exclude = new[] { "README.md" } };

                // configure static source
                var staticsource = new Core.Inputs.FileInputSource(site.GetFullRootPath("_static"));

                // setup markdown processing pipeline
                var parser = new Core.Text.MarkdigSpecifics.MarkdigParser();
                parser.Use(new Core.Text.MarkdigSpecifics.WikiLinkExtension() { Extension = ".md" });
                parser.Use(new Core.Text.MarkdigSpecifics.CustomLinkInlineRendererExtension(site));
                site.AddPipeline(contentSource, new Core.Pipelines.TextTransformPipeline(
                    i => i.Extension == ".md",
                    // read
                    new Core.Text.ReadContentMiddleware(),
                    // prepare
                    new Core.Text.ITextMiddleware[] {
                        new Core.Text.YamlHeaderMiddleware(new Core.Text.YamlParser())
                    },
                    // transform
                    new Core.Text.ITextMiddleware[] {
                        new Core.Text.YamlHeaderMiddleware(new Core.Text.YamlParser()),
                        new Core.Text.LiquidMiddleware(new Core.Text.FluidLiquid.FluidParser(site)),
                        new Core.Text.MarkdownMiddleware(parser)
                    },
                    // write
                    new Core.Text.WriteContentMiddleware() { Extension = ".html" }
                ));

                // setup static files pipeline
                site.AddPipeline(staticsource, new Core.Pipelines.CopyPipeline());

                // generate
                await site.GenerateAsync();
                return 0;
            });


            return app.ExecuteAsync(args);
        }
    }
}
