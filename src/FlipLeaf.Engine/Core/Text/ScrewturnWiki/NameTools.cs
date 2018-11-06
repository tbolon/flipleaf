using System;
using System.Collections.Generic;
using System.Text;

namespace FlipLeaf.Core.Text.ScrewturnWiki
{

    /// <summary>
    /// Implements useful tools for handling full object names.
    /// </summary>
    public static class NameTools
    {
        /// <summary>
        /// Gets the full name of a page from the namespace and local name.
        /// </summary>
        /// <param name="nspace">The namespace (<c>null</c> for the root).</param>
        /// <param name="name">The local name.</param>
        /// <returns>The full name.</returns>
        public static string GetFullName(string nspace, string name)
        {
            return ((!string.IsNullOrEmpty(nspace)) ? (nspace + ".") : "") + name;
        }

        /// <summary>
        /// Expands a full name into the namespace and local name.
        /// </summary>
        /// <param name="fullName">The full name to expand.</param>
        /// <param name="nspace">The namespace.</param>
        /// <param name="name">The local name.</param>
        public static void ExpandFullName(string fullName, out string nspace, out string name)
        {
            if (fullName == null)
            {
                nspace = null;
                name = null;
            }
            else
            {
                string[] array = fullName.Split(new char[1]
                {
                    '.'
                }, StringSplitOptions.RemoveEmptyEntries);
                if (array.Length == 0)
                {
                    nspace = null;
                    name = null;
                }
                else if (array.Length == 1)
                {
                    nspace = null;
                    name = array[0];
                }
                else
                {
                    nspace = array[0];
                    name = array[1];
                }
            }
        }

        /// <summary>
        /// Extracts the namespace from a full name.
        /// </summary>
        /// <param name="fullName">The full name.</param>
        /// <returns>The namespace, or <c>null</c>.</returns>
        public static string GetNamespace(string fullName)
        {
            ExpandFullName(fullName, out string result, out string _);
            return result;
        }

        /// <summary>
        /// Extracts the local name from a full name.
        /// </summary>
        /// <param name="fullName">The full name.</param>
        /// <returns>The local name.</returns>
        public static string GetLocalName(string fullName)
        {
            ExpandFullName(fullName, out string _, out string result);
            return result;
        }

        /// <summary />
        public static bool AreNamespaceEquals(NamespaceInfo ns1, string ns2)
        {
            return AreNamespaceEquals(ns1?.Name, ns2);
        }

        /// <summary />
        public static bool AreNamespaceEquals(string ns1, NamespaceInfo ns2)
        {
            return AreNamespaceEquals(ns1, ns2?.Name);
        }

        /// <summary />
        public static bool AreNamespaceEquals(NamespaceInfo ns1, NamespaceInfo ns2)
        {
            return AreNamespaceEquals(ns1?.Name, ns2?.Name);
        }

        /// <summary />
        public static bool AreNamespaceEquals(string ns1, string ns2)
        {
            if (ns1 == null)
            {
                return ns2 == null;
            }
            if (ns2 == null)
            {
                return false;
            }
            return ns1.Equals(ns2, StringComparison.OrdinalIgnoreCase);
        }
    }
}
