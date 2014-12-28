using System.Collections.Generic;
using System.Xml.Serialization;
using MediaFileParser.MediaTypes.TvFile.Tvdb.Cache;

namespace MediaFileParser.MediaTypes.TvFile.Tvdb
{
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false, ElementName = "Items")]
    public class TvdbApiTime
    {
        protected TvdbApiTime() { }

        [XmlElement("Time")]
        public uint Time { get; set; }

        [XmlElement("Episode")]
        public List<uint> Episodes { get; set; }

        [XmlElement("Series")]
        public List<uint> Series { get; set; }

        public static TvdbApiTime TvdbServerTime(TvdbCacheType cacheType, uint previousTime = 0)
        {
            var ut = cacheType == TvdbCacheType.None || previousTime == 0 ? UpdateType.Time : UpdateType.All;
            var st = TvdbApiRequest.PerformApiRequestAndDeserialize<TvdbApiTime>(GetUpdateUrl(ut, previousTime), true, true);
            st.Series.Sort();
            st.Episodes.Sort();
            return st;
        }

        private static string GetUpdateUrl(UpdateType type, uint previousTime)
        {
            string t;
            switch (type)
            {
                case UpdateType.All: t = "all&time=" + previousTime; break;
                case UpdateType.Time: t = "none"; break;
                case UpdateType.Episode: t = "episode&time=" + previousTime; break;
                case UpdateType.Series: t = "series&time=" + previousTime; break;
                default: goto case UpdateType.All;
            }
            return "Updates.php?type=" + t;
        }

        private enum UpdateType
        {
            Time,
            All,
            Episode,
            Series
        }
    }
}
