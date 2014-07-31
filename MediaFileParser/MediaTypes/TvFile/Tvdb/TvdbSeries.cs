using System;
using System.Xml.Serialization;

namespace tvdbApi
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
            var seriesSearch = TvdbApiRequest.PerformApiRequestAndDeserialize<SeriesSearch>(GetSeriesUrl(series));
            return seriesSearch.Series;
        }

        private static string GetSeriesUrl(string seriesName)
        {
            return "GetSeries.php?seriesname=" + seriesName;
        }

        public TvdbDetailedSeries GetDetailedInformation()
        {
            return TvdbDetailedSeries.GetDetailedSeries(Id);
        }
    }
}
