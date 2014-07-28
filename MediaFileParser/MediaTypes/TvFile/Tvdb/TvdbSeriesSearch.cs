using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Xml.Serialization;

namespace MediaFileParser.MediaTypes.TvFile.Tvdb
{
    public class TvdbSeriesSearch
    {
        protected TvdbSeries[] Series { get; set; }

        [XmlType(AnonymousType = true)]
        [XmlRoot(Namespace = "", IsNullable = false, ElementName = "Data")]
        public class Search
        {
            [XmlArray("Series"), XmlArrayItem(ElementName = "Series",Type = typeof(TvdbSeries))]
            public TvdbSeries[] Series;
        }
        
        public TvdbSeriesSearch(string name, string userAgent, bool useCache = false, string cacheLocation = null)
        {
            Search series;
            name = name.ToLower().Trim();
            if (useCache)
            {
                if (!string.IsNullOrWhiteSpace(cacheLocation))
                {
                    cacheLocation = Environment.CurrentDirectory;
                }
                cacheLocation += Path.DirectorySeparatorChar + "TVDBSearch - " + name + ".ini";

                if (File.Exists(cacheLocation))
                {
                    var streamReader = new StreamReader(cacheLocation);
                    var ser = new XmlSerializer(typeof(Search));
                    var stream = streamReader.BaseStream;
                    if (stream == null) throw new NullReferenceException("Update request returned nothing.");
                    series = (Search)ser.Deserialize(stream);
                    streamReader.Close();
                    Series = series.Series;
                    return;
                }
            }

            var webRequest = (HttpWebRequest)WebRequest.Create("http://thetvdb.com/api/GetSeries.php?seriesname="+name);
            webRequest.UserAgent = userAgent;
            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
            {
                var ser = new XmlSerializer(typeof(Search));
                var stream = webResponse.GetResponseStream();
                if (stream == null) throw new NullReferenceException("Update request returned nothing.");
                series = (Search)ser.Deserialize(stream);
            }

            if (useCache)
            {
                var streamWriter = new StreamWriter(cacheLocation, false);
                var serializer = new XmlSerializer(typeof(Search));
                serializer.Serialize(streamWriter, series);
                streamWriter.Close();
            }
            Series = series.Series;
        }

        public TvdbSeries this[int value]
        {
            get { return Series[value]; }
        }

        public int Length { get { return Series == null ? 0 : Series.Length; } }
        public IEnumerator GetEnumerator()
        {
            return Series.GetEnumerator();
        }
    }
}
