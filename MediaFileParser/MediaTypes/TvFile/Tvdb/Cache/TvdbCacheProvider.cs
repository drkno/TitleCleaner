using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace MediaFileParser.MediaTypes.TvFile.Tvdb.Cache
{
    public class TvdbCacheProvider : Dictionary<string, object>
    {
        private const string FileExtension = ".tvdbCache";
        private bool _cacheLocationLoaded;

        public TvdbCacheType CacheType
        {
            get { return _cacheType; }
            set { _cacheType = value; }
        }

        private string _cacheLocation = Environment.CurrentDirectory;
        private TvdbCacheType _cacheType = TvdbCacheType.None;

        public string Location
        {
            get { return _cacheLocation; }
            set
            {
                if (_cacheLocationLoaded)
                {
                    throw new Exception("Persistent cache cannot be moved after it is in use.");
                }

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
                    throw new ArgumentException("The provided cache location was invalid.", "value", e);
                }
                _cacheLocation = value;
            }
        }

        public new bool ContainsKey(string key)
        {
            if (CacheType == TvdbCacheType.None)
            {
                return false;
            }

            key = Path.GetInvalidFileNameChars().Aggregate(key, (current, c) => current.Replace(c, ' '));

            if (!_cacheLocationLoaded)
            {
                LoadFromPersistentCache();
            }

            return base.ContainsKey(key);
        }

        public new object this[string key]
        {
            get { return InternalGet(key); }
            set { InternalAdd(key, value, false); }
        }

        public void Add(string key, object value, bool ignorePersistentCache)
        {
            InternalAdd(key, value, ignorePersistentCache);
        }

        private void InternalAdd(string key, object value, bool ignorePersistentCache)
        {
            if (CacheType == TvdbCacheType.None)
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

        private object InternalGet(string key)
        {
            if (CacheType == TvdbCacheType.None)
            {
                return null;
            }

            if (!_cacheLocationLoaded)
            {
                LoadFromPersistentCache();
            }

            key = Path.GetInvalidFileNameChars().Aggregate(key, (current, c) => current.Replace(c, ' '));
            return base[key];
        }

        private void LoadFromPersistentCache()
        {
            var files = Directory.EnumerateFileSystemEntries(Location, "*" + FileExtension);
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
                var reader = new StreamReader(file);
                var ser = new XmlSerializer(type);
                var deserialized = ser.Deserialize(reader);
                reader.Close();
                base[key] = deserialized;
            }

            _cacheLocationLoaded = true;
        }

        private void AddToPersistentCache(string key, object value)
        {
            var fileLocation = key + "~" + value.GetType() + FileExtension;
            fileLocation = Path.GetInvalidFileNameChars().Aggregate(fileLocation, (current, c) => current.Replace(c, ' '));
            fileLocation = Path.Combine(Location, fileLocation);
            var cacheWriter = new StreamWriter(fileLocation);
            var x = new XmlSerializer(value.GetType());
            x.Serialize(cacheWriter, value);
            cacheWriter.Close();
        }
    }
}
