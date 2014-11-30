#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using MediaFileParser.MediaTypes.TvFile.Tvdb;

#endregion

namespace MediaFileParser.MediaTypes.TvFile
{
    /// <summary>
    /// Type to store TV files.
    /// </summary>
    public partial class TvFile : MediaFile.MediaFile
    {
        /// <summary>
        /// Characters to trim from file names.
        /// </summary>
        protected static char[] TrimChars = { ' ', '-', '.' };
        /// <summary>
        /// Temporary name of the episode variable.
        /// </summary>
        protected string NameVar;
        /// <summary>
        /// Temporary title of the episode variable.
        /// </summary>
        protected string TitleVar;
        /// <summary>
        /// Storage for the season number.
        /// </summary>
        private uint _season;

        /// <summary>
        /// Gets the season number from the current directory (if possible).
        /// </summary>
        /// <returns>Season number or 0 on failure.</returns>
        private uint GetSeasonFromDir()
        {
            var match = Regex.Match(Folder, "((?<=((Season|Series).))[1-9][0-9]*|Specials?)", RegexOptions.IgnoreCase);
            if (!match.Success || Regex.IsMatch(Folder, "Specials", RegexOptions.IgnoreCase)) return 0;
            uint season;
            uint.TryParse(match.Value, out season);
            return season;
        }

        /// <summary>
        /// Gets the series name.
        /// </summary>
        public string Name
        {
            get
            {
                if (NameVar != "Unknown") return NameVar;
                var temp = GetSeasonFromDir();
                if (temp == 0) return NameVar;
                try   // in this case we are **fairly** certain that the containing dir is the program name
                {
                    var loc = Location;
                    loc = loc.Substring(0, loc.LastIndexOfAny(PathSeperators));
                    loc = loc.Substring(loc.LastIndexOfAny(PathSeperators) + 1);
                    NameVar = loc;
                }
                catch   // probably in drive root
                {
                    Debug.WriteLine("Getting name from directory failed.");
                }
                return NameVar;
            }
            protected set { NameVar = value.Trim(TrimChars); }
        }

        /// <summary>
        /// Gets the season number.
        /// </summary>
        public uint Season
        {
            get
            {
                if (_season != 0) return _season;
                _season = GetSeasonFromDir();
                return _season;
            }
            protected set { _season = value; }
        }

        /// <summary>
        /// Gets the episode numbers of this episode.
        /// </summary>
        public List<uint> Episode { get; protected set; }

        #region Tvdb Lookup Section
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
        /// Gets the TVDB representation of this episode.
        /// </summary>
        /// <returns></returns>
        public TvdbEpisode GetTvdbEpisode()
        {
            // Avoid looking up unknown titles
            if (!string.IsNullOrWhiteSpace(TitleVar) || Name == "Unknown" || Episode.Count == 0) return null;
            // Init API if not done already
            if (TvdbApiManager == null) TvdbApiManager = new Tvdb.Tvdb(TvdbApiKey);
            // Search for series
            var seriesList = TvdbApiManager.Search(Name);
            // No selection required, just assume (unless confirmation is always required
            TvdbEpisode episode;
            if (seriesList.Length == 1 && !TvdbLookupConfirm)
            {
                // Get episode
                var seriesId = seriesList[0].Id;
                episode = TvdbApiManager.LookupEpisode(seriesId, Season, Episode[0]);
                // Return episode
                return episode;
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
            episode = TvdbApiManager.LookupEpisode(id, Season, Episode[0]);
            // Return episode
            return episode;
        }

        /// <summary>
        /// Gets the title of the episode.
        /// </summary>
        public string Title
        {
            get
            {
                // Avoid looking up unknown titles
                if (!string.IsNullOrWhiteSpace(TitleVar) || !TvdbLookup || Name == "Unknown" || Episode.Count == 0) return TitleVar;
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
                    base.Year = DateTime.Parse(episode.FirstAired).Year;
                }
                catch (Exception)
                {
                    Debug.WriteLine("Cannot get Year from TVDB.");
                }
                return yr;
            }
            protected set { base.Year = value; }
        }
        #endregion

        /// <summary>
        /// Gets a cleaner file name.
        /// </summary>
        public override string Cleaned
        {
            get { return ToString("N - [Sxe]" + ((!string.IsNullOrWhiteSpace(Title)) ? " - T" : "")); }
        }

        /// <summary>
        /// Tests if a media file is a valid tv file.
        /// </summary>
        /// <returns>If a file is a tv file.</returns>
        public override bool Test()
        {
            if (Episode.Count == 0 || ((Season == 0 || Episode.Contains(0)) && Name == "Unknown" && Title == ""))
            {
                return false;
            }

            var earliest = SectorList.TakeWhile(sector => !Regex.Match(sector, "^[0-9]+$").Success).Count();
            if (SectorList.Count > earliest && earliest >= 0 && Name == "Unknown"
                && !(SectorList[earliest+1] == "-" || SectorList[earliest+1] == ":" || SectorList[earliest+1] == "."))
            {
                return false;
            }

            if (Season == 0 && Title == "" && Name.Split(' ').Length < 2)
            {
                return false;
            }

            return
                Regex.IsMatch(Cleaned,
                    @"[a-zA-Z0-9 ]*[ ]?[a-zA-Z0-9]+ - \[[0-9]{2}x[0-9]{2}(-[0-9]{2})?\]( - [a-zA-Z0-9&' -]*)?( \([0-9]+\))?");
        }

        /// <summary>
        /// Returns the string representation of this object based on character in format string.
        /// </summary>
        /// <param name="str">
        /// Format string to base this representation on.
        /// L:  Location
        /// O:  Origional Filename
        /// C:  Cleaned Filename
        /// E:  File Extension
        /// T:  Title of the Episode
        /// N:  Name of the Episode
        /// S:  Season Number of the Episode (Padded to 2sf)
        /// s:  Season Number of the Episode
        /// e:  Episode Number of the Episode
        /// and all those in MediaFile.
        /// </param>
        /// <param name="ind">The index of the character to base the string representation on.</param>
        /// <returns>The string representation of this object based on character at the specified index.</returns>
        public override string ToString(ref string str, ref int ind)
        {
            switch (str[ind])
            {
                case 'T':
                {
                    return Title;
                }
                case 'N':
                {
                    return Name;
                }
                case 'S':
                {
                    return Season.ToString("00");
                }
                case 's':
                {
                    return Season.ToString(CultureInfo.InvariantCulture);
                }
                case 'e':
                {
                    var result = "";
                    for (var i = 0; i < Episode.Count; i++)
                    {
                        result += Episode[i].ToString("00");
                        result += (i + 1 != Episode.Count) ? "-" : "";
                    }
                    return result;
                }
                default:
                {
                    return base.ToString(ref str, ref ind);
                }
            }
        }

        /// <summary>
        /// Storage for a MovieFile specific ToString format.
        /// </summary>
        private static string _defaultFormatToString;

        /// <summary>
        /// Gets or sets the default ToString() output format for MovieFile and its inheritors.
        /// </summary>
        public new static string DefaultFormatString
        {
            get
            {
                return string.IsNullOrWhiteSpace(_defaultFormatToString)
                    ? DefaultFormatToString
                    : _defaultFormatToString;
            }
            set { _defaultFormatToString = value; }
        }

        /// <summary>
        /// Storage of the output directory for this media type.
        /// </summary>
        protected new static string TypeOutDirectory = "TV Shows|[ts(N)]|Season [ts(s)]";

        /// <summary>
        /// Gets or sets the default directory name that this
        /// media file type should be stored in if it moves directory.
        /// </summary>
        public new static string TypeDirectory
        {
            get { return TypeOutDirectory; }
            set { TypeOutDirectory = value; }
        }

        /// <summary>
        /// Gets the directory name that this media file type
        /// should be stored in if it moves directory.
        /// See static member TypeDirectory for manipulation.
        /// </summary>
        public override string OutputDirectory
        {
            get { return TypeOutDirectory; }
        }
    }
}