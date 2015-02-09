using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Serialization;
using MediaFileParser.MediaTypes.TvFile.Tvdb.Cache;

namespace MediaFileParser.MediaTypes.TvFile.Tvdb
{
    /// <summary>
    /// Class to manage requests to the API and deserialise requests.
    /// </summary>
    public class TvdbApiRequest
    {
        /// <summary>
        /// Creates a new request object for making API calls.
        /// </summary>
        /// <param name="apiKey">API key to use for requests.</param>
        /// <param name="tvdbCacheProvider">The cache provider to use for persistent API requests.</param>
        public TvdbApiRequest(string apiKey, TvdbCacheProvider tvdbCacheProvider)
        {
            Debug.WriteLine("-> TvdbApiRequest::_cstr tvdbCacheProvider=\"" + tvdbCacheProvider + "\" Called");
            _cookieContainer = new CookieContainer();
            ApiKey = apiKey;
            CacheProvider = tvdbCacheProvider;
            PerformCacheUpdate();
        }

        /// <summary>
        /// API key to use for requests.
        /// </summary>
        public string ApiKey { get; private set; }

        #region Cache
        /// <summary>
        /// Dictionary to store requests if caching is enabled. Will reduce API usage for repetitive calls.
        /// </summary>
        public TvdbCacheProvider CacheProvider { get; private set; }

        /// <summary>
        /// Location of the persistent storage cache (if required).
        /// Defaults to current directory if not set.
        /// </summary>
        public string PersistentCacheLocation
        {
            get { return CacheProvider.PersistentCacheLocation; }
        }

        /// <summary>
        /// Minimum time between cache timeouts in seconds. Defaults to 86400 seconds (24hrs).
        /// </summary>
        public uint CacheTimeout
        {
            get { return _cacheTimeout; }
            set { _cacheTimeout = value; }
        }

        /// <summary>
        /// Backing field for CacheTimeout.
        /// </summary>
        private uint _cacheTimeout = 86400;

        /// <summary>
        /// Performs an API update request if appropriate. This is controlled by the API timeout key.
        /// </summary>
        /// <returns>If updates were made.</returns>
        public bool PerformCacheUpdate()
        {
            Debug.WriteLine("-> TvdbApiRequest::PerformCacheUpdate Called");
            var sec = (DateTime.Now - TvdbCacheProvider.EpochToDateTime(CacheProvider.LastApiTime)).TotalSeconds;
            if (sec < _cacheTimeout) return false;
            var time = TvdbApiTime.TvdbServerTime(this, CacheProvider.LastApiTime);
            CacheProvider.SetApiTime(time);
            return true;
        }

        #endregion

        /// <summary>
        /// URL path to the primary mirror of the TVDB.
        /// </summary>
        private const string MirrorPath = "http://thetvdb.com/api/";

        /// <summary>
        /// Backing field of the user agent property.
        /// </summary>
        private string _userAgent = "TvdbRequestManager/2.2 (C#, part of TitleCleaner, like Gecko)";

        /// <summary>
        /// User Agent to use with each API request.
        /// </summary>
        public string UserAgent
        {
            get { return _userAgent; }
            set { _userAgent = value; }
        }

        /// <summary>
        /// Cookie container so that cookies from requests to the API are persistent.
        /// </summary>
        private CookieContainer _cookieContainer;

        /// <summary>
        /// Performs a request to the API and gets the result as a stream.
        /// </summary>
        /// <param name="url">API sub-URL to use for the request.</param>
        /// <returns>Result of the API request as a stream.</returns>
        private Stream PerformApiRequest(string url)
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
                    Debug.WriteLine("!> TvdbApiRequest::PerformApiRequest is throwing an exception: No data was returned from lookup.");
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

        /// <summary>
        /// Performs a request to the TVDB API and returns the resultant object.
        /// </summary>
        /// <typeparam name="T">Type of the object to return.</typeparam>
        /// <param name="url">API sub-URL to use for the request.</param>
        /// <param name="cacheKey">Key to use for cache storage of this request.</param>
        /// <param name="ignoreCurrentCache">Ignores existing cache entries for this request.</param>
        /// <param name="ignorePersistentCache">Does not add this request to the persistent cache.</param>
        /// <returns>Object returned for the API request.</returns>
        public T PerformApiRequestAndDeserialize<T>(string url, string cacheKey, bool ignoreCurrentCache = false, bool ignorePersistentCache = false)
        {
            Debug.WriteLine("-> TvdbApiRequest::PerformApiRequestAndDeserialize url=\"" + url + "\" cacheKey=\"" + cacheKey +
                "\" ignoreCurrentCache=\"" + ignoreCurrentCache + "\" ignorePersistentCache=\"" + ignorePersistentCache + "\" Called");
            try
            {
                // Retreive previous api request from the cache
                if (CacheProvider.ContainsKey(cacheKey)) return (T)CacheProvider[cacheKey];

                // Perform api request and deserialize
                var stream = PerformApiRequest(url);
                var ser = new XmlSerializer(typeof(T));
                var deserialized = (T) ser.Deserialize(stream);

                // Add to cache if cache is enabled
                if (CacheProvider.CacheType != TvdbCacheType.None)
                {
                    CacheProvider.Add(cacheKey, deserialized, ignorePersistentCache);
                }

                return deserialized;
            }
            catch (Exception e)
            {
                Debug.WriteLine("!> TvdbApiRequest::PerformApiRequestAndDeserialize threw an exception: " + e);
                return default(T); // request probably failed so return null for safe abort
            }
        }
    }
}
