using System;
using System.Collections;
using System.Collections.Generic;

namespace MediaFileParser.MediaTypes.MediaFile.Junk
{
    /// <summary>
    /// Collection for storing and searching JunkStrings.
    /// </summary>
    public class JunkContainer : ICollection<JunkString>
    {
        /// <summary>
        /// List to store long JunkStrings.
        /// </summary>
        private readonly List<JunkString> _junk; 

        /// <summary>
        /// Creates a new JunkContainer.
        /// </summary>
        public JunkContainer()
        {
            _junk = new List<JunkString>();
        }

        /// <summary>
        /// Gets an enumerator over this JunkContainer collection.
        /// </summary>
        /// <returns>An enumerator.</returns>
        public IEnumerator<JunkString> GetEnumerator()
        {
            return _junk.GetEnumerator();
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
        public void Add(string junkString, MediaFileQuality? quality)
        {
            Add(new JunkString(junkString, quality));
            _sorted = false;
        }

        /// <summary>
        /// Adds a new JunkString to this container.
        /// </summary>
        /// <param name="junkString">string to represent with JunkString.</param>
        /// <param name="vetoedSuffixes">JunkString suffixes that should not match when compared to the JunkString.</param>
        /// <param name="quality">Quality that the JunkString should represent.</param>
        public void Add(string junkString, string[] vetoedSuffixes, MediaFileQuality? quality)
        {
            Add(new JunkString(junkString, vetoedSuffixes, quality));
            _sorted = false;
        }

        /// <summary>
        /// Adds an existing JunkString to this JunkContainer.
        /// </summary>
        /// <param name="item">JunkString to add.</param>
        public void Add(JunkString item)
        {
            _junk.Add(item);
            _sorted = false;
        }

        /// <summary>
        /// The long values are currently in a sorted state.
        /// </summary>
        private bool _sorted;

        /// <summary>
        /// Removes all JunkStrings from this JunkContainer.
        /// </summary>
        public void Clear()
        {
            _junk.Clear();
        }

        /// <summary>
        /// Sorts the long junk list.
        /// </summary>
        private void Sort()
        {
            _junk.Sort();//_startsWithComparer);
            _sorted = true;
        }

        /// <summary>
        /// Searches for a JunkString representing a string in this JunkContainer.
        /// </summary>
        /// <param name="item">string to search for.</param>
        /// <returns>JunkString if found, null otherwise.</returns>
        public JunkString Find(string item)
        {
            if (!_sorted)
            {
                Sort();
            }

            var index = _junk.BinarySearch(item);
            if (index >= 0) return _junk[index].JunkEquals(item) ? _junk[index] : null;
            index = ~index;
            if (index > _junk.Count || index == 0) return null;
            if (_junk[index - 1].JunkEquals(item)) return _junk[index - 1];
            if (index != _junk.Count && _junk[index].JunkEquals(item)) return _junk[index];
            return null;
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
            return !Equals(Find(item), null);
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
            return _junk.Remove(item);
        }

        /// <summary>
        /// Gets the number of JunkStrings in this JunkContainer.
        /// </summary>
        public int Count { get { return _junk.Count; } }

        /// <summary>
        /// Checks if this JunkContainer is read only. Always false.
        /// </summary>
        public bool IsReadOnly { get { return false; } }
    }
}
