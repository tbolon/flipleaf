using System;
using System.Collections.Generic;
using System.Text;

namespace FlipLeaf.Core.Text.ScrewturnWiki
{
    /// <summary>
    /// Manages information about Page Redirections.
    /// </summary>
    internal static class Redirections
    {

        /// <summary>
        /// Adds a new Redirection.
        /// </summary>
        /// <param name="source">The source Page.</param>
        /// <param name="destination">The destination Page.</param>
        /// <returns>True if the Redirection is added, false otherwise.</returns>
        /// <remarks>The method prevents circular and multi-level redirection.</remarks>
        public static void AddRedirection(PageInfo source, PageInfo destination)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (destination == null) throw new ArgumentNullException("destination");

            throw new NotImplementedException();
        }
    }
