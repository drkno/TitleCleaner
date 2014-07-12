using System;
using System.Xml.Serialization;

namespace MediaFileParser.MediaFile.TvFile.Tvdb
{
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class TvdbSeriesSearch
    {
        [XmlElement("Series")]
        protected readonly TvdbSeries[] Series;
        
        public TvdbSeriesSearch(string name, bool useCache = false, string cacheLocation = null)
        {
            Series = series;
            throw new NotImplementedException();
        }

        //http://thetvdb.com/api/GetSeries.php?seriesname=

        public TvdbSeries this[int value]
        {
            get { return Series[value]; }
        }

        public int Length { get { return Series.Length; } }
    }
}
