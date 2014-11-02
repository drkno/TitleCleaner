#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

#endregion

namespace MediaFileParser.MediaTypes.MediaFile
{
    /// <summary>
    /// Superclass of all media files
    /// </summary>
    public abstract partial class MediaFile
    {
        /// <summary>
        /// Delimeter chars of separator strings.
        /// </summary>
        protected static readonly char[] DelimChars = {',', '.', '_', ' '};

        /// <summary>
        /// OS Path Separators
        /// </summary>
        protected static readonly char[] PathSeperators = {Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar};

        /// <summary>
        /// List of sectors of the media file name.
        /// </summary>
        protected List<string> SectorList = new List<string>();

        /// <summary>
        /// Storage of the default ToString() output format.
        /// </summary>
        protected static string DefaultFormatToString = "C.E";

        /// <summary>
        /// Gets or sets the default ToString() output format.
        /// </summary>
        public static string DefaultFormatString
        {
            get { return DefaultFormatToString; }
            set { DefaultFormatToString = value; }
        }

        /// <summary>
        /// Storage of the output directory for this media type.
        /// </summary>
        protected static string TypeOutDirectory = "Media";

        /// <summary>
        /// Gets or sets the default directory name that this
        /// media file type should be stored in if it moves directory.
        /// </summary>
        public static string TypeDirectory
        {
            get { return TypeOutDirectory; }
            set { TypeOutDirectory = value; }
        }

        /// <summary>
        /// Gets the directory name that this media file type
        /// should be stored in if it moves directory.
        /// See static member TypeDirectory for manipulation.
        /// </summary>
        public virtual string OutputDirectory
        {
            get { return TypeDirectory; }
        }

        /// <summary>
        /// Constructor for a media file.
        /// Performs common tasks for name cleanup.
        /// </summary>
        /// <param name="file">Name to clean.</param>
        protected MediaFile(string file)
        {
            Origional = file;
            Year = -1;

            // Location
            int pathSep;
            if ((pathSep = file.LastIndexOfAny(PathSeperators)) != -1)
            {
                Location = file.Substring(0, pathSep);
                Origional = Origional.Substring(Location.Length + 1);
            }

            // Extension
            var pathExt = file.LastIndexOf('.');
            Extension = file.Substring(pathExt + 1);
            Origional = Origional.Substring(0, Origional.Length - Extension.Length - 1);

            // Raw
            file = Origional;
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
                    if (!SectorList[i].Substring(ind+1).Contains("-"))     // Cope with strings such as 100-year-old
                    {
                        SectorList.Insert(i + 1, SectorList[i].Substring(ind + 1));
                        SectorList[i] = SectorList[i].Substring(0, ind);
                    }
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

        /// <summary>
        /// Origional file name for this file (excluding file extension).
        /// </summary>
        public string Origional { get; protected set; }

        /// <summary>
        /// File extension for this media file.
        /// </summary>
        public string Extension { get; protected set; }

        /// <summary>
        /// Location on the HD that this media file is stored.
        /// </summary>
        public string Location { get; protected set; }

        /// <summary>
        /// Year that the media in this media file was made.
        /// </summary>
        public int Year { get; protected set; }

        /// <summary>
        /// Folder this file is stored in.
        /// </summary>
        public string Folder
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Location) || Location.LastIndexOfAny(PathSeperators) == -1)
                {
                    return "";      // not stored in a directory (probably root)
                }
                return Location.Substring(Location.LastIndexOfAny(PathSeperators) + 1);
            }
        }

        /// <summary>
        /// Gets the default cleaned name of this file.
        /// </summary>
        public abstract string Cleaned { get; }

        /// <summary>
        /// Tests if the media file is of this type.
        /// </summary>
        /// <returns>If this file is of this media type.</returns>
        public abstract bool Test();

        /// <summary>
        /// Returns the string representation of this object.
        /// </summary>
        /// <returns>The string representation of this object.</returns>
        public override string ToString()
        {
            return ToString(DefaultFormatString);
        }

        /// <summary>
        /// Returns the string representation of this object.
        /// </summary>
        /// <param name="str">
        /// Format string to base this representation on.
        /// L:  Location
        /// O:  Origional Filename
        /// C:  Cleaned Filename
        /// E:  File Extension
        /// </param>
        /// <returns>The string representation of this object.</returns>
        public virtual string ToString(string str)
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