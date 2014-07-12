using System;

namespace MediaFileParser.MediaFile
{
    public abstract partial class MediaFile
    {
        private static readonly string[] JunkStrings =
        {
            "xvid", "hdtv", "uncut", "[vtv]", "dvdscr", "dvdrip", "rerip", "(",
            "repack", "xor", "proper", "notv", "uncut", "xvid", "saints",
            "caph", "aph", "cam", "camrip", "ts", "telesync", "pdvd", "wp",
            "workprint", "tc", "telecine", "ppv", "ppvrip", "screener",
            "dvdscreener", "bdscr", "ddc", "r5", "r5.line", "ac3", "hq",
            "dvd-rip", "dvdr", "dvd-full", "full-rip", "iso rip", "lossless",
            "rip", "dvd-5", "ws", "720p", "ntsc", "dvd", "dts", "1080p", "dvd-9",
            "dsr", "dsrip", "dthrip", "dvbrip", "pdtv", "tvrip", "hdtvrip",
            "vodrip", "vodr", "bdrip", "brrip", "blu-ray", "bluray", "bdr", "bd5",
            "bd9", "bd25", "bd50", "web-rip", "webrip", "web rip", "webdl",
            "web-dl", "web dl", "h.264", "x264", "divx", "r0", "r1", "r2",
            "r3", "r4", "r6", "r7", "r8", "r9", "french", "truefrench", "unrated",
            "limited", "stv", "rapax-249", "[dvdrip]"
        };

        private void RemoveJunk()
        {
            var year = -1;
            for (var i = 1; i < SectorList.Count; i++)
            {
                int temp;
                if (int.TryParse(SectorList[i], out temp) && temp > 1950 && temp <= DateTime.Now.Year && i > year)
                {
                    year = i;
                }

                var sec = SectorList[i].ToLower().Trim();
                foreach (var junkString in JunkStrings)
                {
                    if (SectorList[i].Length <= 3)
                    {
                        if (sec != junkString) continue;
                        SectorRangeRemove(i, year);
                        return;
                    }
                    if (!sec.StartsWith(junkString)) continue;
                    SectorRangeRemove(i, year);
                    return;
                }
            }
            SectorRangeRemove(SectorList.Count, year);
        }

        private void SectorRangeRemove(int i, int year)
        {
            if (year != -1)
            {
                Year = int.Parse(SectorList[year]);
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