using System.Collections.Generic;
using System.Linq;

namespace MediaFileParser.MediaTypes
{
    /// <summary>
    /// 
    /// </summary>
    public class SectorList : LinkedList<string>, IList<string>
    {
        private readonly Dictionary<int, LinkedListNode<string>> _indexes = new Dictionary<int, LinkedListNode<string>>();
        private int _highest = -1;

        public SectorList(IEnumerable<string> sectorList)
        {
            foreach (var node in sectorList.Select(AddLast))
            {
                _indexes[++_highest] = node;
            }
        }

        private LinkedListNode<string> Get(int index)
        {
            LinkedListNode<string> current = null;
            if (_highest < 0)
            {
                current = _indexes[_highest];
            }
            else if (First != null)
            {
                current = First;
                _highest = 0;
            }

            for (var i = 0; i <= index && i < Count; i++)
            {
                if (current == null)
                {
                    return null;
                }
                _indexes[_highest++] = current;
                if (i != index)
                {
                    current = current.Next;
                }
            }
            return current;
        }

        public int IndexOf(string item)
        {
            var current = First;
            for (var i = 0; i < Count; i++)
            {
                if (current == null)
                {
                    return -1;
                }
                if (i > _highest)
                {
                    _indexes[i] = current;
                }
                if (current.Value == item)
                {
                    return i;
                }
                current = current.Next;
            }
            return -1;
        }

        public void Insert(int index, string item)
        {
            index--;
            var beforeNode = index > _highest ? Get(index) : _indexes[index];
            var node = AddAfter(beforeNode, item);
            _indexes[index] = node;
            _highest = index + 1;
        }

        public void RemoveAt(int index)
        {
            if (index > _highest)
            {
                var node = Get(index);
                Remove(node);
            }
            else
            {
                var node = _indexes[index];
                _indexes[index] = null;
                Remove(node);
            }
            _highest = index - 1;
        }

        public string this[int index]
        {
            get
            {
                return index <= _highest ? _indexes[index].Value : Get(index).Value;
            }
            set
            {
                if (index > _highest)
                {
                    Get(index).Value = value;
                    return;
                }
                _indexes[index].Value = value;
            }
        }

        public void RemoveRange(int index, int count)
        {
            for (var j = 0; j < count; j++)
            {
                RemoveAt(index);
            }
        }
    }
}
