using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MediaFileParser.MediaTypes.MediaFile.Junk
{
    /// <summary>
    /// Collection for storing and searching JunkStrings.
    /// </summary>
    public class JunkContainer : ICollection<JunkString>
    {
        /// <summary>
        /// List to store short JunkStrings.
        /// </summary>
        private readonly List<JunkString> _shortJunk;

        /// <summary>
        /// List to store long JunkStrings.
        /// </summary>
        private readonly List<JunkString> _longJunk; 

        /// <summary>
        /// Creates a new JunkContainer.
        /// </summary>
        public JunkContainer()
        {
            _shortJunk = new List<JunkString>();
            _longJunk = new List<JunkString>();
        }

        /// <summary>
        /// Gets an enumerator over this JunkContainer collection.
        /// </summary>
        /// <returns>An enumerator.</returns>
        public IEnumerator<JunkString> GetEnumerator()
        {
            return new Enumerator(_shortJunk.GetEnumerator(), _longJunk.GetEnumerator());
        }

        /// <summary>
        /// Gets an enumerator over this JunkContainer collection.
        /// </summary>
        /// <returns>An enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Adds a new JunkString to this container.
        /// </summary>
        /// <param name="junkString">string to represent with JunkString.</param>
        /// <param name="quality">Quality that the JunkString should represent.</param>
        public void Add(string junkString, MediaFileQuality quality)
        {
            Add(new JunkString(junkString, quality));
        }

        /// <summary>
        /// Adds an existing JunkString to this JunkContainer.
        /// </summary>
        /// <param name="item">JunkString to add.</param>
        public void Add(JunkString item)
        {
            if (item.String.Length <= 3)
            {
                _shortJunk.Add(item);
            }
            else
            {
                _longJunk.Add(item);
            }
        }

        /// <summary>
        /// Removes all JunkStrings from this JunkContainer.
        /// </summary>
        public void Clear()
        {
            _shortJunk.Clear();
            _longJunk.Clear();
        }

        /// <summary>
        /// Searches for a JunkString representing a string in this JunkContainer.
        /// </summary>
        /// <param name="item">string to search for.</param>
        /// <returns>JunkString if found, null otherwise.</returns>
        public JunkString Find(string item)
        {
            if (item.Length <= 3)
            {
                return _shortJunk.Find(item.EqualsJunk);
            }
            var obj = _shortJunk.Find(item.StartsWith) ?? _longJunk.Find(item.StartsWith);
            return obj;
        }

        /// <summary>
        /// Searches for a JunkString in this JunkContainer.
        /// </summary>
        /// <param name="item">JunkString to search for.</param>
        /// <returns>JunkString if found, null otherwise.</returns>
        public JunkString Find(JunkString item)
        {
            return Find(item.ToString());
        }

        /// <summary>
        /// Checks if this JunkContainer contains a JunkString representing a string.
        /// </summary>
        /// <param name="item">string to search for representation of.</param>
        /// <returns>true if found, false otherwise</returns>
        public bool Contains(string item)
        {
            return item.Length <= 3 && _shortJunk.Contains(item) ||
                item.Length > 3 && (_shortJunk.Any(item.StartsWith) || _longJunk.Any(item.StartsWith));
        }

        /// <summary>
        /// Checks if this JunkContainer contains a JunkString.
        /// </summary>
        /// <param name="item">JunkString to search for.</param>
        /// <returns>true if found, false otherwise</returns>
        public bool Contains(JunkString item)
        {
            return Contains(item.ToString());
        }

        /// <summary>
        /// Copies the JunkStrings in this JunkContainer to an array starting at an index.
        /// </summary>
        /// <param name="array">Array to copy the JunkStrings to.</param>
        /// <param name="arrayIndex">Index of the array to start at.</param>
        public void CopyTo(JunkString[] array, int arrayIndex)
        {
            if (arrayIndex + Count >= array.Length)
            {
                Array.Resize(ref array, arrayIndex + Count);
            }

            foreach (var junkString in this)
            {
                array[arrayIndex++] = junkString;
            }
        }

        /// <summary>
        /// Removes a specific JunkString from this JunkContainer.
        /// </summary>
        /// <param name="item">JunkString to remove.</param>
        /// <returns>true if removal was successful, otherwise false</returns>
        public bool Remove(JunkString item)
        {
            return _shortJunk.Remove(item) || _longJunk.Remove(item);
        }

        /// <summary>
        /// Gets the number of JunkStrings in this JunkContainer.
        /// </summary>
        public int Count { get { return _shortJunk.Count + _longJunk.Count; } }

        /// <summary>
        /// Checks if this JunkContainer is read only. Always false.
        /// </summary>
        public bool IsReadOnly { get { return false; } }

        /// <summary>
        /// Junk Container enumerator.
        /// </summary>
        internal class Enumerator : IEnumerator<JunkString>
        {
            /// <summary>
            /// Enumerator for short junk items.
            /// </summary>
            private List<JunkString>.Enumerator _shortEnumerator;

            /// <summary>
            /// Enumerator for long junk items.
            /// </summary>
            private List<JunkString>.Enumerator _longEnumerator;

            /// <summary>
            /// Flag to determine which enumerator (short or long) to use.
            /// </summary>
            private bool _short;

            /// <summary>
            /// Instantiates a new JunkContainer enumerator.
            /// </summary>
            /// <param name="shortEnumerator">Enumerator for short junk strings.</param>
            /// <param name="longEnumerator">Enumerator for long junk strings.</param>
            public Enumerator(List<JunkString>.Enumerator shortEnumerator, List<JunkString>.Enumerator longEnumerator)
            {
                _shortEnumerator = shortEnumerator;
                _longEnumerator = longEnumerator;
                _short = true;
            }

            /// <summary>
            /// Dispose of this enumerator.
            /// </summary>
            public void Dispose()
            {
                _shortEnumerator.Dispose();
                _longEnumerator.Dispose();
            }

            /// <summary>
            /// Move to the next item in the JunkContainer.
            /// </summary>
            /// <returns>true if there was an item to move to, otherwise false</returns>
            public bool MoveNext()
            {
                if (!_short) return _longEnumerator.MoveNext();
                if (_shortEnumerator.MoveNext())
                {
                    return true;
                }
                _short = false;
                return _longEnumerator.MoveNext();
            }

            /// <summary>
            /// Resets this enumerator back to the first item.
            /// TODO: FIXME. System.Collections.Generic.List does not contain a Reset()
            /// </summary>
            public void Reset()
            {
                _short = true;
            }

            /// <summary>
            /// Get the element at the current position in the enumerator.
            /// </summary>
            public JunkString Current { get { return _short ? _shortEnumerator.Current : _longEnumerator.Current; } }

            /// <summary>
            /// Get the element at the current position in the enumerator.
            /// </summary>
            object IEnumerator.Current
            {
                get { return Current; }
            }
        }
    }

    /// <summary>
    /// Junk String Extensions Class
    /// </summary>
    public static class StringExtender
    {
        /// <summary>
        /// Determines whether the beginning of this string instance matches a specified JunkString.
        /// </summary>
        /// <param name="str">The string to compare to.</param>
        /// <param name="startsWith">The JunkString to compare.</param>
        /// <returns>true if value matches the beginning of this string; otherwise, false.</returns>
        public static bool StartsWith(this string str, JunkString startsWith)
        {
            return str.StartsWith(startsWith.ToString());
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
