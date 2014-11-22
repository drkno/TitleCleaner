using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MediaFileParser.MediaTypes.MediaFile
{
    public class JunkString : IComparable<JunkString>
    {
        public string String { get; private set; }

        public MediaFileQuality? Quality { get; private set; }

        public JunkString(string junk, MediaFileQuality? quality = null)
        {
            String = junk;
            Quality = quality;
        }

        public static implicit operator JunkString(string junk)
        {
            return new JunkString(junk);
        }

        public override bool Equals(object obj)
        {
            if (String == null && Quality == null && obj == null) return true;
            return (obj is JunkString && ((JunkString) obj).String == String) ||
                   (obj is string && ((string) obj) == String);
        }

        public int CompareTo(JunkString other)
        {
            return String.Compare(String, other.String, StringComparison.OrdinalIgnoreCase);
        }

        public override string ToString()
        {
            return String;
        }

        public bool StartsWith(JunkString str)
        {
            return String.StartsWith(str.String);
        }

        public bool StartsWith(string str)
        {
            return String.StartsWith(str);
        }
    }

    public class JunkContainer : ICollection<JunkString>
    {
        private readonly List<JunkString> _shortJunk;
        private readonly List<JunkString> _longJunk; 

        public JunkContainer()
        {
            _shortJunk = new List<JunkString>();
            _longJunk = new List<JunkString>();
        }

        public IEnumerator<JunkString> GetEnumerator()
        {
            return new Enumerator(_shortJunk.GetEnumerator(), _longJunk.GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(string junkString, MediaFileQuality quality)
        {
            Add(new JunkString(junkString, quality));
        }

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

        public void Clear()
        {
            _shortJunk.Clear();
            _longJunk.Clear();
        }

        public JunkString Find(string item)
        {
            if (item.Length <= 3 && _shortJunk.Contains(item))
            {
                return _shortJunk.Find(item.Equals);
            }
            var obj = _shortJunk.Find(item.StartsWith) ?? _longJunk.Find(item.StartsWith);
            return obj;
        }

        public JunkString Find(JunkString item)
        {
            return Find(item.ToString());
        }

        public bool Contains(string item)
        {
            return item.Length <= 3 && _shortJunk.Contains(item) ||
                item.Length > 3 && (_shortJunk.Any(item.StartsWith) || _longJunk.Any(item.StartsWith));
        }

        public bool Contains(JunkString item)
        {
            return Contains(item.ToString());
        }

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

        public bool Remove(JunkString item)
        {
            return _shortJunk.Remove(item) || _longJunk.Remove(item);
        }

        public int Count { get { return _shortJunk.Count + _longJunk.Count; } }
        public bool IsReadOnly { get { return false; } }

        internal class Enumerator : IEnumerator<JunkString>
        {
            private List<JunkString>.Enumerator _shortEnumerator;
            private List<JunkString>.Enumerator _longEnumerator;
            private bool _short;
            public Enumerator(List<JunkString>.Enumerator shortEnumerator, List<JunkString>.Enumerator longEnumerator)
            {
                _shortEnumerator = shortEnumerator;
                _longEnumerator = longEnumerator;
                _short = true;
            }

            public void Dispose()
            {
                _shortEnumerator.Dispose();
                _longEnumerator.Dispose();
            }

            public bool MoveNext()
            {
                if (_short)
                {
                    if (_shortEnumerator.MoveNext())
                    {
                        return true;
                    }
                    _short = false;
                }
                return _longEnumerator.MoveNext();
            }

            public void Reset()
            {
                _short = true;
            }

            public JunkString Current { get { return _short ? _shortEnumerator.Current : _longEnumerator.Current; } }

            object IEnumerator.Current
            {
                get { return Current; }
            }
        }
    }

    public static class StringExtender
    {
        public static bool StartsWith(this string str, JunkString startsWith)
        {
            return str.StartsWith(startsWith.ToString());
        }
    }
}
