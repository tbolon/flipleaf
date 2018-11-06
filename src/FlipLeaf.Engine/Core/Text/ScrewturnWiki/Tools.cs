using System;
using System.Collections.Generic;
using System.Text;

namespace FlipLeaf.Core.Text.ScrewturnWiki
{

    internal static class Tools
    {

        /// <summary>
        /// Detects the correct <see cref="T:NamespaceInfo" /> object associated to the current namespace using the <b>NS</b> parameter in the query string.
        /// </summary>
        /// <returns>The correct <see cref="T:NamespaceInfo" /> object, or <c>null</c>.</returns>
        public static NamespaceInfo DetectCurrentNamespaceInfo() => throw new NotImplementedException();

        public static string UrlEncode(string url) => url;

        public static string UrlDecode(string url) => url;

    }
}
