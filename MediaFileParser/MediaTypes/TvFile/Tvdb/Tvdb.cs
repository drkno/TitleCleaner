using System;

namespace MediaFileParser.MediaTypes.TvFile.Tvdb
{
    /// <summary>
    /// Class to help with the usage of the various TVDB Api classes.
    /// Not strictly required but useful.
    /// </summary>
    public class Tvdb
    {
        /// <summary>
        /// Store the API key in an appropriate location
        /// </summary>
        protected string ApiKey
        {
            set { TvdbDetailedSeries.ApiKey = value; }
        }

        /// <summary>
        /// Server time for API caching. TODO: Implement
        /// </summary>
        public TvdbApiTime ServerTime { get; protected set; }

        /// <summary>
        /// Sets if each consecutive lookup will be stored in a memory cache.
        /// Disable this for low memory platforms as is memory intensive.
        /// </summary>
        public bool UseCache
        {
            get { return TvdbApiRequest.UseCache; }
            set { TvdbApiRequest.UseCache = value; }
        }

        /// <summary>
        /// Instantiates a new copy of the TVDB API helper class
        /// </summary>
        /// <param name="apiKey">API Key to use for lookups</param>
        /// <param name="useCache">Use the memory lookup cache</param>
        public Tvdb(string apiKey, bool useCache = true)
        {
            // Set the key.
            ApiKey = apiKey;
            // Must have the server time as a part of the API spec.
            ServerTime = TvdbApiTime.TvdbServerTime();
            // Set if the cache will be enabled.
            UseCache = useCache;
        }

        /// <summary>
        /// Search for a series by name.
        /// </summary>
        /// <param name="series">Series to search for.</param>
        /// <returns>An array of undetailed series information. Null if failure.</returns>
        public TvdbSeries[] Search(string series)
        {
            return TvdbSeries.GetTvdbSeriesSearch(series);
        }

        /// <summary>
        /// Retreive a series by its TVDB id.
        /// </summary>
        /// <param name="id">ID to lookup</param>
        /// <returns>Detailed series information or null if failure.</returns>
        public TvdbDetailedSeries LookupId(uint id)
        {
            return TvdbDetailedSeries.GetDetailedSeries(id);
        }

        /// <summary>
        /// Retreive a specific episode of a tvdb series.
        /// WARNING:
        /// Will retreive all series information and episodes when used
        /// so that API usage is minimised. DO NOT USE if you plan to
        /// do this multiple times AND UseCache is set to false.
        /// </summary>
        /// <param name="id">Series TVDB id.</param>
        /// <param name="season">Season number.</param>
        /// <param name="episode">Episode number.</param>
        /// <returns>The episode or null if not found.</returns>
        public TvdbEpisode LookupEpisode(uint id, uint season, uint episode)
        {
            try
            {
                var series = LookupId(id);
                return series.GetEpisode(season, episode);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
