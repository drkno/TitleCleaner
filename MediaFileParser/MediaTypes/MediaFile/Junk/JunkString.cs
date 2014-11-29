using System;

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
            var s = obj as string;
            return s != null ? string.Equals(s, String) : ReferenceEquals(this, obj);
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
            return String.Compare(String, other.String, StringComparison.OrdinalIgnoreCase);
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
            return String.StartsWith(str.String, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Tests if this JunkString starts with a string.
        /// </summary>
        /// <param name="str">string to test.</param>
        /// <returns>true if this JunkString starts with the string, otherwise false</returns>
        public bool StartsWith(string str)
        {
            return String.StartsWith(str, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
