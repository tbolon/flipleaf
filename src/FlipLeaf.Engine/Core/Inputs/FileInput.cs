using System.IO;

namespace FlipLeaf.Core.Inputs
{
    public class FileInput : IInput
    {
        public FileInput(string relativeName, string path)
        {
            RelativeName = relativeName;
            Path = path;
            Extension = System.IO.Path.GetExtension(path);
        }

        public string Extension { get; }

        public string RelativeName { get; }

        public string Path { get; }

        public Stream Open(IWebSite ctx)
        {
            return File.OpenRead(Path ?? System.IO.Path.Combine(ctx.InputDirectory, RelativeName));
        }
    }
}
