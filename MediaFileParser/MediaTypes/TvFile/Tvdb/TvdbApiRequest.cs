using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Serialization;

namespace tvdbApi
{
    /// <summary>
    /// Class to manage requests to the API and deserialise requests.
    /// </summary>
    public class TvdbApiRequest
    {
        // Dictionary to store requests if caching is enabled. Will reduce API usage for repetitive calls.
        private static readonly Dictionary<string, object> RuntimeApiCache = new Dictionary<string, object>();

        public static bool UseCache { get; set; }

        private const string MirrorPath = "http://thetvdb.com/api/";
        private static string _userAgent = "TvdbRequestManager/2.1 (C#, part of TitleCleaner, like Gecko)";
        public static string UserAgent
        {
            get { return _userAgent; }
            set { _userAgent = value; }
        }

        private static CookieContainer _cookieContainer = new CookieContainer();

        protected static Stream PerformApiRequest(string url)
        {
            Debug.WriteLine("-> TvdbApiRequest::PerformApiRequest url=\"" + url + "\" Called");
            var webRequest = (HttpWebRequest) WebRequest.Create(MirrorPath + url);
            webRequest.UserAgent = UserAgent;
            webRequest.AutomaticDecompression = DecompressionMethods.Deflate |
                                                DecompressionMethods.GZip |
                                                DecompressionMethods.None;
            webRequest.AllowAutoRedirect = true;
            webRequest.CookieContainer = _cookieContainer;

            var stream = new MemoryStream();
            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
            {
                var responseStream = webResponse.GetResponseStream();
                if (responseStream == null)
                {
                    throw new Exception("No data was returned from lookup.");
                }
                var reader = new StreamReader(responseStream);
                var text = reader.ReadToEnd();
                Debug.WriteLine("---- Response ----\n" + text + "\n----   Halt   ----");
                reader.Close();
                var bytes = Encoding.UTF8.GetBytes(text);
                stream.Write(bytes, 0, bytes.Length);
            }
            _cookieContainer = webRequest.CookieContainer;
            stream.Position = 0;
            return stream;
        }

        public static T PerformApiRequestAndDeserialize<T>(string url)
        {
            try
            {
                // Retreive previous api request from the cache
                if (UseCache && RuntimeApiCache.ContainsKey(url))
                {
                    return (T) RuntimeApiCache[url];
                }

                // Perform api request and deserialize
                var stream = PerformApiRequest(url);
                var ser = new XmlSerializer(typeof(T));
                var deserialized = (T)ser.Deserialize(stream);

                // Add to cache if cache is enabled
                if (UseCache)
                {
                    RuntimeApiCache.Add(url, deserialized);
                }

                return deserialized;
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}
