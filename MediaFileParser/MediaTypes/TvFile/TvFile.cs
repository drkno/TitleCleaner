#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

#endregion

namespace MediaFileParser.MediaTypes.TvFile
{
    /// <summary>
    /// Type to store TV files.
    /// </summary>
    public partial class TvFile : MediaFile.MediaFile
    {
        /// <summary>
        /// String to use for unknown.
        /// </summary>
        private const string UnknownString = "Unknown";

        /// <summary>
        /// String to use for special.
        /// </summary>
        private const string SpecialString = "Special";

        /// <summary>
        /// String to use for special, when shortened.
        /// </summary>
        private const string SpecialShortString = "SP";

        /// <summary>
        /// String to use for season.
        /// </summary>
        private const string SeasonString = "Season";

        /// <summary>
        /// String to use for series.
        /// </summary>
        private const string SeriesString = "Series";

        /// <summary>
        /// String to use for episode.
        /// </summary>
        private const string EpisodeString = "Episode";

        /// <summary>
        /// Characters to trim from file names.
        /// </summary>
        protected static readonly char[] TrimChars = { ' ', '-', '.' };

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
        /// Set when this file is gaurenteed to be a TV file.
        /// </summary>
        private readonly bool _testGaurentee;

        /// <summary>
        /// Gets the season number from the current directory (if possible).
        /// </summary>
        /// <returns>Season number or 0 on failure.</returns>
        private uint GetSeasonFromDir()
        {
            var match = Regex.Match(Folder, "((?<=((" + SeasonString + "|" + 
                SeriesString + ").))[1-9][0-9]*|" + SpecialString + "s?)", RegexOptions.IgnoreCase);
            if (!match.Success || Regex.IsMatch(Folder, SpecialString+"s?", RegexOptions.IgnoreCase)) return 0;
            uint season;
            uint.TryParse(match.Value, out season);
            return season;
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
                    SetTvdbFields();
                    return TitleVar;
                }
                catch (Exception)
                {
                    return TitleVar;
                }
            }
            protected set { TitleVar = value.Trim(TrimChars); }
        }

        /// <summary>
        /// Gets the series name.
        /// </summary>
        public string Name
        {
            get
            {
                if (NameVar != UnknownString) return NameVar;
                var temp = GetSeasonFromDir();
                if ((temp == 0 && (Season <= 0 || Episode.Contains(0)))) return NameVar;
                try   // in this case we are **fairly** certain that the containing dir is the program name
                {
                    if (temp == 0)
                    {
                        var loc = Location;
                        var spl = loc.Substring(loc.LastIndexOfAny(PathSeperators) + 1).Split(' ');
                        var builder = new StringBuilder(spl.Length * 2);
                        foreach (var s in spl)
                        {
                            builder.Append(ToUpperString(s));
                            builder.Append(' ');
                        }
                        builder.Remove(builder.Length - 1, 1);
                        NameVar = builder.ToString();
                    }
                    else
                    {
                        var loc = Location;
                        loc = loc.Substring(0, loc.LastIndexOfAny(PathSeperators));
                        loc = loc.Substring(loc.LastIndexOfAny(PathSeperators) + 1);
                        var spl = loc.Split(' ');
                        var builder = new StringBuilder(spl.Length * 2);
                        foreach (var s in spl)
                        {
                            builder.Append(ToUpperString(s));
                            builder.Append(' ');
                        }
                        builder.Remove(builder.Length - 1, 1);
                        NameVar = builder.ToString();
                    }
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
            protected set
            {
                if (_season != 0 && _season != value) throw new Exception("Can't have an episode with multiple seasons.");
                _season = value;
            }
        }

        /// <summary>
        /// Gets the episode numbers of this episode.
        /// </summary>
        public List<uint> Episode { get; protected set; }

        /// <summary>
        /// Year that this TV episode was made.
        /// </summary>
        public override int Year
        {
            get
            {
                if (base.Year > 0 || !TvdbLookup) return base.Year;
                SetTvdbFields();
                return base.Year;
            }
            protected set { base.Year = value; }
        }

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
            if (_testGaurentee)
            {
                return true;
            }

            if (Episode.Count == 0 || ((Season == 0 || Episode.Contains(0)) && Name == UnknownString && string.IsNullOrWhiteSpace(Title)))
            {
                return false;
            }

            if (Season == 0 && (Name == UnknownString || (Episode.Count == 1 && Episode[0] > 20 && string.IsNullOrWhiteSpace(Title))))
            {
                return false;
            }

            var earliest = SectorList.TakeWhile(sector => !Regex.Match(sector, "^([0-9]+)|(SP)$").Success).Count();
            if (SectorList.Count > earliest && earliest >= 0 && Name == UnknownString
                && !(SectorList[earliest+1] == "-" || SectorList[earliest+1] == ":" || SectorList[earliest+1] == "."))
            {
                return false;
            }
            
            if (Season == 0 && string.IsNullOrWhiteSpace(Title) && Name.Split(' ').Length < 2)
            {
                return false;
            }

            return true;
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
                    if (Episode.Count > 0 && Episode[0] == 0) return SpecialShortString;
                    var result = new StringBuilder(Episode.Count);
                    for (var i = 0; i < Episode.Count; i++)
                    {
                        result.Append(Episode[i].ToString("00"));
                        result.Append((i + 1 != Episode.Count) ? "-" : "");
                    }
                    return result.ToString();
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
        /// ToString implementation of the TV File.
        /// </summary>
        /// <returns>The default string representation of this object.</returns>
        public override string ToString()
        {
            return ToString(DefaultFormatString);
        }

        /// <summary>
        /// Storage of the output directory for this media type.
        /// </summary>
        protected new static string TypeOutDirectory = "TV Shows|[ts(N)]|" + SeasonString + " [ts(s)]";

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