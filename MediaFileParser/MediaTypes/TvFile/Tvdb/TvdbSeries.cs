using System;
using System.Xml.Serialization;

namespace MediaFileParser.MediaTypes.TvFile.Tvdb
{
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class TvdbSeries
    {
        [XmlElement(ElementName = "seriesid")]
        public uint SeriesId { get; set; }
        [XmlElement(ElementName = "language")]
        public string Language { get; set; }
        public string SeriesName { get; set; }
        [XmlElement(ElementName = "banner")]
        public string Banner { get; set; }
        public string Overview { get; set; }
        [XmlElement(DataType = "date")]
        public DateTime FirstAired { get; set; }
        [XmlIgnore]
        public bool FirstAiredSpecified { get; set; }
        public string Network { get; set; }
        [XmlElement(ElementName = "IMDB_ID")]
        public string ImdbId { get; set; }
        [XmlElement(ElementName = "zap2it_id")]
        public string Zap2ItId { get; set; }
        [XmlElement(ElementName = "Id")]
        public uint Id { get; set; }
    }
}
