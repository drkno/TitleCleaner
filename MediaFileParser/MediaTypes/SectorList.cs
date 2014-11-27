using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Threading;

namespace MediaFileParser.MediaTypes
{
    /// <summary>
    /// 
    /// </summary>
    public class SectorList : List<string>
    {
        public SectorList()
        {

        }

        public new bool RemoveAt(int index)
        {
            this[index] = String.Empty;
        }

        public new bool Remove(string str)
        {

        }
    }
}
