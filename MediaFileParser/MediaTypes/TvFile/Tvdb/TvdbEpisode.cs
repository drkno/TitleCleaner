using System.Xml.Serialization;

namespace MediaFileParser.MediaTypes.TvFile.Tvdb
{
    [XmlType("Episode", AnonymousType = true)]
    public class TvdbEpisode
    {
        protected TvdbEpisode() { }

        [XmlElement(ElementName = "id")]
        public uint Id { get; set; }

        [XmlElement(ElementName = "Combined_episodenumber")]
        public decimal CombinedEpisodeNumber { get; set; }

        [XmlElement(ElementName = "Combined_season")]
        public uint CombinedSeason { get; set; }

        [XmlElement(ElementName = "DVD_chapter")]
        public string DvdChapter { get; set; }

        [XmlElement(ElementName = "DVD_discid")]
        public string DvdDiscId { get; set; }

        [XmlElement(ElementName = "DVD_episodenumber")]
        public string DvdEpisodeNumber { get; set; }

        [XmlElement(ElementName = "DVD_season")]
        public string DvdSeason { get; set; }

        [XmlElement(ElementName = "Director")]
        public string Director { get; set; }

        [XmlElement(ElementName = "EpImgFlag")]
        public string EpImgFlag { get; set; }

        [XmlElement(ElementName = "EpisodeName")]
        public string EpisodeName { get; set; }

        [XmlElement(ElementName = "EpisodeNumber")]
        public uint EpisodeNumber { get; set; }

        [XmlElement(ElementName = "FirstAired")]
        public string FirstAired { get; set; }

        [XmlElement(ElementName = "GuestStars")]
        public string GuestStars { get; set; }

        [XmlElement(ElementName = "IMDB_ID")]
        public string ImdbId { get; set; }

        [XmlElement(ElementName = "Language")]
        public string Language { get; set; }

        [XmlElement(ElementName = "Overview")]
        public string Overview { get; set; }

        [XmlElement(ElementName = "ProductionCode")]
        public string ProductionCode { get; set; }

        [XmlElement(ElementName = "Rating")]
        public string Rating { get; set; }

        [XmlElement(ElementName = "RatingCount")]
        public uint RatingCount { get; set; }

        [XmlElement(ElementName = "SeasonNumber")]
        public uint SeasonNumber { get; set; }

        [XmlElement(ElementName = "Writer")]
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

        [XmlElement(ElementName = "thumb_width")]
        public string ThumbWidth { get; set; }
    }
}