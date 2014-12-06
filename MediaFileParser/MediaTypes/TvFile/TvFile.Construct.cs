using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MediaFileParser.MediaTypes.TvFile
{
    public partial class TvFile
    {
        /// <summary>
        /// Creates a new TV File
        /// </summary>
        /// <param name="file">File to base the TV File on</param>
        public TvFile(string file)
            : base(file)
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
                        if (matches[0].Index > 0)
                        {
                            var before = SectorList[i][matches[0].Index - 1];
                            if (Regex.Match(before.ToString(CultureInfo.InvariantCulture), @"[0-9]|[-]|\s").Success)
                            {
                                throw new Exception("Can't have an episode with multiple seasons.");
                            }
                        }
                        else
                        {
                            throw new Exception("Can't have an episode with multiple seasons.");
                        }
                    }
                    else
                    {
                        Season = season;
                        if (i > blockEnd) blockEnd = i;
                        if (i < blockStart) blockStart = i;
                    }
                }
                else if (matches.Count != 0)
                {
                    var i1 = i;
                    if (matches.Cast<Match>().Any(match => match.Index == 0 || (SectorList[i1].Length > match.Length &&
                        Regex.Match(SectorList[i1][match.Length].ToString(CultureInfo.InvariantCulture), "[0-9^s^S]").Success)))
                    {
                        throw new Exception("Can't have an episode with multiple seasons.");
                    }
                }

                // Get Episode from an E00 block
                matches = Regex.Matches(SectorList[i], @"(e|E)[0-9]{1,2}(-[0-9]{1,2})?");
                if (matches.Count > 0)
                {
                    foreach (Match match in matches)
                    {
                        if (match.Index > 0)
                        {
                            var before = SectorList[i][match.Index - 1];
                            if (!Regex.Match(before.ToString(CultureInfo.InvariantCulture), @"[0-9]|[-]|\s").Success)
                            {
                                continue;
                            }
                        }

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
                if (Regex.IsMatch(SectorList[i], SeasonString, RegexOptions.IgnoreCase) &&
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
                if (Regex.IsMatch(SectorList[i], EpisodeString, RegexOptions.IgnoreCase) &&
                    (Regex.IsMatch(SectorList[i + 1], @"[0-9]{1,2}") || FromNumberWord(ref SectorList, i + 1)))
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
                if ((len - 2) >= 0)
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
            var name = new StringBuilder(blockStart - begin);
            for (var i = begin; i < blockStart; i++)
            {
                name.Append(SectorList[i]);
                name.Append(" ");
            }
            Name = name.ToString();

            // Get show title (or swap if wrong way around)
            if (begin == 0)
            {
                var builder = new StringBuilder(SectorList.Count - blockEnd - 1);
                for (var i = blockEnd + 1; i < SectorList.Count; i++)
                {
                    builder.Append(SectorList[i] + ((i + 1 != SectorList.Count) ? " " : ""));
                }
                Title = builder.ToString();
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
                    Name = UnknownString;
                }
            }
        }
    }
}
