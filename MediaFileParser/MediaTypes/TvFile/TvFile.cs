#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using MediaFileParser.MediaTypes.TvFile.Tvdb;

#endregion

namespace MediaFileParser.MediaTypes.TvFile
{
    public class TvFile : MediaFile.MediaFile
    {
        protected static char[] TrimChars = { ' ', '-', '.' };
        protected string NameVar;
        protected string TitleVar;
        private uint _season;

        public TvFile(string file) : base(file)
        {
            Episode = new List<uint>();

            int blockStart = int.MaxValue, blockEnd = int.MinValue;
            for (var i = 0; i < SectorList.Count; i++)
            {
                // Get Season from a S00 block
                var matches = Regex.Matches(SectorList[i], @"(s|S)[0-9]{1,2}");
                if (matches.Count == 1)
                {
                    var season = uint.Parse(matches[0].Value.Substring(1));
                    if (Season != 0 && season != Season)
                    {
                        throw new Exception("Can't have an episode with multiple seasons.");
                    }
                    Season = season;
                    if (i > blockEnd) blockEnd = i;
                    if (i < blockStart) blockStart = i;
                }
                else if (matches.Count != 0)
                {
                    throw new Exception("Can't have an episode with multiple seasons.");
                }

                // Get Episode from an E00 block
                matches = Regex.Matches(SectorList[i], @"(e|E)[0-9]{1,2}(-[0-9]{1,2})?");
                if (matches.Count > 0)
                {
                    foreach (Match match in matches)
                    {
                        var split = match.Value.Substring(1).Split('-');
                        foreach (var s in split)
                        {
                            Episode.Add(uint.Parse(s));
                        }
                    }
                    if (i > blockEnd) blockEnd = i;
                    if (i < blockStart) blockStart = i;
                    continue;
                }

                // Get Season/Episodes from a 00x00(-00) block
                matches = Regex.Matches(SectorList[i], @"[0-9]{1,2}(x|X)[0-9]{1,2}(-[0-9]{1,2})?");
                if (matches.Count == 1)
                {
                    SectorList[i] = "S" + Regex.Replace(matches[0].Value, "x", "E", RegexOptions.IgnoreCase);
                    i--;
                    continue;
                }
                if (matches.Count != 0)
                {
                    throw new Exception("Can't have an episode with multiple seasons.");
                }

                // Get Season from explicit mention
                if (Regex.IsMatch(SectorList[i], "season", RegexOptions.IgnoreCase) &&
                    Regex.IsMatch(SectorList[i + 1], @"[0-9]{1,2}"))
                {
                    i += 1;
                    var season = uint.Parse(SectorList[i]);
                    if (Season != 0 && season != Season)
                    {
                        throw new Exception("Can't have an episode with multiple seasons.");
                    }
                    Season = season;
                    if (i > blockEnd) blockEnd = i;
                    if (i < blockStart) blockStart = i - 1;
                    continue;
                }

                // Get Episode from explicit mention
                if (Regex.IsMatch(SectorList[i], "episode", RegexOptions.IgnoreCase) &&
                    Regex.IsMatch(SectorList[i + 1], @"[0-9]{1,2}"))
                {
                    i += 1;
                    Episode.Clear(); // would be hard to do this with joint episodes...
                    Episode.Add(uint.Parse(SectorList[i]));
                    if (i > blockEnd) blockEnd = i;
                    if (i < blockStart) blockStart = i - 1;
                }
            }
            // Get Episode from a "00 Blah Blah.ext" type file
            var initMatch = Regex.Match(SectorList[0], @"[0-9]{1,2}(?<![0-9])");
            if (Episode.Count == 0 && initMatch.Success)
            {
                Episode.Add(uint.Parse(initMatch.Value));
                blockStart = 0;
                if (0 > blockEnd) blockEnd = 0;
            }

            // Episode/Season mixed into a short number
            var k = false;
            var j = -1;
            for (var i = 0; i < SectorList.Count + ((j == -1) ? 0 : -1); i++)
            {
                if (!Regex.IsMatch(SectorList[i], @"^[0-9]{1,3}$")) continue;
                if (k)
                {
                    k = false;
                    break;
                }
                k = true;
                j = i;
            }
            if (k && SectorList[j].Length <= 4 && Episode.Count == 0)
            {
                var len = SectorList[j].Length;
                if (SectorList[j].Length >= 3)
                {
                    Season = uint.Parse(SectorList[j].Substring(0, len - 2));
                }
                if ((len-2)>=0)
                {
                    Episode.Add(uint.Parse(SectorList[j].Substring(len - 2, 2)));
                }
                
                if (j < blockStart) blockStart = j;
                if (j > blockEnd) blockEnd = j;
            }
            // Get show name
            if (blockStart == int.MaxValue) blockStart = 0;
            if (blockEnd == int.MinValue) blockEnd = 0;
            var begin = 0;
            if (blockStart == 0)
            {
                blockStart = SectorList.Count;
                begin = blockEnd + 1;
            }
            var name = "";
            for (var i = begin; i < blockStart; i++)
            {
                name += SectorList[i] + ((i + 1 != blockStart) ? " " : "");
            }
            Name = name;

            // Get show title (or swap if wrong way around)
            if (begin == 0)
            {
                var title = "";
                for (var i = blockEnd + 1; i < SectorList.Count; i++)
                {
                    title += SectorList[i] + ((i + 1 != SectorList.Count) ? " " : "");
                }
                Title = title;
            }
            else
            {
                var split = Name.Split('-');
                if (split.Length == 2 && split[0][split[0].Length - 1] == split[1][0] && split[1][0] == ' ')
                {
                    Title = split[1];
                    Name = split[0];
                }
                else
                {
                    Title = Name;
                    Name = "Unknown";
                }
            }
        }

        public string Name
        {
            get { return NameVar; }
            protected set { NameVar = value.Trim(TrimChars); }
        }

        public uint Season
        {
            get
            {
                if (_season != 0) return _season;
                var match = Regex.Match(Folder, "((?<=((Season|Series).))[1-9][0-9]*|Specials?)", RegexOptions.IgnoreCase);
                if (match.Success && !Regex.IsMatch(Folder, "Specials", RegexOptions.IgnoreCase))
                {
                    uint.TryParse(match.Value, out _season);
                }
                return _season;
            }
            protected set { _season = value; }
        }

        public List<uint> Episode { get; protected set; }

        #region Tvdb Lookup Section
        public static bool TvdbLookup { get; set; }
        public static bool TvdbLookupConfirm { get; set; }
        public static string TvdbApiKey { protected get; set; }
        protected static Dictionary<string, uint> TvdbSearchSelectionCache { get; set; }

        protected static Tvdb.Tvdb TvdbApiManager;

        public delegate uint TvdbSearchSelectionRequiredEvent(TvdbSeries[] seriesSearch);

        public static event TvdbSearchSelectionRequiredEvent TvdbSearchSelectionRequired;

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
            var searchCacheName = Name.ToLower().Trim();
            var id = TvdbSearchSelectionCache.ContainsKey(searchCacheName) ? TvdbSearchSelectionCache[searchCacheName] : TvdbSearchSelectionRequired(seriesList);
            // Add search selection to cache
            if (!TvdbSearchSelectionCache.ContainsKey(searchCacheName)) TvdbSearchSelectionCache.Add(searchCacheName, id);
            // 0 is a sentinal "none of them" value
            if (id == 0) return null;
            // Get episode
            episode = TvdbApiManager.LookupEpisode(id, Season, Episode[0]);
            // Return episode
            return episode;
        }

        public string Title
        {
            get
            {
                // Avoid looking up unknown titles
                if (!string.IsNullOrWhiteSpace(TitleVar) || !TvdbLookup || Name == "Unknown" || Episode.Count == 0) return TitleVar;
                // Get episode
                var episode = GetTvdbEpisode();
                // Return name
                return episode == null ? TitleVar : episode.EpisodeName;
            }
            protected set { TitleVar = value.Trim(TrimChars); }
        }
        #endregion

        public override string Cleaned
        {
            get { return ToString("N - [Sxe]" + ((!string.IsNullOrWhiteSpace(Title)) ? " - T" : "")); }
        }

        public override bool Test()
        {
            return
                Regex.IsMatch(Cleaned,
                    @"[a-zA-Z0-9 ]*[ ]?[a-zA-Z0-9]+ - \[[0-9]{2}x[0-9]{2}(-[0-9]{2})?\]( - [a-zA-Z0-9&' -]*)?( \([0-9]+\))?");
        }

        public new string ToString(string format)
        {
            var result = "";
            foreach (var c in format)
            {
                switch (c)
                {
                    case 'T':
                        result += Title;
                        break;

                    case 'N':
                        result += Name;
                        break;

                    case 'S':
                        result += Season.ToString("00");
                        break;

                    case 'e':
                        {
                            for (var i = 0; i < Episode.Count; i++)
                            {
                                result += Episode[i].ToString("00");
                                result += (i + 1 != Episode.Count) ? "-" : "";
                            }
                            break;
                        }
                    default:
                        result += base.ToString(c.ToString(CultureInfo.InvariantCulture));
                        break;
                }
            }
            return result;
        }
    }
}