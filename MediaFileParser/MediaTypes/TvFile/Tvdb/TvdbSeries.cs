using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace MediaFileParser.MediaTypes.TvFile.Tvdb
{
    /// <summary>
    /// Represents a TVDB series.
    /// </summary>
    [Serializable, ReadOnly(true), XmlType("Series", AnonymousType = true), XmlRoot(Namespace = "", IsNullable = false)]
    public class TvdbSeries : TvdbSeriesCommon
    {
        /// <summary>
        /// This class should not be instantiated.
        /// </summary>
        protected TvdbSeries(){}

        /// <summary>
        /// Represents a search for a TVDB series.
        /// </summary>
        [Serializable, ReadOnly(true), XmlType(AnonymousType = true), XmlRoot(ElementName = "Data", Namespace = "", IsNullable = false)]
        public class SeriesSearch
        {
            /// <summary>
            /// Array of series returned from a search.
            /// </summary>
            [XmlElement("Series")]
            public TvdbSeries[] Series;
        }

        /// <summary>
        /// Searches the TVDB for a series.
        /// </summary>
        /// <param name="series">Series to search for.</param>
        /// <param name="request">API request object to use.</param>
        /// <returns>An array of possible series.</returns>
        public static TvdbSeries[] GetTvdbSeriesSearch(string series, TvdbApiRequest request)
        {
            Debug.WriteLine("-> TvdbSeries::GetTvdbSeriesSearch series=\"" + series + "\" request=\"" + request + "\" Called");
            series = series.ToLower().Trim();
            var cacheKey = series + "~search";
            var seriesSearch = request.PerformApiRequestAndDeserialize<SeriesSearch>(GetSeriesUrl(series), cacheKey, false, true);
            return seriesSearch.Series;
        }

        /// <summary>
        /// Gets the API location used to perform this request.
        /// </summary>
        /// <param name="seriesName">Series name to search for.</param>
        /// <returns>URL to the appropriate request location.</returns>
        private static string GetSeriesUrl(string seriesName)
        {
            Debug.WriteLine("-> TvdbSeries::GetSeriesUrl seriesName=\"" + seriesName + "\" Called");
            return "GetSeries.php?seriesname=" + seriesName;
        }

        /// <summary>
        /// Gets indepth information about the current series.
        /// </summary>
        /// <param name="request">API requester to use.</param>
        /// <returns>Detailed series information.</returns>
        public TvdbDetailedSeries GetDetailedInformation(TvdbApiRequest request)
        {
            Debug.WriteLine("-> TvdbSeries::GetDetailedInformation request=\"" + request + "\" Called");
            return TvdbDetailedSeries.GetDetailedSeries(TvdbId, request);
        }
    }
}
