using System;
using System.Linq;

namespace MediaFileParser.MediaTypes.MediaFile.Junk
{
    /// <summary>
    /// Type to store junk strings and those strings associated properties.
    /// </summary>
    public class JunkString : IComparable<JunkString>
    {
        /// <summary>
        /// Gets a unique hash code of this JunkString string representation.
        /// </summary>
        /// <returns>Hash code.</returns>
        public override int GetHashCode()
        {
            return String.GetHashCode();
        }

        /// <summary>
        /// Actual string that this JunkString represents.
        /// </summary>
        public string String { get; private set; }

        /// <summary>
        /// Quality of the media file that this JunkString represents.
        /// </summary>
        public MediaFileQuality? Quality { get; private set; }

        /// <summary>
        /// Types that this JunkString is applicable to.
        /// </summary>
        public Type[] ApplicableTypes { get; private set; }

        /// <summary>
        /// Strings that should not appear next in lexical order after this one.
        /// </summary>
        public string[] VetoedNextStr { get; private set; }

        /// <summary>
        /// Suffixes of this JunkString that are vetoed from being matched in a comparison.
        /// </summary>
        public string[] VetoedSuffixes { get; private set; }

        /// <summary>
        /// Creates a new JunkString.
        /// </summary>
        /// <param name="junk">String to represent.</param>
        /// <param name="quality">Quality of the media file to associate with this string.</param>
        public JunkString(string junk, MediaFileQuality? quality = null)
        {
            String = junk;
            Quality = quality;
        }

        /// <summary>
        /// Creates a new JunkString.
        /// </summary>
        /// <param name="junk">String to represent.</param>
        /// <param name="vetoedSuffixes">Suffixes to this JunkString to veto.</param>
        /// <param name="quality">Quality of the media file to associate with this string.</param>
        /// <param name="applicableTypes">Types of MediaFile that this JunkString is applicable to.</param>
        /// <param name="vetoedNextStr">Strings that should not appear after this one.</param>
        public JunkString(string junk, string[] vetoedSuffixes, MediaFileQuality? quality = null, Type[] applicableTypes = null, string[] vetoedNextStr = null)
        {
            String = junk;
            VetoedSuffixes = vetoedSuffixes;
            Quality = quality;
            ApplicableTypes = applicableTypes;
            VetoedNextStr = vetoedNextStr;
        }

        /// <summary>
        /// Creates a new JunkString from an assigned string.
        /// </summary>
        /// <param name="junk">String to convert to JunkString</param>
        /// <returns>A new JunkString.</returns>
        public static implicit operator JunkString(string junk)
        {
            return new JunkString(junk);
        }

        /// <summary>
        /// Compares another object to the string this JunkString represents. 
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>true if the object represents the same string, false otherwise</returns>
        public override bool Equals(object obj)
        {
            var junkString = obj as JunkString;
            if (junkString != null)
            {
                return junkString.Quality == Quality && junkString.String == String;
            }
            var s = obj as string;
            return s != null && string.Equals(s, String);
        }

        /// <summary>
        /// Compares the string that this JunkString represents to the string that
        /// another JunkString represents and determines if this should be before or
        /// after that string.
        /// </summary>
        /// <param name="other">Other JunkString to compare.</param>
        /// <returns>Negitive if before, positive if after or zero if equal in sort order.</returns>
        public int CompareTo(JunkString other)
        {
            var i = string.CompareOrdinal(String, 0, other.String, 0, String.Length);
            if (i == 0 && VetoedSuffixes != null)
            {
                foreach (var vetoedSuffix in VetoedSuffixes)
                {
                    if (string.CompareOrdinal(vetoedSuffix, 0, other.String, String.Length, vetoedSuffix.Length) == 0)
                    {
                        return vetoedSuffix.Length < other.String.Length ? 1 : -1;
                    }
                }
            }
            return i;
        }

        /// <summary>
        /// Gets the string representation of this JunkString object.
        /// </summary>
        /// <returns>String representation.</returns>
        public override string ToString()
        {
            return String;
        }

        /// <summary>
        /// Tests if this JunkString starts with another JunkString.
        /// </summary>
        /// <param name="str">JunkString to test.</param>
        /// <returns>true if this JunkString starts with other JunkString, otherwise false</returns>
        public bool StartsWith(JunkString str)
        {
            return String.StartsWith(str.String, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Tests if this JunkString starts with a string.
        /// </summary>
        /// <param name="str">string to test.</param>
        /// <returns>true if this JunkString starts with the string, otherwise false</returns>
        public bool StartsWith(string str)
        {
            return String.StartsWith(str, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Compares this JunkString to another using the JunkString comparison rules.
        /// </summary>
        /// <param name="str">JunkString to compare.</param>
        /// <returns>If they are equal.</returns>
        public bool JunkEquals(JunkString str)
        {
            return (str.String.Length > 3 || str.String.Length == String.Length) && CompareTo(str) == 0;
        }

        /// <summary>
        /// Checks if a string can appear after this one.
        /// </summary>
        /// <returns>True if it can, false otherwise.</returns>
        public bool CheckNextString(string next)
        {
            if (VetoedNextStr == null) return true;
            return !VetoedNextStr.Contains(next.ToLower());
        }
    }
}
