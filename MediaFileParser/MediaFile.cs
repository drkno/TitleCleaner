#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

#endregion

namespace MediaFileParser
{
    public abstract partial class MediaFile
    {
        protected static readonly char[] DelimChars = {',', '.', '_', ' '};
        protected static readonly char[] PathSeperators = {Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar};
        protected List<string> SectorList = new List<string>();

        protected MediaFile(string file)
        {
            Origional = file;
            Year = -1;

            // Location
            int pathSep;
            if ((pathSep = file.IndexOfAny(PathSeperators)) != -1)
            {
                Location = file.Substring(0, pathSep);
            }

            // Extension
            if (pathSep != -1)
            {
                pathSep += 2;
            }
            else
            {
                pathSep += 1;
            }
            var pathExt = file.LastIndexOf('.');
            Extension = file.Substring(pathExt + 1);

            // Raw
            file = file.Substring(pathSep, pathExt - pathSep);
            SectorList.AddRange(file.Split(DelimChars, StringSplitOptions.RemoveEmptyEntries));

            // Autocapitalise first letter in each word
            // And merge alone letters. eg "A M" -> "AM"
            // And add splits along num-letter boundries
            for (var i = 0; i < SectorList.Count; i++)
            {
                var match = Regex.Match(SectorList[i], @"([0-9])+-([a-zA-Z])+");
                if (match.Success)
                {
                    var ind = match.Value.IndexOf('-') + match.Index;
                    SectorList.Insert(i + 1, SectorList[i].Substring(ind + 1));
                    SectorList[i] = SectorList[i].Substring(0, ind);
                }
                if (String.IsNullOrEmpty(SectorList[i])) continue;

                var a = SectorList[i].ToCharArray();
                a[0] = Char.ToUpper(a[0], CultureInfo.InvariantCulture);
                SectorList[i] = new string(a);
                if (i > 0 && SectorList[i].Length == 1 && SectorList[i - 1].Length == 1 &&
                    Regex.IsMatch(SectorList[i], @"[A-Z]") && Regex.IsMatch(SectorList[i - 1], @"[A-Z]"))
                {
                    i--;
                    SectorList[i] += SectorList[i + 1];
                    SectorList.RemoveAt(i + 1);
                }
            }

            // Remove junk names
            RemoveJunk();
        }

        public string Origional { get; protected set; }
        public string Extension { get; protected set; }
        public string Location { get; protected set; }
        public int Year { get; protected set; }

        public string Folder
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Location) || Location.LastIndexOfAny(PathSeperators) == -1)
                {
                    return "";
                }
                return Location.Substring(Location.LastIndexOfAny(PathSeperators) + 1);
            }
        }

        public abstract string Cleaned { get; }

        public abstract bool Test();

        public override string ToString()
        {
            return ToString("");
        }

        public string ToString(string str)
        {
            var result = "";
            foreach (var t in str)
            {
                switch (t)
                {
                    case 'L':
                        result += Location;
                        break;
                    case 'O':
                        result += Origional;
                        break;
                    case 'C':
                        result += Cleaned;
                        break;
                    case 'E':
                        result += Extension;
                        break;
                    default:
                        result += t;
                        break;
                }
            }
            return result;
        }
    }
}