FlipLeaf
========

FlipLeaf is a small static website generator.

It is aimed to be used with .NET Core Tooling.

# Features

* Copy all content files to `_site` folder
* Read and transform .md files (jekyll like)
* Handle yaml headers (jekyll like)
* Accept liquid syntax (jekyll like)

## Architecture

The Engine simply runs all `IPipeline`.

An `IPipeline` consist of an `IInputSource` and an `IInputTransform`.

The `IInputSource` returns multiple `IInput`.
In most frequent cases, the IInput will be a file, and the IInputSource will return all files in the working directory.

The IInputTransform process an IInputFile and emits (or not) a result.

A simple implementation is `CopyTransform`, which will simple copy the file in the output directory.
A more complex implementation can read the content of the file as text, change it then write it to the output directory.
Another implementation could execute an external tool to optimize some files then write the optimized version in the output directory.

## Sample script

```
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

```