using System;
using System.Collections.Generic;
using System.Linq;

namespace MediaFileParser.MediaTypes.MediaFile
{
    /// <summary>
    /// Superclass of all media files
    /// </summary>
    public abstract partial class MediaFile
    {
        /// <summary>
        /// First year to start accepting as a year value.
        /// </summary>
        private const short YearStart = 1900;

        /// <summary>
        /// Common long (len > 3) junk strings that are contained in filenames.
        /// </summary>
        private static readonly string[] LongJunkStrings =
        {
            "xvid", "hdtv", "uncut", "[vtv]", "dvdscr", "dvdrip", "rerip", "repack", "proper",
            "notv", "uncut", "xvid", "saints", "caph", "camrip", "telesync", "pdvd", "workprint",
            "telecine", "ppvrip", "screener", "[hq]", "dvdscreener", "bdscr", "r5.line", "real",
            "dvd-rip", "dvdr", "dvd-full", "full-rip", "lossless", "dvd-5", "720p", "ntsc",
            "1080p", "dvd-9", "dsrip", "dthrip", "dvbrip", "pdtv", "tvrip", "hdtvrip", "vodrip",
            "vodr", "bdrip", "brrip", "blu-ray", "bluray", "bd25", "bd50", "web-rip", "webrip",
            "webdl", "web-dl", "h.264", "x264", "divx", "1080", "french", "truefrench", "unrated",
            "limited", "rapax-249", "[dvdrip]", "korsub", "hdrip", "readnfo"
        };

        /// <summary>
        /// Common short (len less than or equal to 3) junk strings that are contained in filenames.
        /// </summary>
        private static readonly HashSet<string> ShortJunkStrings = new HashSet<string>
        {
            "(", "xor","aph", "cam", "ts", "wp", "tc",  "ppv", "ddc", "r5",  "ac3", "hq", "rip", 
            "ws", "dvd", "dts", "dsr", "bdr", "bd5", "bd9","720", "r0", "r1", "r2", "r3", "r4",
            "r6", "r7", "r8", "r9", "stv"
        };

        /// <summary>
        /// Removes junk strings from the sectorlist.
        /// </summary>
        private void RemoveJunk()
        {
            var year = -1;
            for (var i = 1; i < SectorList.Count; i++)
            {
                var sec = SectorList[i].ToLower().Trim();

                int temp;
                if (int.TryParse(sec, out temp) && temp > YearStart && temp <= DateTime.Now.Year && i > year)
                {
                    year = i;   // we've found the year (we think)
                }

                if ((sec.Length > 3 || !ShortJunkStrings.Contains(sec)) &&
                    (sec.Length <= 3 || (!LongJunkStrings.Any(sec.StartsWith) && !ShortJunkStrings.Any(sec.StartsWith))))
                    continue;
                SectorRangeRemove(i, year);
                return;
            }
            SectorRangeRemove(SectorList.Count, year);
        }

        /// <summary>
        /// Removes a range of sectors starting at the index provided and going
        /// on to the end of the sector list.
        /// </summary>
        /// <param name="i">The index to start removing from.</param>
        /// <param name="year">The year found or -1 if the year is still unknown.</param>
        private void SectorRangeRemove(int i, int year)
        {
            if (year != -1)
            {
                Year = int.Parse(SectorList[year]); // extract year before removing sectors
                if (year < i)
                {
                    SectorList.Remove(SectorList[year]);
                    i--;
                }
            }

            if (SectorList.Count - i == 0) return;
            SectorList.RemoveRange(i, SectorList.Count - i);
        }
    }
}