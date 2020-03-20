using System;
using System.Collections.Generic;

namespace FlipLeaf.Core.Text
{
    public class TextInputContext : IInputContext
    {
        public TextInputContext(IWebSite context, IInput input)
        {
            Input = input;
            InputPath = input.GetFullInputPath(context);
            Site = context;
            
            // add fixed values
            Items["name"] = System.IO.Path.GetFileName(input.RelativeName);
            Items["path"] = input.RelativeName;
            Items["fullPath"] = input.Path;
        }

        public IWebSite Site { get; }

        public IInput Input { get; }

        public string InputPath { get; }

        /// <summary>
        /// Gets or sets the output path for this item.
        /// </summary>
        public string OutputPath { get; set; }

        /// <summary>
        /// Gets or sets the ouput content for this item.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets all the items available in the context of this input.
        /// </summary>
        public InputItems Items { get; } = new InputItems();
    }
}
