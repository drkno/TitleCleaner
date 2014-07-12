using System.Linq;
using System.Xml.Serialization;

namespace MediaFileParser.MediaFile.TvFile.Tvdb
{
    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class TvdbDetailedSeries
    {
        public TvdbSeriesExtended Series { get; set; }

        [XmlElementAttribute("Episode")]
        public TvdbEpisode[] Episode { get; set; }

        internal TvdbEpisode GetEpisode(uint season, uint episode)
        {
            return Episode.FirstOrDefault(tvdbEpisode => tvdbEpisode.SeasonNumber == season && tvdbEpisode.EpisodeNumber == episode);
        }
    }

    [XmlTypeAttribute(AnonymousType = true)]
    public class TvdbSeriesExtended : TvdbSeries
    {
        public string Actors { get; set; }
        [XmlElementAttribute(ElementName = "Airs_DayOfWeek")]
        public string AirsDayOfWeek { get; set; }
        [XmlElementAttribute(ElementName = "Airs_Time")]
        public string AirsTime { get; set; }
        public string ContentRating { get; set; }
        public string Genre { get; set; }
        [XmlElementAttribute(ElementName = "NetworkID")]
        public string NetworkId { get; set; }
        public decimal Rating { get; set; }
        public uint RatingCount { get; set; }
        public byte Runtime { get; set; }
        public string Status { get; set; }
        [XmlElementAttribute(ElementName = "added")]
        public string Added { get; set; }
        [XmlElementAttribute(ElementName = "addedBy")]
        public string AddedBy { get; set; }
        [XmlElementAttribute(ElementName = "fanart")]
        public string Fanart { get; set; }
        [XmlElementAttribute(ElementName = "lastupdated")]
        public uint LastUpdated { get; set; }
        [XmlElementAttribute(ElementName = "poster")]
        public string Poster { get; set; }
        [XmlElementAttribute(ElementName = "tms_wanted_old")]
        public byte TmsWantedOld { get; set; }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public class TvdbEpisode
    {
        [XmlElementAttribute(ElementName = "id")]
        public uint Id { get; set; }
        [XmlElementAttribute(ElementName = "Combined_episodenumber")]
        public decimal CombinedEpisodeNumber { get; set; }
        [XmlElementAttribute(ElementName = "Combined_season")]
        public byte CombinedSeason { get; set; }
        [XmlElementAttribute(ElementName = "DVD_chapter")]
        public string DvdChapter { get; set; }
        [XmlElementAttribute(ElementName = "DVD_discid")]
        public string DvdDiscId { get; set; }
        [XmlElementAttribute(ElementName = "DVD_episodenumber")]
        public string DvdEpisodeNumber { get; set; }
        [XmlElementAttribute(ElementName = "DVD_season")]
        public string DvdSeason { get; set; }
        public string Director { get; set; }
        public string EpImgFlag { get; set; }
        public string EpisodeName { get; set; }
        public byte EpisodeNumber { get; set; }
        public string FirstAired { get; set; }
        public string GuestStars { get; set; }
        [XmlElementAttribute(ElementName = "IMDB_ID")]
        public string ImdbId { get; set; }
        public string Language { get; set; }
        public string Overview { get; set; }
        public string ProductionCode { get; set; }
        public string Rating { get; set; }
        public byte RatingCount { get; set; }
        public byte SeasonNumber { get; set; }
        public string Writer { get; set; }
        [XmlElementAttribute(ElementName = "absolute_number")]
        public string AbsoluteNumber { get; set; }
        [XmlElementAttribute(ElementName = "airsafter_season")]
        public string AirsAfterSeason { get; set; }
        [XmlElementAttribute(ElementName = "airsbefore_episode")]
        public string AirsBeforeEpisode { get; set; }
        [XmlElementAttribute(ElementName = "airsbefore_season")]
        public string AirsBeforeSeason { get; set; }
        [XmlElementAttribute(ElementName = "filename")]
        public string FileName { get; set; }
        [XmlElementAttribute(ElementName = "lastupdated")]
        public uint LastUpdated { get; set; }
        [XmlElementAttribute(ElementName = "seasonid")]
        public uint SeasonId { get; set; }
        [XmlElementAttribute(ElementName = "seriesid")]
        public uint SeriesId { get; set; }
        [XmlElementAttribute(ElementName = "thumb_added")]
        public string ThumbAdded { get; set; }
        [XmlElementAttribute(ElementName = "thumb_height")]
        public string ThumbHeight { get; set; }
        public string ThumbWidth { get; set; }
    }
}
