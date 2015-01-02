using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace MediaFileParser.MediaTypes.TvFile.Tvdb
{
    /// <summary>
    /// Common attributes returned from different API calls.
    /// </summary>
    [Serializable, ReadOnly(true), XmlType("Series", AnonymousType = true)]
    public class TvdbSeriesCommon
    {
        /// <summary>
        /// This class should be inherited, so constructor should
        /// not be called except in derived type.
        /// </summary>
        protected TvdbSeriesCommon() { }

        /// <summary>
        /// ID used by the TVDB to uniquely identify this TV Show series.
        /// </summary>
        [XmlElement("SeriesID")]
        public uint SeriesId { get; set; }

        /// <summary>
        /// Sets the ID used by the TVDB to uniquely identify this TV Show series.
        /// </summary>
        [XmlElement("seriesid"), Obsolete("Used for compatibility with older XML files, use SeriesId instead.")]
        public uint SeriesIdSetter { set { SeriesId = value; } }

        /// <summary>
        /// Language of the TV Show that this TVDB record refers to.
        /// </summary>
        [XmlElement(ElementName = "Language")]
        public string Language { get; set; }

        /// <summary>
        /// Language of the TV Show that this TVDB record refers to.
        /// </summary>
        [XmlElement("language"), Obsolete("Used for compatibility with older XML files, use Language instead.")]
        public string LanguageSetter { set { Language = value; } }

        /// <summary>
        /// Name of the series.
        /// </summary>
        [XmlElement(ElementName = "SeriesName")]
        public string SeriesName { get; set; }
        /// <summary>
        /// URL of a banner image for this TV Show.
        /// </summary>
        [XmlElement(ElementName = "banner")]
        public string Banner { get; set; }
        /// <summary>
        /// Description of this TV Show.
        /// </summary>
        [XmlElement(ElementName = "Overview")]
        public string Description { get; set; }
        /// <summary>
        /// Date that this TV Show first aired.
        /// </summary>
        [XmlElement(ElementName = "FirstAired")]
        public DateTime FirstAired { get; set; }
        /// <summary>
        /// Network that created this TV Show.
        /// </summary>
        [XmlElement(ElementName = "Network")]
        public string Network { get; set; }
        /// <summary>
        /// ID used by the IMDB to uniquely identify this TV Show.
        /// </summary>
        [XmlElement(ElementName = "IMDB_ID")]
        public string ImdbId { get; set; }
        /// <summary>
        /// ID used by the Zap2It to uniquely identify this TV Show.
        /// </summary>
        [XmlElement(ElementName = "zap2it_id")]
        public string Zap2ItId { get; set; }
        /// <summary>
        /// ID used by the TVDB to uniquely identify this TV Show.
        /// </summary>
        [XmlElement(ElementName = "id")]
        public uint TvdbId { get; set; }
    }
}
