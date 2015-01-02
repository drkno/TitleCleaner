namespace MediaFileParser.MediaTypes.TvFile.Tvdb.Cache
{
    /// <summary>
    /// Type of cache to use when retreiving from the TVDB.
    /// </summary>
    public enum TvdbCacheType
    {
        /// <summary>
        /// No cache will be used.
        /// Note: if any other flag is specified this will be ignored.
        /// </summary>
        None = 0,
        /// <summary>
        /// Each lookup will be stored in memory and be avalible to any future
        /// lookups within this program instance that also specified this option.
        /// </summary>
        Memory = 1,
        /// <summary>
        /// Each lookup will be stored in a non volitile location so that it can be
        /// reused during later lookups.
        /// </summary>
        PersistentMemory = 2
    }
}