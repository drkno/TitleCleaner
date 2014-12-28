using System;
using System.Xml.Serialization;

namespace MediaFileParser.MediaTypes.TvFile.Tvdb
{
    [Serializable]
    [XmlType("Series", AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class TvdbSeries : TvdbSeriesCommon
    {
        protected TvdbSeries(){}

        [XmlType(AnonymousType = true)]
        [XmlRoot(ElementName = "Data", Namespace = "", IsNullable = false)]
        public class SeriesSearch
        {
            [XmlElement("Series")]
            public TvdbSeries[] Series;
        }

        public static TvdbSeries[] GetTvdbSeriesSearch(string series)
        {
            series = series.ToLower().Trim();
            var seriesSearch = TvdbApiRequest.PerformApiRequestAndDeserialize<SeriesSearch>(GetSeriesUrl(series), false, true);
            return seriesSearch.Series;
        }

        private static string GetSeriesUrl(string seriesName)
        {
            return "GetSeries.php?seriesname=" + seriesName;
        }

        public TvdbDetailedSeries GetDetailedInformation(TvdbApiTime time = null)
        {
            if (time == null) return TvdbDetailedSeries.GetDetailedSeries(TvdbId, false);
            var ind = time.Series.BinarySearch(TvdbId);
            return TvdbDetailedSeries.GetDetailedSeries(TvdbId, ind >= 0 && ind < time.Series.Count);
        }
    }
}
