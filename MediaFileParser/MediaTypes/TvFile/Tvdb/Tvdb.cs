using System;
using System.Diagnostics;
using MediaFileParser.MediaTypes.TvFile.Tvdb.Cache;

namespace MediaFileParser.MediaTypes.TvFile.Tvdb
{
    /// <summary>
    /// Class to help with the usage of the various TVDB Api classes.
    /// Not strictly required but useful.
    /// </summary>
    public class Tvdb
    {
        /// <summary>
        /// API key used for API requests
        /// </summary>
        public string ApiKey { get { return TvdbApiRequest.ApiKey; } }

        /// <summary>
        /// Gets the type of cache that will be used for each consecutive lookup.
        /// </summary>
        public TvdbCacheType CacheType
        {
            get { return TvdbApiRequest.CacheProvider.CacheType; }
        }

        /// <summary>
        /// Gets the persistent cache storage location. If this is not specified the
        /// persistent cache will be in the same directory as the executable.
        /// </summary>
        public string PersistentCacheLocation
        {
            get { return TvdbApiRequest.CacheProvider.PersistentCacheLocation; }
        }

        /// <summary>
        /// Minimum time between cache update checks in seconds. Defaults to 86400 seconds (24hrs).
        /// </summary>
        public uint CacheTimeout
        {
            get { return TvdbApiRequest.CacheTimeout; }
            set { TvdbApiRequest.CacheTimeout = value; }
        }

        /// <summary>
        /// Request object to use for each successive API call.
        /// </summary>
        public TvdbApiRequest TvdbApiRequest { get; protected set; }

        /// <summary>
        /// Instantiates a new copy of the TVDB API helper class
        /// </summary>
        /// <param name="apiKey">API Key to use for lookups</param>
        /// <param name="cacheType">Type of cache to use. Defaults to memory and persistent.</param>
        /// <param name="persistentCacheLocation">Location of persistent cache (if one is to be used).</param>
        public Tvdb(string apiKey, TvdbCacheType cacheType = TvdbCacheType.PersistentMemory, string persistentCacheLocation = null)
        {
            Debug.WriteLine("-> Tvdb::_cstr apiKey=\"" + apiKey + "\" cacheType=\"" + cacheType + "\" persistentCacheLocation=\"" + persistentCacheLocation + "\" Called");
            // Create the cache
            var tvdbCacheProvider = new TvdbCacheProvider(cacheType, persistentCacheLocation);
            // Create request object
            TvdbApiRequest = new TvdbApiRequest(apiKey, tvdbCacheProvider);
        }

        /// <summary>
        /// Search for a series by name.
        /// </summary>
        /// <param name="series">Series to search for.</param>
        /// <returns>An array of undetailed series information. Null if failure.</returns>
        public TvdbSeries[] Search(string series)
        {
            Debug.WriteLine("-> Tvdb::Search series=\"" + series + "\" Called");
            return TvdbSeries.GetTvdbSeriesSearch(series, TvdbApiRequest);
        }

        /// <summary>
        /// Retreive a series by its TVDB id.
        /// </summary>
        /// <param name="id">ID to lookup</param>
        /// <returns>Detailed series information or null if failure.</returns>
        public TvdbDetailedSeries LookupId(uint id)
        {
            Debug.WriteLine("-> Tvdb::LookupId id=\"" + id + "\" Called");
            return TvdbDetailedSeries.GetDetailedSeries(id, TvdbApiRequest);
        }

        /// <summary>
        /// Retreive a specific episode of a tvdb series.
        /// WARNING:
        /// Will retreive all series information and episodes when used
        /// so that API usage is minimised. DO NOT USE if you plan to
        /// do this multiple times AND CacheType is set to None.
        /// </summary>
        /// <param name="id">Series TVDB id.</param>
        /// <param name="season">Season number.</param>
        /// <param name="episode">Episode number.</param>
        /// <returns>The episode or null if not found.</returns>
        public TvdbEpisode LookupEpisode(uint id, uint season, uint episode)
        {
            Debug.WriteLine("-> Tvdb::LookupEpisode id=\"" + id + "\" season=\"" + season + "\" episode=\"" + episode + "\" Called");
            try
            {
                var series = LookupId(id);
                return series.GetEpisode(season, episode);
            }
            catch (Exception e)
            {
                Debug.WriteLine("!> Tvdb::LookupEpisode threw an exception: " + e);
                return null;
            }
        }

        /// <summary>
        /// Performs an API update request if appropriate. This is controlled by the API timeout key.
        /// </summary>
        /// <returns>If updates were made.</returns>
        public bool UpdateCache()
        {
            Debug.WriteLine("-> Tvdb::UpdateCache Called");
            return TvdbApiRequest.PerformCacheUpdate();
        }
    }
}
