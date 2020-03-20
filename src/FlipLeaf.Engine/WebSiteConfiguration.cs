using System.Collections.Generic;
using System.IO;

namespace FlipLeaf
{
    public class WebSiteConfiguration
    {
        public const string DefaultFileName = "_config.yml";
        private const string DefaultLayoutsDir = "_layouts";
        private const string DefaultOutputDir = "_site";

        public static readonly WebSiteConfiguration Default = new WebSiteConfiguration();

        public string Title { get; set; }

        public string InputDir { get; set; }

        public string LayoutDir { get; set; } = DefaultLayoutsDir;

        public string OutputDir { get; set; } = DefaultOutputDir;

        public string BaseUrl { get; set; } = string.Empty;

        public static WebSiteConfiguration LoadFromDisk(string path)
        {
            if (File.Exists(path))
            {
                return new Core.Text.YamlConfigParser().ParseConfig(path);
            }

            return null;
        }

        public void ApplyTo(IDictionary<string, object> target)
        {
            target["title"] = Title;
            target["inputDir"] = InputDir;
            target["baseUrl"] = BaseUrl;
            target["layoutDir"] = LayoutDir;
            target["outputDir"] = OutputDir;
        }
    }
}
