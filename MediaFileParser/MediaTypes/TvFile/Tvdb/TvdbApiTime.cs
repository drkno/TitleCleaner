using System.Xml.Serialization;

namespace MediaFileParser.MediaTypes.TvFile.Tvdb
{
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false, ElementName = "Items")]
    public class TvdbApiTime
    {
        protected TvdbApiTime() { }

        [XmlElement("Time")]
        public uint Time { get; set; }

        public static TvdbApiTime TvdbServerTime()
        {
            return TvdbApiRequest.PerformApiRequestAndDeserialize<TvdbApiTime>(GetUpdateUrl());
        }

        private static string GetUpdateUrl()
        {
            return "Updates.php?type=none";
        }
    }
}
