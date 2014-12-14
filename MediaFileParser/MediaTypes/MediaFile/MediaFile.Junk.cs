using System;
using System.Text.RegularExpressions;
using MediaFileParser.MediaTypes.MediaFile.Junk;

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
        /// Junk strings that are contained in filenames and their associated attributes.
        /// </summary>
        private static readonly JunkContainer JunkStrings = new JunkContainer
        {
            "(", "xor", "aph", "ac3", "rerip", "repack", "proper", "notv", "uncut", "xvid", "saints", "caph", "rip", "xvid", "uncut",
            "[vtv]", "ppvrip", {"real", new []{"ity"}, null}, "full-rip", "ntsc", "web-rip", "webrip", "webdl", "web-dl", "h.264", "x264", "divx", "french",
            "truefrench", "unrated", "limited", "rapax-249", "readnfo", "korsub",
            {"cam", new[]{"e","p","i"}, MediaFileQuality.LowQuality}, {"ts", MediaFileQuality.LowQuality}, {"wp", MediaFileQuality.LowQuality},
            {"tc", MediaFileQuality.LowQuality}, {"ppv", MediaFileQuality.MediumQuality}, {"ddc", MediaFileQuality.MediumQuality},
            {"r5", MediaFileQuality.MediumToHighQuality}, {"hq", MediaFileQuality.HighQuality}, {"ws", MediaFileQuality.LowToMediumQuality},
            {"dvd", MediaFileQuality.MediumToHighQuality}, {"dts", MediaFileQuality.MediumToHighQuality},
            {"dsr", MediaFileQuality.MediumToHighQuality}, {"bdr", MediaFileQuality.HighQuality}, {"bd5", MediaFileQuality.HighQuality},
            {"bd9", MediaFileQuality.HighQuality}, {"720", MediaFileQuality.MediumToHighQuality}, {"r0", MediaFileQuality.MediumQuality},
            {"r1", MediaFileQuality.MediumQuality}, {"r2", MediaFileQuality.MediumQuality}, {"r3", MediaFileQuality.MediumQuality},
            {"r4", MediaFileQuality.MediumQuality}, {"r6", MediaFileQuality.MediumQuality}, {"r7", MediaFileQuality.MediumQuality},
            {"r8", MediaFileQuality.MediumQuality}, {"r9", MediaFileQuality.MediumQuality}, {"stv", MediaFileQuality.MediumQuality},
            {"hdtv", MediaFileQuality.HighQuality}, {"dvdscr", MediaFileQuality.MediumQuality},
            {"dvdrip", MediaFileQuality.MediumToHighQuality}, {"telesync", MediaFileQuality.LowQuality},
            {"pdvd", MediaFileQuality.LowQuality}, {"workprint", MediaFileQuality.LowQuality},
            {"telecine", MediaFileQuality.LowQuality}, {"screener", MediaFileQuality.MediumQuality}, {"[hq]", MediaFileQuality.HighQuality},
            {"480p", MediaFileQuality.MediumQuality},{"dvdscreener", MediaFileQuality.MediumQuality}, {"bdscr", MediaFileQuality.HighQuality},
            {"dvd-rip", MediaFileQuality.MediumToHighQuality}, {"dvdr", MediaFileQuality.MediumToHighQuality},
            {"dvd-full", MediaFileQuality.MediumToHighQuality}, {"lossless", MediaFileQuality.HighQuality},
            {"dvd-5", MediaFileQuality.MediumToHighQuality}, {"720p", MediaFileQuality.MediumToHighQuality},
            {"1080p", MediaFileQuality.HighQuality}, {"dvd-9", MediaFileQuality.MediumToHighQuality},
            {"dsrip", MediaFileQuality.MediumToHighQuality}, {"dthrip", MediaFileQuality.MediumToHighQuality},
            {"dvbrip", MediaFileQuality.MediumToHighQuality}, {"pdtv", MediaFileQuality.MediumToHighQuality},
            {"tvrip", MediaFileQuality.MediumToHighQuality}, {"hdtvrip", MediaFileQuality.HighQuality},
            {"vodrip", MediaFileQuality.LowToMediumQuality}, {"vodr", MediaFileQuality.LowToMediumQuality},
            {"bdrip", MediaFileQuality.HighQuality}, {"brrip", MediaFileQuality.HighQuality}, {"blu-ray", MediaFileQuality.HighQuality},
            {"bluray", MediaFileQuality.HighQuality}, {"bd25", MediaFileQuality.HighQuality}, {"bd50", MediaFileQuality.HighQuality},
            {"1080", MediaFileQuality.HighQuality},  {"[dvdrip]", MediaFileQuality.MediumToHighQuality},
            {"hdrip", MediaFileQuality.HighQuality}
        };

        /// <summary>
        /// Detect junk strings int the sectorlist.
        /// </summary>
        private void DetectJunkAtIndex(ref int i, ref int year, ref int removeStart)
        {
            if (i == 0) return;

            var sec = SectorList[i].ToLower();

            if (Regex.IsMatch(SectorList[i], "^(([(][12][0-9]{3}[)])|([12][0-9]{3}))$"))
            {
                var tempS = SectorList[i];
                if (tempS[0] == '(')
                {
                    tempS = tempS.Substring(1, 4);
                }

                int temp;
                if (int.TryParse(tempS, out temp) && temp > YearStart && temp <= DateTime.Now.Year && i > year)
                {
                    Year = temp;
                    year = i;   // we've found the year (we think)
                    SectorList[i] = tempS;
                }
            }

            var j = JunkStrings.Find(sec);
            if (j == null) return;
            Quality = j.Quality;
            if (removeStart > i) removeStart = i;
        }

        /// <summary>
        /// Removes a range of sectors starting at the index provided and going
        /// on to the end of the sector list.
        /// </summary>
        /// <param name="i">The index to start removing from.</param>
        /// <param name="year">The year found or -1 if the year is still unknown.</param>
        private void SectorRangeRemove(int i, int year)
        {
            if (year != -1 && year < i)
            {
                if (year != i - 1)
                {
                    SectorList.RemoveAt(year);
                }
                i--;
            }

            if (SectorList.Count - i == 0) return;
            SectorList.RemoveRange(i, SectorList.Count - i);
        }
    }
}