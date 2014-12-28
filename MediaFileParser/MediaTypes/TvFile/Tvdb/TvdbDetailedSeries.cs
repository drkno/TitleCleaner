using System.Linq;
using System.Xml.Serialization;

namespace MediaFileParser.MediaTypes.TvFile.Tvdb
{
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false, ElementName = "Data")]
    public class TvdbDetailedSeries
    {
        protected TvdbDetailedSeries(){}

        [XmlIgnore]
        public static string ApiKey { set; protected get; }

        public static TvdbDetailedSeries GetDetailedSeries(uint id, bool ignoreCache)
        {
            return TvdbApiRequest.PerformApiRequestAndDeserialize<TvdbDetailedSeries>(GetExtendedSeriesUrl(id, ApiKey), ignoreCache);
        }

        private static string GetExtendedSeriesUrl(uint id, string apiKey)
        {
            return apiKey + "/series/" + id + "/all/en.xml";
        }

        public TvdbEpisode GetEpisode(uint season, uint episode)
        {
            return Episodes.FirstOrDefault(tvdbEpisode => tvdbEpisode.EpisodeNumber == episode && tvdbEpisode.SeasonNumber == season);
        }

        [XmlElement("Series")]
        public TvdbSeriesExtended Series { get; set; }

        [XmlElement("Episode")]
        public TvdbEpisode[] Episodes { get; set; }

        [XmlType("Series", AnonymousType = true)]
        public class TvdbSeriesExtended : TvdbSeriesCommon
        {
            protected TvdbSeriesExtended() { }

            [XmlElement(ElementName = "Actors")]
            public string Actors { get; set; }

            [XmlElement(ElementName = "Airs_DayOfWeek")]
            public string AirsDayOfWeek { get; set; }

            [XmlElement(ElementName = "Airs_Time")]
            public string AirsTime { get; set; }

            [XmlElement("ContentRating")]
            public string ContentRating { get; set; }

            [XmlElement("Genre")]
            public string Genre { get; set; }

            [XmlElement(ElementName = "NetworkID")]
            public string NetworkId { get; set; }

            [XmlElement("Rating")]
            public decimal Rating { get; set; }

            [XmlElement("RatingCount")]
            public uint RatingCount { get; set; }

            [XmlElement("Runtime")]
            public uint Runtime { get; set; }

            [XmlElement("Status")]
            public string Status { get; set; }

            [XmlElement(ElementName = "added")]
            public string Added { get; set; }

            [XmlElement(ElementName = "addedBy")]
            public string AddedBy { get; set; }

            [XmlElement(ElementName = "fanart")]
            public string Fanart { get; set; }

            [XmlElement(ElementName = "lastupdated")]
            public uint LastUpdated { get; set; }

            [XmlElement(ElementName = "poster")]
            public string Poster { get; set; }

            [XmlElement(ElementName = "tms_wanted_old")]
            public uint TmsWantedOld { get; set; }
        }
    }
}