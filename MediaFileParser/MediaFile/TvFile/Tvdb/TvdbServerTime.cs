using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Xml.Serialization;

namespace MediaFileParser.MediaFile.TvFile.Tvdb
{
    internal class TvdbServerTime
    {
        public uint Time { get; protected set; }
        public uint[] Series { get; protected set; }

        public TvdbServerTime(string userAgent, bool useCache, string settingLocation = null)
        {
            if (useCache)
            {
                if (string.IsNullOrWhiteSpace(settingLocation))
                {
                    settingLocation = Environment.CurrentDirectory;
                }
                settingLocation += Path.PathSeparator + "TVDBUpdate.ini";

                try
                {
                    var streamReader = new StreamReader(settingLocation);
                    var line = streamReader.ReadLine();
                    if (!string.IsNullOrWhiteSpace(line)) Time = uint.Parse(line);
                    streamReader.Close();
                }
                catch (Exception e)
                {
                    Debug.WriteLine("TVDB Cache Read Error: " + settingLocation + ", " + e.Message);
                }
            }

            var webRequest = (HttpWebRequest)WebRequest.Create("http://thetvdb.com/api/Updates.php?type="+((useCache && Time != 0)?"series&time="+Time:"none"));
            webRequest.UserAgent = userAgent;
            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
            {
                var ser = new XmlSerializer(typeof(TvdbXmlUpdateParser));
                var stream = webResponse.GetResponseStream();
                if (stream == null) throw new NullReferenceException("Update request returned nothing.");
                var updates = (TvdbXmlUpdateParser)ser.Deserialize(stream);
                Time = updates.Time;
                Series = updates.Series;
            }

            if (!useCache) return;
            var streamWriter = new StreamWriter(settingLocation, false);
            streamWriter.WriteLine(Time);
            streamWriter.Close();
        }

        [XmlTypeAttribute(AnonymousType = true)]
        [XmlRootAttribute(Namespace = "", IsNullable = false)]
        public class TvdbXmlUpdateParser
        {
            public uint Time { get; set; }
            [XmlElement("Series")]
            public uint[] Series { get; set; }
        }
    }
}
