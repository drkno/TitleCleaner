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
        /// ID used by the TVDB to uniquely identify this TV Show.
        /// </summary>
        [XmlIgnore]
        public uint TvdbId { get; protected set; }

        /// <summary>
        /// ID used by the TVDB to uniquely identify this TV Show.
        /// Used for compatibility with older XML files, use TvdbId instead.
        /// </summary>
        [XmlElement(ElementName = "id"), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public string ObsoleteSerializeId
        {
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    TvdbId = uint.Parse(value);
                }
            }
            get { return TvdbId.ToString(); }
        }

        /// <summary>
        /// ID used by the TVDB to uniquely identify this TV Show series.
        /// Used for compatibility with older XML files, use TvdbId instead.
        /// </summary>
        [XmlElement("SeriesID"), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public string ObsoleteSerializeSeriesId
        {
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    TvdbId = uint.Parse(value);
                }
            }
            get { return TvdbId.ToString(); }
        }

        /// <summary>
        /// Sets the ID used by the TVDB to uniquely identify this TV Show series.
        /// Used for compatibility with older XML files, use TvdbId instead.
        /// </summary>
        [XmlElement("seriesid"), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public string ObsoleteSerializeSeriesIdLower
        {
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    TvdbId = uint.Parse(value);
                }
            }
            get { return TvdbId.ToString(); }
        }

        /// <summary>
        /// Language of the TV Show that this TVDB record refers to.
        /// </summary>
        [XmlElement(ElementName = "Language")]
        public string Language { get; set; }

        /// <summary>
        /// Language of the TV Show that this TVDB record refers to.
        /// </summary>
        [XmlElement("language"), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public string ObsoleteSerializeLanguage { set { Language = value; } get { return Language; } }

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
    }
}
