#! "netcoreapp2.1"
#r "_bin/FlipLeaf.Engine.dll"
#r "_bin/Markdig.dll"
#load "FlipLeaf.Scripting.csx"

using FlipLeaf;
using FlipLeaf.Core;
using FlipLeaf.Core.Text;
using static FlipLeafGenerator;

Sources.Add("content", s => new FlipLeaf.Core.Inputs.FileInputSource(s.InputDirectory) { Exclude = new[] { "README.md" } });
Sources.Add("static", s => new FlipLeaf.Core.Inputs.FileInputSource(s.GetFullRootPath("_static")));

Pipelines.Add("content", site =>
{
    var parser = new FlipLeaf.Core.Text.MarkdigSpecifics.MarkdigParser();
    parser.Use(new FlipLeaf.Core.Text.MarkdigSpecifics.WikiLinkExtension() { Extension = ".md" });
    parser.Use(new FlipLeaf.Core.Text.MarkdigSpecifics.CustomLinkInlineRendererExtension(site));

    return new FlipLeaf.Core.Pipelines.TextTransformPipeline(
        i => i.Extension == ".md",
        // read
        new FlipLeaf.Core.Text.ReadContentMiddleware(),
        // prepare
        new FlipLeaf.Core.Text.ITextMiddleware[] {
            new FlipLeaf.Core.Text.YamlHeaderPrepareMiddleware(new FlipLeaf.Core.Text.YamlParser())
        },
        // transform
        new FlipLeaf.Core.Text.ITextMiddleware[] {
            new FlipLeaf.Core.Text.YamlHeaderMiddleware(new FlipLeaf.Core.Text.YamlParser()),
            new FlipLeaf.Core.Text.LiquidMiddleware(new FlipLeaf.Core.Text.FluidLiquid.FluidParser(site)),
            new FlipLeaf.Core.Text.MarkdownMiddleware(parser)
        },
        // write
        new FlipLeaf.Core.Text.WriteContentMiddleware() { Extension = ".html" }
    );
});

Pipelines.Add("static", s => new FlipLeaf.Core.Pipelines.CopyPipeline());

await Generate(Args.ToArray());
