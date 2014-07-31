using System;
using System.Xml.Serialization;

namespace tvdbApi
{
    [Serializable]
    [XmlType("Series", AnonymousType = true)]
    public class TvdbSeriesCommon
    {
        protected TvdbSeriesCommon() { }

        [XmlElement(ElementName = "seriesid")]
        public uint SeriesId { get; set; }
        [XmlElement(ElementName = "language")]
        public string Language { get; set; }
        [XmlElement(ElementName = "SeriesName")]
        public string SeriesName { get; set; }
        [XmlElement(ElementName = "banner")]
        public string Banner { get; set; }
        [XmlElement(ElementName = "Overview")]
        public string Overview { get; set; }
        [XmlElement(DataType = "date", ElementName = "FirstAired")]
        public DateTime FirstAired { get; set; }
        [XmlElement(ElementName = "Network")]
        public string Network { get; set; }
        [XmlElement(ElementName = "IMDB_ID")]
        public string ImdbId { get; set; }
        [XmlElement(ElementName = "zap2it_id")]
        public string Zap2ItId { get; set; }
        [XmlElement(ElementName = "id")]
        public uint Id { get; set; }
    }
}
