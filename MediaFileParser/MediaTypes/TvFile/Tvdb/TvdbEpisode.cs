using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace MediaFileParser.MediaTypes.TvFile.Tvdb
{
    /// <summary>
    /// Categories that an episode image could be.
    /// </summary>
    public enum TvdbEpisodeImageFlag
    {
        /// <summary>
        /// Indicates an image is a proper 4:3 (1.31 to 1.35) aspect ratio.
        /// </summary>
        Proper4By3 = 1,
        /// <summary>
        /// Indicates an image is a proper 16:9 (1.739 to 1.818) aspect ratio. 
        /// </summary>
        Proper16By9 = 2,
        /// <summary>
        /// Indicates anything not in a 4:3 or 16:9 ratio. TVDB doesn't bother listing any other non standard ratios.
        /// </summary>
        InvalidAspectRatio = 3,
        /// <summary>
        /// Image is smaller then 300x170.
        /// </summary>
        ImageTooSmall = 4,
        /// <summary>
        /// Indicates there are black bars along one or all four sides of the image.
        /// </summary>
        BlackBars = 5,
        /// <summary>
        /// Could mean a number of things, usually used when someone uploads a promotional picture that isn't actually
        /// from that episode but does refrence the episode, it could also mean it's a credit shot or that there is
        /// writting all over it. It's rarely used. 
        /// </summary>
        ImproperActionShot = 6
    }

    /// <summary>
    /// TV Show individual episode data from the TVDB.
    /// </summary>
    [Serializable, ReadOnly(true), XmlType("Episode", AnonymousType = true)]
    public class TvdbEpisode
    {
        /// <summary>
        /// This class should only be deserialised, not instantiated directly.
        /// </summary>
        protected TvdbEpisode() {}

        /// <summary>
        /// An unsigned integer assigned by the TVDB to the episode. Cannot be null.
        /// </summary>
        [XmlElement(ElementName = "id")]
        public uint TvdbId { get; set; }

        /// <summary>
        /// This returns the value of DvdEpisodeNumber if that field is not null.
        /// Otherwise it returns the value from EpisodeNumber. Cannot be null.
        /// </summary>
        [XmlElement(ElementName = "Combined_episodenumber")]
        public decimal CombinedEpisodeNumber { get; set; }

        /// <summary>
        /// This returns the value of DVD_season if that field is not null.
        /// Otherwise it returns the value from SeasonNumber. Cannot be null.
        /// </summary>
        [XmlElement(ElementName = "Combined_season")]
        public uint CombinedSeason { get; set; }

        /// <summary>
        /// Was meant to be used to aid in scrapping of actual DVD's but has never been populated properly.
        /// Any information returned in this field shouldn't be trusted. Will usually be null. 
        /// 
        /// This field has been deprecated and should not be trusted.
        /// </summary>
        [XmlElement(ElementName = "DVD_chapter")]
        public string DvdChapter { get; set; }

        /// <summary>
        /// Was meant to be used to aid in scrapping of actual DVD's but has never been populated properly.
        /// Any information returned in this field shouldn't be trusted. Will usually be null.
        /// 
        /// This field has been deprecated and should not be trusted.
        /// </summary>
        [XmlElement(ElementName = "DVD_discid")]
        public string DvdDiscId { get; set; }

        /// <summary>
        /// Used to join episodes that aired as two episodes but were released on DVD as a single episode.
        /// Can be null. If you see an episode 1.1 and 1.2 that means both records should be combined to make episode 1.
        /// </summary>
        [XmlElement(ElementName = "DVD_episodenumber")]
        public string DvdEpisodeNumberString { get; set; }

        /// <summary>
        /// Used to join episodes that aired as two episodes but were released on DVD as a single episode.
        /// If you see an episode 1.1 and 1.2 that means both records should be combined to make episode 1.
        /// Throws an exception if null.
        /// </summary>
        [XmlIgnore]
        public double DvdEpisodeNumber { get { return double.Parse(DvdEpisodeNumberString); } }

        /// <summary>
        /// An unsigned integer indicating the season the episode was in according to the DVD release.
        /// Usually is the same as SeasonNumber but can be different. Can be null. 
        /// </summary>
        [XmlElement(ElementName = "DVD_season")]
        public string DvdSeasonString { get; set; }

        /// <summary>
        /// An unsigned integer indicating the season the episode was in according to the DVD release.
        /// Usually is the same as SeasonNumber but can be different. Throws an exception if null. 
        /// </summary>
        [XmlIgnore]
        public uint DvdSeason { get { return uint.Parse(DvdSeasonString); } }

        /// <summary>
        /// A pipe delimited string of directors in plain text. Can be null.
        /// </summary>
        [XmlElement(ElementName = "Director")]
        public string Director { get; set; }

        /// <summary>
        /// Array of all directors that produced the episode. Throws an exception if null.
        /// </summary>
        [XmlIgnore]
        public string[] Directors { get { return Director.Split('|'); } }

        /// <summary>
        /// Category that the current episode image falls in to (as a string).
        /// </summary>
        [XmlElement(ElementName = "EpImgFlag")]
        public string EpisodeImageFlagString
        {
            get { return ((int) EpisodeImageFlag).ToString(); }
            set
            {
                TvdbEpisodeImageFlag flag;
                EpisodeImageFlag = Enum.TryParse(value, out flag) ? flag : TvdbEpisodeImageFlag.ImproperActionShot;
            }
        }

        /// <summary>
        /// Category that the current episode image falls in to. Defaults to ImproperActionShot.
        /// </summary>
        [XmlIgnore]
        public TvdbEpisodeImageFlag EpisodeImageFlag { get; set; }

        /// <summary>
        /// Episode name in the current API localisation.
        /// </summary>
        [XmlElement("EpisodeName")]
        public string EpisodeName { get; set; }

        /// <summary>
        /// Episode number in its season. Cannot be null.
        /// </summary>
        [XmlElement(ElementName = "EpisodeNumber")]
        public uint EpisodeNumber { get; set; }

        /// <summary>
        /// The date the series first aired in plain text using the format "YYYY-MM-DD". Can be null. 
        /// </summary>
        [XmlElement(ElementName = "FirstAired")]
        public string FirstAiredString { get; set; }

        /// <summary>
        /// The date the series first aired. Throws an exception if null.
        /// </summary>
        [XmlIgnore]
        public DateTime FirstAiredDate { get { return DateTime.Parse(FirstAiredString); } }

        /// <summary>
        /// A pipe delimited string of guest stars in plain text. Can be null.
        /// </summary>
        [XmlElement(ElementName = "GuestStars")]
        public string GuestStarsString { get; set; }

        /// <summary>
        /// Guest stars in this episode. Throws an exception if null.
        /// </summary>
        [XmlIgnore]
        public string[] GuestStars { get { return GuestStarsString.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries); } }

        /// <summary>
        /// An alphanumeric string containing the IMDB ID for the series. Can be null. 
        /// </summary>
        [XmlElement(ElementName = "IMDB_ID")]
        public string ImdbId { get; set; }

        /// <summary>
        /// A two character string indicating the language in accordance with ISO-639-1. Cannot be null. 
        /// </summary>
        [XmlElement(ElementName = "Language")]
        public string Language { get; set; }

        /// <summary>
        /// Short description of the content of the episode. Can be null.
        /// </summary>
        [XmlElement(ElementName = "Overview")]
        public string Synopsis { get; set; }

        /// <summary>
        /// An alphanumeric string. Can be null. 
        /// </summary>
        [XmlElement(ElementName = "ProductionCode")]
        public string ProductionCode { get; set; }

        /// <summary>
        /// The average rating TVDB users have rated the series out of 10, rounded to 1 decimal place. Can be null. 
        /// </summary>
        [XmlElement(ElementName = "Rating")]
        public string RatingString { get; set; }

        /// <summary>
        /// The average rating TVDB users have rated the series out of 10, rounded to 1 decimal place. Throws an exception if null. 
        /// </summary>
        [XmlIgnore]
        public float Rating { get { return float.Parse(RatingString);  } }

        /// <summary>
        /// Number of TVDB users who have rated the series. Can be null. 
        /// </summary>
        [XmlElement(ElementName = "RatingCount")]
        public string RatingCountString { get; set; }

        /// <summary>
        /// Number of TVDB users who have rated the series. Throws an exception if null. 
        /// </summary>
        [XmlIgnore]
        public uint RatingCount { get { return uint.Parse(RatingCountString); } }

        /// <summary>
        /// Season number for the episode according to the aired order. Cannot be null. 
        /// </summary>
        [XmlElement(ElementName = "SeasonNumber")]
        public uint SeasonNumber { get; set; }

        /// <summary>
        /// A pipe delimited string of writers in plain text. Can be null. 
        /// </summary>
        [XmlElement(ElementName = "Writer")]
        public string WriterString { get; set; }

        /// <summary>
        /// Writers of this episode. Throws an exception if null.
        /// </summary>
        [XmlIgnore]
        public string[] Writers { get { return WriterString.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries); } }

        /// <summary>
        /// Indicates the absolute episode number (completely ignoring seasons). Can be null.
        /// </summary>
        [XmlElement("absolute_number")]
        public string AbsoluteNumberString { get; set; }

        /// <summary>
        /// Indicates the absolute episode number (completely ignoring seasons). Throws an exception if null.
        /// </summary>
        [XmlIgnore]
        public uint AbsoluteNumber { get { return uint.Parse(AbsoluteNumberString); } }

        /// <summary>
        /// Indicates the season number this episode comes after. This field is only available for special episodes. Can be null.
        /// </summary>
        [XmlElement(ElementName = "airsafter_season")]
        public string AirsAfterSeasonString { get; set; }

        /// <summary>
        /// Indicates the season number this episode comes after. This field is only available for special episodes. Throws an exception if null.
        /// </summary>
        [XmlIgnore]
        public uint AirsAfterSeason { get { return uint.Parse(AirsAfterSeasonString); } }

        /// <summary>
        /// Indicates the episode number this special episode airs before. Used in conjunction with AirsBeforeSeason,
        /// but not with AirsAfterSeason. This field is only available for special episodes. Can be null. 
        /// </summary>
        [XmlElement(ElementName = "airsbefore_episode")]
        public string AirsBeforeEpisodeString { get; set; }

        /// <summary>
        /// Indicates the episode number this special episode airs before. Used in conjunction with AirsBeforeSeason,
        /// but not with AirsAfterSeason. This field is only available for special episodes. Throws an exception if null.
        /// </summary>
        [XmlIgnore]
        public uint AirsBeforeEpisode { get { return uint.Parse(AirsBeforeEpisodeString); } }

        /// <summary>
        /// Indicates the season number this special episode airs before. Used in conjunction with AirsBeforeEpisode
        /// for exact placement. This field is only available for special episodes. Can be null. 
        /// </summary>
        [XmlElement(ElementName = "airsbefore_season")]
        public string AirsBeforeSeasonString { get; set; }

        /// <summary>
        /// Indicates the season number this special episode airs before. Used in conjunction with AirsBeforeEpisode
        /// for exact placement. This field is only available for special episodes. Throws an exception if null.
        /// </summary>
        [XmlIgnore]
        public uint AirsBeforeSeason { get { return uint.Parse(AirsBeforeSeasonString); } }

        /// <summary>
        /// A string which should be appended to http://thetvdb.com/banners/ to determine the
        /// actual location of the artwork. Returns the location of the episode image. Can be null.
        /// </summary>
        [XmlElement(ElementName = "filename")]
        public string FileName { get; set; }

        /// <summary>
        /// Unix time stamp indicating the last time any changes were made to the episode. Can be null.
        /// </summary>
        [XmlElement(ElementName = "lastupdated")]
        public uint LastUpdated { get; set; }

        /// <summary>
        /// An unsigned integer assigned by the TVDB to the season. Cannot be null.
        /// </summary>
        [XmlElement(ElementName = "seasonid")]
        public uint SeasonId { get; set; }

        /// <summary>
        /// An unsigned integer assigned by the TVDB to the series. It does not change and will
        /// always represent the same series. Cannot be null.
        /// </summary>
        [XmlElement(ElementName = "seriesid")]
        public uint SeriesId { get; set; }

        /// <summary>
        /// Time the episode image was added to the TVDB in the format "YYYY-MM-DD HH:MM:SS"
        /// based on a 24 hour clock. Can be null.
        /// </summary>
        [XmlElement(ElementName = "thumb_added")]
        public string ThumbnailAddedString { get; set; }

        /// <summary>
        /// Date/time the episode image was added to the TVDB. Throws an exception if null.
        /// </summary>
        [XmlIgnore]
        public DateTime ThumbnailAdded { get { return DateTime.Parse(ThumbnailAddedString); } }

        /// <summary>
        /// Represents the height of the episode image in pixels. Can be null.
        /// </summary>
        [XmlElement(ElementName = "thumb_height")]
        public string ThumbnailHeightString { get; set; }

        /// <summary>
        /// Represents the height of the episode image in pixels. Throws an exception if null.
        /// </summary>
        [XmlIgnore]
        public uint ThumbnailHeight { get { return uint.Parse(ThumbnailHeightString); } }

        /// <summary>
        /// Represents the width of the episode image in pixels. Can be null.
        /// </summary>
        [XmlElement(ElementName = "thumb_width")]
        public string ThumbnailWidthString { get; set; }

        /// <summary>
        /// Represents the width of the episode image in pixels. Throws an exception if null.
        /// </summary>
        [XmlIgnore]
        public uint ThumbnailWidth { get { return uint.Parse(ThumbnailWidthString); } }
    }
}