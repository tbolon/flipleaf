using System;
using System.Collections.Generic;
using System.Text;

namespace FlipLeaf.Core.Text.ScrewturnWiki
{
    internal static class Settings
    {
        public static bool ProcessSingleLineBreaks => true;
        /// <summary>
		/// Gets or sets a value indicating if links must use double square brackets exclusively.
		/// By default both single and double square brackets are allowed to create links.
		/// </summary>
		public static bool IgnoreSingleSquareBrackets => true;

        public static bool EnableSectionAnchors => true;
    }
}
