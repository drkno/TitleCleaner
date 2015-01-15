using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;
using MediaFileParser.MediaTypes.TvFile.Tvdb.Cache;

namespace MediaFileParser.MediaTypes.TvFile.Tvdb
{
    /// <summary>
    /// Detailed series data.
    /// </summary>
    [Serializable, XmlType(AnonymousType = true), XmlRoot("Data", Namespace = "", IsNullable = false)]
    public class TvdbDetailedSeries
    {
        /// <summary>
        /// Class should not be instantiated.
        /// </summary>
        protected TvdbDetailedSeries(){}

        /// <summary>
        /// Gets detailed information about a series.
        /// </summary>
        /// <param name="id">ID of series to get details for.</param>
        /// <param name="request">API request object to use.</param>
        /// <returns>Detailed series object.</returns>
        public static TvdbDetailedSeries GetDetailedSeries(uint id, TvdbApiRequest request)
        {
            Debug.WriteLine("-> TvdbDetailedSeries::GetDetailedSeries id=\"" + id + "\" request=\"" + request + "\" Called");
            var cacheKey = id + "~series";
            return request.PerformApiRequestAndDeserialize<TvdbDetailedSeries>(GetExtendedSeriesUrl(id, request.ApiKey), cacheKey);
        }

        /// <summary>
        /// Gets URL for retreiving detailed series information.
        /// </summary>
        /// <param name="id">ID of series to get details for.</param>
        /// <param name="apiKey">API key to use to generate URL.</param>
        /// <returns>API url for retreiving detailed series.</returns>
        private static string GetExtendedSeriesUrl(uint id, string apiKey)
        {
            Debug.WriteLine("-> TvdbDetailedSeries::GetExtendedSeriesUrl id=\"" + id + "\" apiKey=\"" + apiKey + "\" Called");
            return apiKey + "/series/" + id + "/all/en.xml";
        }

        /// <summary>
        /// Gets a specific episode.
        /// </summary>
        /// <param name="season">Season number to get.</param>
        /// <param name="episode">Episode number to get.</param>
        /// <returns>A TvdbEpisode representing the episode or null if one doesn't exist.</returns>
        public TvdbEpisode GetEpisode(uint season, uint episode)
        {
            Debug.WriteLine("-> TvdbDetailedSeries::GetEpisode season=\"" + season + "\" episode=\"" + episode + "\" Called");
            return Episodes.FirstOrDefault(tvdbEpisode => tvdbEpisode.EpisodeNumber == episode && tvdbEpisode.SeasonNumber == season);
        }

        /// <summary>
        /// Information about this series.
        /// </summary>
        [XmlElement("Series")]
        public TvdbSeriesExtended Series { get; set; }

        /// <summary>
        /// Episodes in this series.
        /// </summary>
        [XmlElement("Episode")]
        public TvdbEpisode[] Episodes { get; set; }

        /// <summary>
        /// Detailed series information.
        /// </summary>
        [Serializable, XmlType("Series", AnonymousType = true)]
        public class TvdbSeriesExtended : TvdbSeriesCommon
        {
            /// <summary>
            /// This class should not be instantiated.
            /// </summary>
            protected TvdbSeriesExtended() { }

            /// <summary>
            /// A pipe delimited string of actors in plain text. Begins and ends with a pipe even if no actors are listed. Cannot be null.
            /// </summary>
            [XmlElement("Actors")]
            public string ActorsString { get; set; }

            /// <summary>
            /// A pipe delimited string of actors in plain text. Begins and ends with a pipe even if no actors are listed. Throws an exception if null.
            /// </summary>
            [XmlIgnore]
            public string[] Actors { get { return ActorsString.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries); } }

            /// <summary>
            /// The full name in English for the day of the week the series airs in plain text. Can be null.
            /// </summary>
            [XmlElement("Airs_DayOfWeek")]
            public string AirsDayOfWeekString { get; set; }

            /// <summary>
            /// The full name in English for the day of the week the series airs in plain text. Throws an exception if null.
            /// </summary>
            [XmlIgnore]
            public DayOfWeek AirsDayOfWeek { get { return (DayOfWeek)Enum.Parse(typeof(DayOfWeek), AirsDayOfWeekString); } }

            /// <summary>
            /// A string indicating the time of day the series airs on its original network. Format "HH:MM AM/PM". Can be null.
            /// </summary>
            [XmlElement("Airs_Time")]
            public string AirsTimeString { get; set; }

            /// <summary>
            /// A string indicating the time of day the series airs on its original network. Throws an exception if null.
            /// </summary>
            [XmlElement("Airs_Time")]
            public TimeSpan AirsTime { get { return DateTime.Parse(AirsTimeString).TimeOfDay; } }

            /// <summary>
            /// The rating given to the series based on the US rating system. Can be null or a 4-5 character string.
            /// </summary>
            [XmlElement("ContentRating")]
            public string ContentRating { get; set; }

            /// <summary>
            /// Pipe delimited list of genres in plain text. Begins and ends with a | but may also be null.
            /// </summary>
            [XmlElement("Genre")]
            public string GenreString { get; set; }

            /// <summary>
            /// Pipe delimited list of genres in plain text. Throws an exception if null.
            /// </summary>
            [XmlIgnore]
            public string[] Genre { get { return GenreString.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries); } }

            /// <summary>
            /// Not in use. Defaults to 0. 
            /// </summary>
            [XmlIgnore]
            public uint NetworkId { get; protected set; }

            /// <summary>
            /// Not in use. Can be null. 
            /// </summary>
            [XmlElement("NetworkID"), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
            public string ObsoleteSerializeNetworkId
            {
                get { return NetworkId.ToString(); }
                set
                {
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        NetworkId = uint.Parse(value);
                    }
                }
            }

            /// <summary>
            /// The average rating our users have rated the series out of 10, rounded to 1 decimal place. Can be null. 
            /// </summary>
            [XmlElement("Rating")]
            public string RatingString { get; set; }

            /// <summary>
            /// The average rating our users have rated the series out of 10, rounded to 1 decimal place. Throws an exception if null. 
            /// </summary>
            [XmlIgnore]
            public float Rating { get { return float.Parse(RatingString); } }

            /// <summary>
            /// An unsigned integer representing the number of users who have rated the series. Can be null.
            /// </summary>
            [XmlElement("RatingCount")]
            public string RatingCountString { get; set; }

            /// <summary>
            /// Represents the number of users who have rated the series. Throws an exception if null.
            /// </summary>
            [XmlIgnore]
            public uint RatingCount { get { return uint.Parse(RatingCountString); } }

            /// <summary>
            /// An unsigned integer representing the runtime of the series in minutes. Can be null. 
            /// </summary>
            [XmlElement("Runtime")]
            public string RuntimeString { get; set; }

            /// <summary>
            /// Represents the runtime of the series in minutes. Throws an exception if null. 
            /// </summary>
            [XmlElement("Runtime")]
            public uint Runtime { get { return uint.Parse(RuntimeString); } }

            /// <summary>
            /// A string containing either "Ended" or "Continuing". Can be null. 
            /// </summary>
            [XmlElement("Status")]
            public string StatusString { get; set; }

            /// <summary>
            /// A string containing either "Ended" or "Continuing". Throws an exception if null. 
            /// </summary>
            [XmlIgnore]
            public TvdbSeriesStatus Status { get { return (TvdbSeriesStatus)Enum.Parse(typeof(TvdbSeriesStatus), StatusString); } }

            /// <summary>
            /// A string containing the date/time the series was added to the TVDB in the format "YYYY-MM-DD HH:MM:SS" based on a 24 hour clock. Is null for older series.
            /// </summary>
            [XmlElement("added")]
            public string AddedString { get; set; }

            /// <summary>
            /// A string containing the date/time the series was added to the TVDB in the format "YYYY-MM-DD HH:MM:SS" based on a 24 hour clock. Throws an exception if null.
            /// </summary>
            [XmlIgnore]
            public DateTime Added { get { return DateTime.Parse(AddedString); } }

            /// <summary>
            /// An unsigned integer. The ID of the user on our site who added the series to our database. Is null for older series.
            /// </summary>
            [XmlElement("addedBy")]
            public string AddedByString { get; set; }

            /// <summary>
            /// The ID of the user on our site who added the series to our database. Is null for older series.
            /// </summary>
            [XmlIgnore]
            public uint AddedBy { get { return uint.Parse(AddedByString); } }

            /// <summary>
            /// A string which should be appended to http://thetvdb.com/banners/ to determine the actual location of the artwork. Returns the highest voted fanart for the requested series. Can be null.
            /// </summary>
            [XmlElement("fanart")]
            public string Fanart { get; set; }

            /// <summary>
            /// Unix time stamp indicating the last time any changes were made to the series. Can be null.
            /// </summary>
            [XmlElement("lastupdated")]
            public string LastUpdatedString { get; set; }

            /// <summary>
            /// Last time any changes were made to the series. Throws an exception if null.
            /// </summary>
            [XmlIgnore]
            public DateTime LastUpdatedDateTime { get { return TvdbCacheProvider.EpochToDateTime(uint.Parse(LastUpdatedString)); } }

            /// <summary>
            /// A string which should be appended to http://thetvdb.com/banners/ to determine the actual location of the artwork. Returns the highest voted poster for the requested series. Can be null.
            /// </summary>
            [XmlElement("poster")]
            public string Poster { get; set; }

            /// <summary>
            /// TMS Wanted.
            /// </summary>
            [XmlElement("tms_wanted_old")]
            public uint TmsWantedOld { get; set; }
        }

        /// <summary>
        /// Current production status of the series.
        /// </summary>
        public enum TvdbSeriesStatus
        {
            /// <summary>
            /// Production has ended.
            /// </summary>
            Ended,
            /// <summary>
            /// Production is continuing.
            /// </summary>
            Continuing
        }
    }
}