using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using MediaFileParser.MediaTypes.TvFile.Tvdb;
using MediaFileParser.MediaTypes.TvFile.Tvdb.Cache;

namespace MediaFileParser.MediaTypes.TvFile
{
    public partial class TvFile
    {
        /// <summary>
        /// Set wheather to lookup info from the TVDB automatically.
        /// </summary>
        public static bool TvdbLookup { get; set; }
        /// <summary>
        /// Set wheather to confirm automatic selections with the user when using the TVDB.
        /// </summary>
        public static bool TvdbLookupConfirm { get; set; }
        /// <summary>
        /// API Key to use with the TVDB.
        /// </summary>
        public static string TvdbApiKey { protected get; set; }
        /// <summary>
        /// Runtime cache to use with the TVDB.
        /// </summary>
        protected static Dictionary<string, uint> TvdbSearchSelectionCache { get; set; }

        /// <summary>
        /// TVDB API Manager
        /// </summary>
        protected static Tvdb.Tvdb TvdbApiManager;

        /// <summary>
        /// TVDB requires a search selection.
        /// </summary>
        /// <param name="seriesSearch">The resultant series returned from the search.</param>
        /// <param name="seriesName">The series this search is for.</param>
        /// <returns>The ID of the series that is selected by the event.</returns>
        public delegate uint TvdbSearchSelectionRequiredEvent(TvdbSeries[] seriesSearch, string seriesName);

        /// <summary>
        /// Fired on a TVDB request requiring selection.
        /// </summary>
        public static event TvdbSearchSelectionRequiredEvent TvdbSearchSelectionRequired;

        /// <summary>
        /// Stores reference to a previous lookup of this episode.
        /// </summary>
        protected TvdbEpisode _tvdbEpisode;

        /// <summary>
        /// Gets the TVDB representation of this episode.
        /// </summary>
        /// <returns></returns>
        public TvdbEpisode GetTvdbEpisode()
        {
            // Avoid looking up unknown titles
            if (!string.IsNullOrWhiteSpace(TitleVar) || Name == UnknownString || Episode.Count == 0) return null;
            // Return previous lookup if already done
            if (_tvdbEpisode != null) return _tvdbEpisode;
            // Init API if not done already
            if (TvdbApiManager == null)
            {
                TvdbApiManager = new Tvdb.Tvdb(TvdbApiKey, TvdbCacheType.PersistentMemory,
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TitleCleaner"));
            }
            // Search for series
            var seriesList = TvdbApiManager.Search(Name);
            // No selection required, just assume (unless confirmation is always required)
            if (seriesList.Length == 1 && !TvdbLookupConfirm)
            {
                // Get episode
                var seriesId = seriesList[0].TvdbId;
                _tvdbEpisode = TvdbApiManager.LookupEpisode(seriesId, Season, Episode[0]);
                // Return episode
                return _tvdbEpisode;
            }
            if (seriesList.Length <= 1 && (!TvdbLookupConfirm || seriesList.Length <= 0)) return null;
            // Selection required...
            if (TvdbSearchSelectionCache == null) TvdbSearchSelectionCache = new Dictionary<string, uint>();
            var searchCacheName = Name.ToLower();
            var id = TvdbSearchSelectionCache.ContainsKey(searchCacheName) ? TvdbSearchSelectionCache[searchCacheName] : TvdbSearchSelectionRequired(seriesList, Name);
            // Add search selection to cache
            if (!TvdbSearchSelectionCache.ContainsKey(searchCacheName)) TvdbSearchSelectionCache.Add(searchCacheName, id);
            // 0 is a sentinal "none of them" value
            if (id == 0) return null;
            // Get episode
            _tvdbEpisode = TvdbApiManager.LookupEpisode(id, Season, Episode[0]);
            // Return episode
            return _tvdbEpisode;
        }

        /// <summary>
        /// Gets the title of the episode.
        /// </summary>
        public string Title
        {
            get
            {
                // Avoid looking up unknown titles
                if (!string.IsNullOrWhiteSpace(TitleVar) || !TvdbLookup || Name == UnknownString || Episode.Count == 0) return TitleVar;
                try
                {   // Get episode
                    var episode = GetTvdbEpisode();
                    return episode.EpisodeName;
                }
                catch (Exception)
                {
                    return TitleVar;
                }
            }
            protected set { TitleVar = value.Trim(TrimChars); }
        }



        /// <summary>
        /// Year that this TV episode was made.
        /// </summary>
        public override int Year
        {
            get
            {
                var yr = base.Year;
                if (yr > 0 || !TvdbLookup) return yr;
                try
                {
                    var episode = GetTvdbEpisode();
                    base.Year = episode.FirstAiredDate.Year;
                }
                catch (Exception)
                {
                    Debug.WriteLine("Cannot get Year from TVDB.");
                }
                return yr;
            }
            protected set { base.Year = value; }
        }
    }
}
