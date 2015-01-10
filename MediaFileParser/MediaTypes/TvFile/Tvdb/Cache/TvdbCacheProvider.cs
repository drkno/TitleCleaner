using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace MediaFileParser.MediaTypes.TvFile.Tvdb.Cache
{
    /// <summary>
    /// Handles the caching of API requests.
    /// </summary>
    public class TvdbCacheProvider : Dictionary<string, object>
    {
        /// <summary>
        /// File extension to use for cache files.
        /// </summary>
        private const string FileExtension = ".xml";

        /// <summary>
        /// Type of cache to provide.
        /// </summary>
        public TvdbCacheType CacheType { get; protected set; }

        /// <summary>
        /// Location of the persistent cache. Defaults to the current directory if not specified.
        /// </summary>
        public string PersistentCacheLocation
        {
            get { return _cacheLocation; }
            protected set
            {
                Debug.WriteLine("-> TvdbCacheProvider::Location::set value=\"" + value + "\" Called");
                try
                {
                    if (File.Exists(value))
                    {
                        throw new Exception("Folder location not file must be provided.");
                    }
                    if (!Directory.Exists(value))
                    {
                        Directory.CreateDirectory(value);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine("!> TvdbCacheProvider::Location threw an exception: " + e);
                    throw new ArgumentException("The provided cache location was invalid.", "value", e);
                }
                _cacheLocation = value;
            }
        }

        /// <summary>
        /// API time of entries in this cache.
        /// </summary>
        public uint LastApiTime { get; private set; }

        /// <summary>
        /// Sets the API time of the entries of this cache.
        /// </summary>
        /// <param name="time">New API time, must be in the future relative to the current time.</param>
        public void SetApiTime(TvdbApiTime time)
        {
            Debug.WriteLine("-> TvdbCacheProvider::SetApiTime time=\"" + time + "\" Called");
            if (time.Time < LastApiTime)
            {
                throw new Exception("API time changes must be futuristic.");
            }

            var ts = EpochToDateTime(time.Time) - EpochToDateTime(LastApiTime);
            LastApiTime = time.Time;
            if (ts.Days >= 30)
            {
                foreach (var key in Keys)
                {
                    if (CacheType == TvdbCacheType.PersistentMemory)
                    {
                        RemoveFromPersistentCache(key);
                    }
                    Remove(key);
                }
            }
            else
            {
                var keys = Keys.ToArray();
                foreach (var key in keys)
                {
                    var spl = key.Split('~');
                    uint id;
                    if (spl.Length != 2 || !uint.TryParse(spl[0], out id)) continue;
                    var ind = -1;
                    switch (spl[1])
                    {
                        case "series":
                        {
                            ind = time.Series.BinarySearch(id);
                            break;
                        }
                        case "episode":
                        {
                            ind = time.Episodes.BinarySearch(id);
                            break;
                        }
                    }
                    if (ind < 0) continue;
                    if (CacheType == TvdbCacheType.PersistentMemory)
                    {
                        RemoveFromPersistentCache(key);
                    }
                    Remove(key);
                }
            }

            var timeLoc = Path.Combine(PersistentCacheLocation, "time.ini");
            try
            {
                var writer = new StreamWriter(timeLoc);
                writer.WriteLine(time.Time);
                writer.Close();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Saving time to persistent cache failed.");
                Debug.WriteLine("!> TvdbCacheProvider::SetApiTime threw an exception: " + e);
            }
        }

        /// <summary>
        /// Gets the DateTime for a UNIX epoch.
        /// </summary>
        /// <param name="epoch">Seconds from the epoch.</param>
        /// <returns>A DateTime representing the time since the epoch.</returns>
        public static DateTime EpochToDateTime(double epoch)
        {
            Debug.WriteLine("-> TvdbCacheProvider::EpochToDateTime epoch=\"" + epoch + "\" Called");
            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(epoch);
        }

        /// <summary>
        /// Removes a key from the persistent cache.
        /// </summary>
        /// <param name="key">Key to remove.</param>
        private void RemoveFromPersistentCache(string key)
        {
            Debug.WriteLine("-> TvdbCacheProvider::RemoveFromPersistentCache key=\"" + key + "\" Called");
            var fileLocation = key + "~" + this[key].GetType() + FileExtension;
            fileLocation = Path.GetInvalidFileNameChars().Aggregate(fileLocation, (current, c) => current.Replace(c, ' '));
            fileLocation = Path.Combine(PersistentCacheLocation, fileLocation);
            File.Delete(fileLocation);
        }

        /// <summary>
        /// Backing field of the Location property.
        /// </summary>
        private string _cacheLocation;

        /// <summary>
        /// Creates a new cache provider for persistent storage of API requests.
        /// </summary>
        /// <param name="cacheType">Type of cache to provide.</param>
        /// <param name="persistentCacheLocation">Location to store the persistent cache.
        /// If null will default to current directory.</param>
        public TvdbCacheProvider(TvdbCacheType cacheType, string persistentCacheLocation)
        {
            Debug.WriteLine("-> TvdbCacheProvider::_cstr cacheType=\"" + cacheType + "\" persistentCacheLocation=\"" + persistentCacheLocation + "\" Called");
            LastApiTime = 0;
            CacheType = cacheType;
            PersistentCacheLocation = persistentCacheLocation ?? Environment.CurrentDirectory;
            if (cacheType == TvdbCacheType.PersistentMemory)
            {
                LoadFromPersistentCache();
            }
        }

        /// <summary>
        /// Determines if a key exists in the cache dictionary.
        /// </summary>
        /// <param name="key">Key to search for.</param>
        /// <returns>True if key is in cache dictionary, false otherwise.</returns>
        public new bool ContainsKey(string key)
        {
            Debug.WriteLine("-> TvdbCacheProvider::ContainsKey key=\"" + key + "\" Called");
            if (CacheType == TvdbCacheType.None) return false;
            key = Path.GetInvalidFileNameChars().Aggregate(key, (current, c) => current.Replace(c, ' '));
            return base.ContainsKey(key);
        }

        /// <summary>
        /// Gets a request response from the dictionary.
        /// </summary>
        /// <param name="key">Key to get response for.</param>
        /// <returns>Associated object to the request key.</returns>
        public new object this[string key]
        {
            get { return InternalGet(key); }
            set { InternalAdd(key, value, false); }
        }

        /// <summary>
        /// Adds an item to the cache dictionary.
        /// </summary>
        /// <param name="key">Key for the item to add.</param>
        /// <param name="value">Response object for key.</param>
        /// <param name="ignorePersistentCache">True to avoid adding to the persistent cache.</param>
        public void Add(string key, object value, bool ignorePersistentCache)
        {
            InternalAdd(key, value, ignorePersistentCache);
        }

        /// <summary>
        /// Adds an item to the cache dictionary.
        /// </summary>
        /// <param name="key">Key for the item to add.</param>
        /// <param name="value">Response object for key.</param>
        /// <param name="ignorePersistentCache">True to avoid adding to the persistent cache.</param>
        private void InternalAdd(string key, object value, bool ignorePersistentCache)
        {
            Debug.WriteLine("-> TvdbCacheProvider::InternalAdd key=\"" + key + "\" value=\"" + value + "\" ignorePersistentCache=\"" + ignorePersistentCache + "\" Called");
            if (CacheType == TvdbCacheType.None || string.IsNullOrWhiteSpace(key))
            {
                return;
            }

            key = Path.GetInvalidFileNameChars().Aggregate(key, (current, c) => current.Replace(c, ' '));

            base[key] = value;
            if (CacheType == TvdbCacheType.PersistentMemory && !ignorePersistentCache)
            {
                AddToPersistentCache(key, value);
            }
        }

        /// <summary>
        /// Gets a request response from the dictionary.
        /// </summary>
        /// <param name="key">Key to get response for.</param>
        /// <returns>Associated object to the request key.</returns>
        private object InternalGet(string key)
        {
            Debug.WriteLine("-> TvdbCacheProvider::InternalGet key=\"" + key + "\" Called");
            if (CacheType == TvdbCacheType.None)
            {
                return null;
            }
            key = Path.GetInvalidFileNameChars().Aggregate(key, (current, c) => current.Replace(c, ' '));
            return base[key];
        }

        /// <summary>
        /// Loads requests from the persistent cache into the dictionary.
        /// </summary>
        private void LoadFromPersistentCache()
        {
            Debug.WriteLine("-> TvdbCacheProvider::LoadFromPersistentCache Called");
            var files = Directory.EnumerateFileSystemEntries(PersistentCacheLocation, "*" + FileExtension);
            foreach (var file in files)
            {
                var key = Path.GetFileNameWithoutExtension(file);
                if (key == null) continue;
                var ind = key.LastIndexOf('~');
                if (ind < 0) continue;
                var typeString = key.Substring(ind + 1);
                if (string.IsNullOrWhiteSpace(typeString)) continue;
                var type = Type.GetType(typeString);
                if (type == null) continue;
                key = key.Substring(0, ind);
                if (string.IsNullOrWhiteSpace(key)) continue;
                var reader = new FileStream(file, FileMode.Open);
                var ser = new XmlSerializer(type);
                var deserialized = ser.Deserialize(reader);
                reader.Close();
                base[key] = deserialized;
            }

            var timeLoc = Path.Combine(PersistentCacheLocation, "time.ini");
            if (!File.Exists(timeLoc)) return;
            try
            {
                var reader = new StreamReader(timeLoc);
                var line = reader.ReadLine();
                if (!string.IsNullOrWhiteSpace(line))
                {
                    LastApiTime = uint.Parse(line);
                }
                reader.Close();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Reading time from persistent cache failed.");
                Debug.WriteLine("!> TvdbCacheProvider::LoadFromPersistentCache threw an exception: " + e);
            }
        }

        /// <summary>
        /// Adds a request to the persistent cache.
        /// </summary>
        /// <param name="key">Key to use for storage and retreival.</param>
        /// <param name="value">Request object to store.</param>
        private void AddToPersistentCache(string key, object value)
        {
            Debug.WriteLine("-> TvdbCacheProvider::AddToPersistentCache key=\"" + key + "\" value=\"" + value + "\" Called");
            var fileLocation = key + "~" + value.GetType() + FileExtension;
            fileLocation = Path.GetInvalidFileNameChars().Aggregate(fileLocation, (current, c) => current.Replace(c, ' '));
            fileLocation = Path.Combine(PersistentCacheLocation, fileLocation);
            var cacheWriter = new FileStream(fileLocation, FileMode.Create);
            var x = new XmlSerializer(value.GetType());
            x.Serialize(cacheWriter, value);
            cacheWriter.Close();
        }
    }
}
