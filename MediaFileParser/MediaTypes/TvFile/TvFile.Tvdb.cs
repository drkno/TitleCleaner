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
        protected TvdbEpisode TvdbEpisode;

        /// <summary>
        /// Gets the TVDB representation of this episode.
        /// </summary>
        /// <returns>The TVDB equivilent of this episode.</returns>
        public TvdbEpisode GetTvdbEpisode()
        {
            // Avoid looking up unknown titles
            if (!string.IsNullOrWhiteSpace(TitleVar) || Name == UnknownString || Episode.Count == 0) return null;
            // Return previous lookup if already done
            if (TvdbEpisode != null) return TvdbEpisode;
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
                TvdbEpisode = TvdbApiManager.LookupEpisode(seriesId, Season, Episode[0]);
                // Return episode
                return TvdbEpisode;
            }
            if (seriesList.Length <= 1 && (!TvdbLookupConfirm || seriesList.Length <= 0)) return null;
            // Selection required...
            if (TvdbSearchSelectionCache == null) TvdbSearchSelectionCache = new Dictionary<string, uint>();
            var searchCacheName = Name.ToLower();
            Debug.Assert(TvdbSearchSelectionRequired != null, "TvdbSearchSelectionRequired != null");
            var id = TvdbSearchSelectionCache.ContainsKey(searchCacheName) ? TvdbSearchSelectionCache[searchCacheName] : TvdbSearchSelectionRequired.Invoke(seriesList, Name);
            // Add search selection to cache
            if (!TvdbSearchSelectionCache.ContainsKey(searchCacheName)) TvdbSearchSelectionCache.Add(searchCacheName, id);
            // 0 is a sentinal "none of them" value
            if (id == 0) return null;
            // Get episode
            TvdbEpisode = TvdbApiManager.LookupEpisode(id, Season, Episode[0]);
            // Return episode
            return TvdbEpisode;
        }

        /// <summary>
        /// Sets the fields of this TVFile based on a returned TVDB episode.
        /// </summary>
        private void SetTvdbFields()
        {
            var episode = GetTvdbEpisode();
            Title = episode.EpisodeName;
            try { Year = episode.FirstAiredDate.Year; } catch { Debug.WriteLine("Failed to set Year from TVDB"); }
            if (Season != episode.SeasonNumber) Season = episode.SeasonNumber;
            if (!Episode.Contains(episode.EpisodeNumber)) Episode.Add(episode.EpisodeNumber);
        }
    }
}
