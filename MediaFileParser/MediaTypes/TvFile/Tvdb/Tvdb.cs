using System;

namespace MediaFileParser.MediaTypes.TvFile.Tvdb
{
    public class Tvdb
    {
        /// <summary>
        /// Unique application key to allow access to the TVDB api.
        /// </summary>
        public string ApiKey { get; set; } // F9D98CE470B5ABAE

        /// <summary>
        /// User Agent to use for HTTP requests to the TVDB server.
        /// </summary>
        public static string UserAgent { get { return _userAgent; } set { _userAgent = value; } }
        private static string _userAgent = "TvdbRequestManager/2.0 (C#, part of TitleCleaner, like Gecko)";

        /// <summary>
        /// Currently in use mirror. Random mirror selection is deprecated.
        /// </summary>
        public static string Mirror { get { return "http://thetvdb.com"; } }

        public string CacheLocation { get; set; }
        public bool UseCache { get; set; }

        private readonly TvdbServerTime _serverTime;

        public Tvdb(string apiKey, bool useCache = false, string cacheLocation = null)
        {
            ApiKey = apiKey;
            UseCache = useCache;
            CacheLocation = cacheLocation;

            _serverTime = new TvdbServerTime(UserAgent, UseCache, CacheLocation);
            
        }

        internal TvdbSeriesSearch SearchSeries(string name)
        {
            var seriesSearch = new TvdbSeriesSearch(name, UseCache, CacheLocation);
            return seriesSearch;
        }

        public delegate TvdbSeries SelectCallback(TvdbSeriesSearch seriesSearch);
        public static event SelectCallback SeriesSelectCallback;

        public TvdbSeries SelectSeries(TvdbSeriesSearch seriesList)
        {
            return SeriesSelectCallback != null ? SeriesSelectCallback(seriesList) : seriesList[0];
        }

        public TvdbDetailedSeries LookupSeries(TvdbSeries series)
        {
            throw new NotImplementedException();
        }

        public TvdbDetailedSeries LookupSeries(uint seriesId)
        {
            throw new NotImplementedException();
        }
    }
}
