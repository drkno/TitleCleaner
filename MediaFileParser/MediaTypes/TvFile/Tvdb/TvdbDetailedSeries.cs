using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Serialization;

namespace MediaFileParser.MediaTypes.TvFile.Tvdb
{
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false, ElementName = "Data")]
    public class TvdbDetailedSeries
    {
        [XmlElement("Series")]
        public TvdbSeriesExtended Series { get; set; }

        [XmlArray("Episode"), XmlArrayItem(ElementName = "Episode", Type = typeof(TvdbEpisode))]
        public TvdbEpisode[] Episode { get; set; }

        internal TvdbEpisode GetEpisode(uint season, uint episode)
        {
            return Episode.FirstOrDefault(tvdbEpisode => tvdbEpisode.SeasonNumber == season && tvdbEpisode.EpisodeNumber == episode);
        }

        public static TvdbDetailedSeries Lookup(uint seriesId, string userAgent, string apiKey, bool useCache = false, string cacheLocation = null, TvdbServerTime serverTime = null)
        {
            TvdbDetailedSeries series;
            if (useCache)
            {
                if (string.IsNullOrWhiteSpace(cacheLocation))
                {
                    cacheLocation = Environment.CurrentDirectory;
                }
                cacheLocation += Path.PathSeparator + "Tvdb - " + seriesId + ".ini";
                if (File.Exists(cacheLocation) && !serverTime.Series.Contains(seriesId))
                {
                    var streamReader = new StreamReader(cacheLocation);
                    var ser = new XmlSerializer(typeof(TvdbDetailedSeries));
                    var stream = streamReader.BaseStream;
                    if (stream == null) throw new NullReferenceException("Update request returned nothing.");
                    series = (TvdbDetailedSeries)ser.Deserialize(stream);
                    streamReader.Close();
                    return series;
                }
            }

            var webRequest = (HttpWebRequest)WebRequest.Create("http://thetvdb.com/api/"+apiKey+"/series/"+seriesId+"/all/en.xml");
            webRequest.UserAgent = userAgent;
            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
            {
                var ser = new XmlSerializer(typeof(TvdbDetailedSeries));
                var stream = webResponse.GetResponseStream();
                if (stream == null) throw new NullReferenceException("Update request returned nothing.");
                series = (TvdbDetailedSeries)ser.Deserialize(stream);
            }

            if (useCache)
            {
                var streamWriter = new StreamWriter(cacheLocation, false);
                var serializer = new XmlSerializer(typeof(TvdbDetailedSeries));
                serializer.Serialize(streamWriter, series);
                streamWriter.Close();
            }
            return series;
        }
    }

    [XmlType(AnonymousType = true)]
    public class TvdbSeriesExtended : TvdbSeries
    {
        [XmlElement("Actors")]
        public string Actors { get; set; }
        [XmlElement(ElementName = "Airs_DayOfWeek")]
        public string AirsDayOfWeek { get; set; }
        [XmlElement(ElementName = "Airs_Time")]
        public string AirsTime { get; set; }
        [XmlElement("ContentRating")]
        public string ContentRating { get; set; }
        [XmlElement("Genre")]
        public string Genre { get; set; }
        [XmlElement(ElementName = "NetworkID")]
        public string NetworkId { get; set; }
        [XmlElement("Rating")]
        public decimal Rating { get; set; }
        [XmlElement("RatingCount")]
        public uint RatingCount { get; set; }
        [XmlElement("Runtime")]
        public byte Runtime { get; set; }
        [XmlElement("Status")]
        public string Status { get; set; }
        [XmlElement(ElementName = "added")]
        public string Added { get; set; }
        [XmlElement(ElementName = "addedBy")]
        public string AddedBy { get; set; }
        [XmlElement(ElementName = "fanart")]
        public string Fanart { get; set; }
        [XmlElement(ElementName = "lastupdated")]
        public uint LastUpdated { get; set; }
        [XmlElement(ElementName = "poster")]
        public string Poster { get; set; }
        [XmlElement(ElementName = "tms_wanted_old")]
        public byte TmsWantedOld { get; set; }
    }

    [XmlType(AnonymousType = true)]
    public class TvdbEpisode
    {
        [XmlElement(ElementName = "id")]
        public uint Id { get; set; }
        [XmlElement(ElementName = "Combined_episodenumber")]
        public decimal CombinedEpisodeNumber { get; set; }
        [XmlElement(ElementName = "Combined_season")]
        public byte CombinedSeason { get; set; }
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
        public byte EpisodeNumber { get; set; }
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
        public byte RatingCount { get; set; }
        [XmlElement(ElementName = "SeasonNumber")]
        public byte SeasonNumber { get; set; }
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
