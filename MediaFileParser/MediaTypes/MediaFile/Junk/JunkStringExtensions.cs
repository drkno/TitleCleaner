namespace MediaFileParser.MediaTypes.MediaFile.Junk
{
    /// <summary>
    /// Junk String Extensions Class
    /// </summary>
    public static class JunkStringExtensions
    {
        /// <summary>
        /// Determines whether the beginning of this string instance matches a specified JunkString.
        /// </summary>
        /// <param name="str">The string to compare to.</param>
        /// <param name="startsWith">The JunkString to compare.</param>
        /// <returns>true if value matches the beginning of this string; otherwise, false.</returns>
        public static bool StartsWith(this string str, JunkString startsWith)
        {
            return str.StartsWith(startsWith.String);
        }

        /// <summary>
        /// Determines whether this instance and another specified JunkString object have the same value.
        /// </summary>
        /// <param name="str">The string to compare to.</param>
        /// <param name="obj">The JunkString to compare to this instance.</param>
        /// <returns>true if the value of the value parameter is the same as this instance; otherwise, false.</returns>
        public static bool EqualsJunk(this string str, JunkString obj)
        {
            return str.Equals(obj.String);
        }
    }
}
