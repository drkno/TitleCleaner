using System;
using System.Xml.Serialization;

namespace MediaFileParser.MediaFile.TvFile.Tvdb
{
    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class TvdbSeries
    {
        [XmlElementAttribute(ElementName = "seriesid")]
        public uint SeriesId { get; set; }
        [XmlElementAttribute(ElementName = "language")]
        public string Language { get; set; }
        public string SeriesName { get; set; }
        [XmlElementAttribute(ElementName = "banner")]
        public string Banner { get; set; }
        public string Overview { get; set; }
        [XmlElementAttribute(DataType = "date")]
        public DateTime FirstAired { get; set; }
        [XmlIgnoreAttribute]
        public bool FirstAiredSpecified { get; set; }
        public string Network { get; set; }
        [XmlElementAttribute(ElementName = "IMDB_ID")]
        public string ImdbId { get; set; }
        [XmlElementAttribute(ElementName = "zap2it_id")]
        public string Zap2ItId { get; set; }
        [XmlElementAttribute(ElementName = "Id")]
        public uint Id { get; set; }
    }
}
