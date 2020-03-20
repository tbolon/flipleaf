using System.Collections.Generic;
using System.IO;

using FlipLeaf.Core;

namespace FlipLeaf
{
    public interface IWebSite
    {
        WebSiteConfiguration Configuration { get; }

        string RootDirectory { get; }

        string InputDirectory { get; }

        string OutputDirectory { get; }

        IDictionary<string, object> Runtime { get; }

        void AddPipeline(IInputSource source, IPipeline pipeline);
    }

    public static class WebSiteExtensions
    {
        public static string GetFullRootPath(this IWebSite @this, string path)
        {
            if (path == null)
            {
                return @this.RootDirectory;
            }

            return Path.Combine(@this.RootDirectory, path);
        }

        public static string GetFullInputPath(this IWebSite @this, string path)
        {
            if (path == null)
            {
                return @this.InputDirectory;
            }

            return Path.Combine(@this.InputDirectory, path);
        }

        public static string GetFullOutputPath(this IWebSite @this, string path)
        {
            if (path == null)
            {
                return @this.OutputDirectory;
            }

            return Path.Combine(@this.OutputDirectory, path);
        }
    }
}
