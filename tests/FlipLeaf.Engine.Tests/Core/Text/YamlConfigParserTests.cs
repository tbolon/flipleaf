using System.IO;

using Xunit;

namespace FlipLeaf.Core.Text
{
    public class YamlConfigParserTests
    {
        [Fact]
        public void Empty_Config_Should_Result_In_Default_Config()
        {
            // arrange
            var reader = new StringReader(string.Empty);

            // act
            var config = new YamlConfigParser().ParseConfig(reader);

            // assert
            Assert.Equal(WebSiteConfiguration.Default.BaseUrl, config.BaseUrl);
            Assert.Equal(WebSiteConfiguration.Default.LayoutDir, config.LayoutDir);
            Assert.Equal(WebSiteConfiguration.Default.OutputDir, config.OutputDir);
            Assert.Equal(WebSiteConfiguration.Default.Title, config.Title);
        }

        [Fact]
        public void Empty_NewLine_Config_Should_Result_In_Default_Config()
        {
            // arrange
            var reader = new StringReader("\n");

            // act
            var config = new YamlConfigParser().ParseConfig(reader);

            // assert
            Assert.Equal(WebSiteConfiguration.Default.BaseUrl, config.BaseUrl);
            Assert.Equal(WebSiteConfiguration.Default.LayoutDir, config.LayoutDir);
            Assert.Equal(WebSiteConfiguration.Default.OutputDir, config.OutputDir);
            Assert.Equal(WebSiteConfiguration.Default.Title, config.Title);
        }

        [Fact]
        public void Read_BaseUrl_Config()
        {
            // arrange
            var reader = new StringReader("baseUrl: test\n");

            // act
            var config = new YamlConfigParser().ParseConfig(reader);

            // assert
            Assert.Equal("test", config.BaseUrl);
        }

        [Fact]
        public void Read_Title_Config()
        {
            // arrange
            var reader = new StringReader("title: 'my title'\n");

            // act
            var config = new YamlConfigParser().ParseConfig(reader);

            // assert
            Assert.Equal("my title", config.Title);
        }
    }
}
