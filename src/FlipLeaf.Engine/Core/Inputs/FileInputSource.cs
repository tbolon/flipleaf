using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FlipLeaf.Core.Inputs
{
    public class FileInputSource : IInputSource
    {
        private static readonly string[] WildcardPattern = new[] { "*" };
        private readonly string[] _patterns;
        private readonly string _path;

        public FileInputSource(string path)
            : this(path, WildcardPattern)
        {
        }

        public FileInputSource(string path, string pattern)
            : this(path, new[] { pattern })
        {
        }

        public FileInputSource(string path, string[] patterns)
        {

            _path = path;
            _patterns = patterns ?? throw new ArgumentNullException(nameof(patterns));
        }

        public string[] Exclude { get; set; }

        public bool Recursive { get; set; } = true;

        public IEnumerable<IInput> Get(IWebSite context)
        {
            var dir = new DirectoryInfo(Path.Combine(context.RootDirectory, _path));
            if (!dir.Exists)
            {
                return Enumerable.Empty<IInput>();
            }

            return GetDirectoryFiles(context, dir, dir, true);
        }

        private IEnumerable<IInput> GetDirectoryFiles(IWebSite context, DirectoryInfo dir, DirectoryInfo rootDir, bool root)
        {
            for (var i = 0; i < _patterns.Length; i++)
            {
                foreach (var file in dir.GetFiles(_patterns[i]))
                {
                    if (root)
                    {
                        if (Exclude != null && Exclude.Contains(file.Name, StringComparer.Ordinal))
                        {
                            continue;
                        }

                        yield return new FileInput(file.Name, file.FullName);
                    }
                    else
                    {
                        var relativeDir = dir.FullName.Substring(rootDir.FullName.Length + 1);
                        yield return new FileInput(Path.Combine(relativeDir, file.Name), file.FullName);
                    }
                }
            }

            if (Recursive)
            {
                foreach (var subDir in dir.GetDirectories())
                {
                    if (root)
                    {
                        if (Exclude != null && Exclude.Contains(subDir.Name, StringComparer.Ordinal))
                        {
                            continue;
                        }
                    }

                    foreach (var file in GetDirectoryFiles(context, subDir, rootDir, false))
                    {
                        yield return file;
                    }
                }
            }
        }
    }
}
