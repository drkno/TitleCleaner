using System.Linq;
using System.Xml.Serialization;

namespace MediaFileParser.MediaTypes.TvFile.Tvdb
{
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class TvdbDetailedSeries
    {
        public TvdbSeriesExtended Series { get; set; }

        [XmlElement("Episode")]
        public TvdbEpisode[] Episode { get; set; }

        internal TvdbEpisode GetEpisode(uint season, uint episode)
        {
            return Episode.FirstOrDefault(tvdbEpisode => tvdbEpisode.SeasonNumber == season && tvdbEpisode.EpisodeNumber == episode);
        }
    }

    [XmlType(AnonymousType = true)]
    public class TvdbSeriesExtended : TvdbSeries
    {
        public string Actors { get; set; }
        [XmlElement(ElementName = "Airs_DayOfWeek")]
        public string AirsDayOfWeek { get; set; }
        [XmlElement(ElementName = "Airs_Time")]
        public string AirsTime { get; set; }
        public string ContentRating { get; set; }
        public string Genre { get; set; }
        [XmlElement(ElementName = "NetworkID")]
        public string NetworkId { get; set; }
        public decimal Rating { get; set; }
        public uint RatingCount { get; set; }
        public byte Runtime { get; set; }
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
        public byte TmsWantedOld { get; set; }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true)]
    public class TvdbEpisode
    {
        [XmlElement(ElementName = "id")]
        public uint Id { get; set; }
        [XmlElement(ElementName = "Combined_episodenumber")]
        public decimal CombinedEpisodeNumber { get; set; }
        [XmlElement(ElementName = "Combined_season")]
        public byte CombinedSeason { get; set; }
        [XmlElement(ElementName = "DVD_chapter")]
        public string DvdChapter { get; set; }
        [XmlElement(ElementName = "DVD_discid")]
        public string DvdDiscId { get; set; }
        [XmlElement(ElementName = "DVD_episodenumber")]
        public string DvdEpisodeNumber { get; set; }
        [XmlElement(ElementName = "DVD_season")]
        public string DvdSeason { get; set; }
        public string Director { get; set; }
        public string EpImgFlag { get; set; }
        public string EpisodeName { get; set; }
        public byte EpisodeNumber { get; set; }
        public string FirstAired { get; set; }
        public string GuestStars { get; set; }
        [XmlElement(ElementName = "IMDB_ID")]
        public string ImdbId { get; set; }
        public string Language { get; set; }
        public string Overview { get; set; }
        public string ProductionCode { get; set; }
        public string Rating { get; set; }
        public byte RatingCount { get; set; }
        public byte SeasonNumber { get; set; }
        public string Writer { get; set; }
        [XmlElement(ElementName = "absolute_number")]
        public string AbsoluteNumber { get; set; }
        [XmlElement(ElementName = "airsafter_season")]
        public string AirsAfterSeason { get; set; }
        [XmlElement(ElementName = "airsbefore_episode")]
        public string AirsBeforeEpisode { get; set; }
        [XmlElement(ElementName = "airsbefore_season")]
        public string AirsBeforeSeason { get; set; }
        [XmlElement(ElementName = "filename")]
        public string FileName { get; set; }
        [XmlElement(ElementName = "lastupdated")]
        public uint LastUpdated { get; set; }
        [XmlElement(ElementName = "seasonid")]
        public uint SeasonId { get; set; }
        [XmlElement(ElementName = "seriesid")]
        public uint SeriesId { get; set; }
        [XmlElement(ElementName = "thumb_added")]
        public string ThumbAdded { get; set; }
        [XmlElement(ElementName = "thumb_height")]
        public string ThumbHeight { get; set; }
        public string ThumbWidth { get; set; }
    }
}
