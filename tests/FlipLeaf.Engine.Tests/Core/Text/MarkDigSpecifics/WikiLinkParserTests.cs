using System.Linq;

using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

using Xunit;

namespace FlipLeaf.Core.Text.MarkdigSpecifics
{
    public class WikiLinkParserTests
    {
        [Fact]
        public void Detect_Url()
        {
            var md = "[[Hello]]";

            var b = TryParse(md, out var link);

            Assert.True(b);
            Assert.Equal("Hello.html", link.Url);

            Assert.Equal("Hello", GetSpanText(md, link.UrlSpan));
        }

        [Fact]
        public void Detect_Label()
        {
            var md = "[[Hello]]";

            var b = TryParse(md, out var link);

            Assert.True(b);
            Assert.Equal("Hello", link.Label);

            Assert.Equal("Hello", GetSpanText(md, link.LabelSpan));
        }

        [Fact]
        public void Detect_Split()
        {
            var md = "[[Hello|World]]";

            var b = TryParse(md, out var link);

            Assert.True(b);
            Assert.Equal("Hello.html", link.Url);

            Assert.Equal("Hello", GetSpanText(md, link.UrlSpan));
            Assert.Equal("World", link.Label);
            Assert.Equal("World", GetSpanText(md, link.LabelSpan));
        }

        [Fact]
        public void Adapt_Url()
        {
            var md = "[[Foo Bar]]";

            var b = TryParse(md, out var link);

            Assert.True(b);
            Assert.Equal("Foo_Bar.html", link.Url);

            Assert.Equal("Foo Bar", GetSpanText(md, link.UrlSpan));
            Assert.Equal("Foo Bar", link.Label);
            Assert.Equal("Foo Bar", GetSpanText(md, link.LabelSpan));
        }

        [Fact]
        public void Adapt_Url_With_Dash()
        {
            var md = "[[Foo Bar]]";

            var b = TryParse(md, out var link, whiteSpaceUrlChar: '-');

            Assert.True(b);
            Assert.Equal("Foo-Bar.html", link.Url);

            Assert.Equal("Foo Bar", GetSpanText(md, link.UrlSpan));
            Assert.Equal("Foo Bar", link.Label);
            Assert.Equal("Foo Bar", GetSpanText(md, link.LabelSpan));
        }

        [Fact]
        public void Ignore_NonClosing()
        {
            var md = "[[Foo";

            var b = TryParse(md, out var link);

            Assert.False(b);
        }

        [Fact]
        public void Ignore_NonClosing2()
        {
            var md = "[[Foo]";

            var b = TryParse(md, out var link);

            Assert.False(b);
        }

        [Fact]
        public void Ignore_Single_Closing()
        {
            var md = "[[Foo]Bar]]";

            var b = TryParse(md, out var link);

            Assert.True(b);
            Assert.Equal("Foo]Bar", link.Label);
        }

        [Fact]
        public void Ignore_Empty()
        {
            var md = "None";

            var b = TryParse(md, out var link);

            Assert.False(b);
        }

        [Fact]
        public void Accept_Empty_Link()
        {
            var md = "[[|Text]]";

            var b = TryParse(md, out var link);

            Assert.True(b);
            Assert.Equal("Text", link.Label);
            Assert.Equal("Text.html", link.Url);
        }

        [Fact]
        public void Accept_Empty_Label()
        {
            var md = "[[Url|]]";

            var b = TryParse(md, out var link);

            Assert.True(b);
            Assert.Equal("Url", link.Label);
            Assert.Equal("Url.html", link.Url);
        }

        [Fact]
        public void Include_Trailing_Characters()
        {
            var md = "[[Text]]s";

            var b = TryParse(md, out var link, includeTrailingCharacters: true);

            Assert.True(b);
            Assert.Equal("Texts", link.Label);

            Assert.Equal("Text", GetSpanText(md, link.LabelSpan));
            Assert.Equal("Text.html", link.Url);
            Assert.Equal("Text", GetSpanText(md, link.UrlSpan));
        }

        [Fact]
        public void Exclude_Trailing_Whitespaces()
        {
            var md = "[[Text]] abc";

            var b = TryParse(md, out var link, includeTrailingCharacters: true);

            Assert.True(b);
            Assert.Equal("Text", link.Label);
            Assert.Equal("Text.html", link.Url);
        }

        [Fact]
        public void Include_Trailing_Apostrophe()
        {
            var md = "[[Text]]'s";

            var b = TryParse(md, out var link, includeTrailingCharacters: true);

            Assert.True(b);
            Assert.Equal("Text's", link.Label);
            Assert.Equal("Text.html", link.Url);
        }

        [Fact]
        public void Ignore_Trailing_When_Explicit_Text()
        {
            var md = "[[Url|Text]]'s";

            var b = TryParse(md, out var link, includeTrailingCharacters: true);

            Assert.True(b);
            Assert.Equal("Text", link.Label);
            Assert.Equal("Url.html", link.Url);
        }

        [Fact]
        public void Ignore_Escaped_Separator()
        {
            var md = @"[[Url\|Url|Text]]";

            var b = TryParse(md, out var link);

            Assert.True(b);
            Assert.Equal("Text", link.Label);
            Assert.Equal(@"Url\|Url.html", link.Url);
        }

        [Fact]
        public void Ignore_Escaped_Separator_02()
        {
            var md = @"[[Url\|Text]]";

            var b = TryParse(md, out var link);

            Assert.True(b);
            Assert.Equal(@"Url\|Text", link.Label);
            Assert.Equal(@"Url\|Text.html", link.Url);
        }

        private string GetSpanText(string markdown, SourceSpan? span)
        {
            if (!span.HasValue)
                return null;

            return markdown.Substring(span.Value.Start, span.Value.Length);
        }

        private bool TryParse(string markdown, out LinkInline link, bool? includeTrailingCharacters = null, char? whiteSpaceUrlChar = null)
        {
            var pr = BuidProcessor();
            var sl = new StringSlice(markdown);

            var p = new WikiLinkParser() { Extension = ".html" };
            if (includeTrailingCharacters != null) p.IncludeTrailingCharacters = includeTrailingCharacters.Value;
            if (whiteSpaceUrlChar != null) p.WhiteSpaceUrlChar = whiteSpaceUrlChar.Value;

            var b = p.Match(pr, ref sl);

            if (b)
            {
                link = (LinkInline)pr.Inline;
            }
            else
            {
                link = null;
            }

            return b;
        }

        private InlineProcessor BuidProcessor()
        {
            var p = new WikiLinkParser();
            var pr = new InlineProcessor(
                new StringBuilderCache(), 
                new MarkdownDocument(),
                new InlineParserList(Enumerable.Empty<InlineParser>()),
                false,
                new Markdig.MarkdownParserContext()
                );
            return pr;
        }
    }
}
