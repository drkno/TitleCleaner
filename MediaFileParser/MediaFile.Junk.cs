namespace MediaFileParser
{
    public abstract partial class MediaFile
    {
        private static readonly string[] JunkStrings =
        {
            "xvid", "hdtv", "uncut", "[vtv]", "dvdscr", "dvdrip", "rerip", "(",
            "repack", "xor", "proper", "notv", "uncut", "dvdrip", "xvid", "saints",
            "caph", "aph", "cam", "camrip", "ts", "telesync", "pdvd", "wp",
            "workprint", "tc", "telecine", "ppv", "ppvrip", "screener", "dvdscr",
            "dvdscreener", "bdscr", "ddc", "r5", "r5.line", "ac3", "hq", "dvdrip",
            "dvd-rip", "dvdr", "dvd-full", "full-rip", "iso rip", "lossless",
            "rip", "dvd-5", "ws", "720p", "ntsc", "dvd", "dts", "1080p", "dvd-9",
            "dsr", "dsrip", "dthrip", "dvbrip", "hdtv", "pdtv", "tvrip", "hdtvrip",
            "vodrip", "vodr", "bdrip", "brrip", "blu-ray", "bluray", "bdr", "bd5",
            "bd9", "bd25", "bd50", "web-rip", "webrip", "web rip", "webdl",
            "web-dl", "web dl", "h.264", "x264", "xvid", "divx", "r0", "r1", "r2",
            "r3", "r4", "r6", "r7", "r8", "r9"
        };

        private void RemoveJunk()
        {
            for (var i = 0; i < SectorList.Count; i++)
            {
                var sec = SectorList[i].ToLower().Trim();
                foreach (var junkString in JunkStrings)
                {
                    if (SectorList[i].Length <= 3)
                    {
                        if (sec != junkString) continue;
                        SectorList.RemoveRange(i, SectorList.Count - i);
                        return;
                    }
                    if (!sec.StartsWith(junkString)) continue;
                    SectorList.RemoveRange(i, SectorList.Count - i);
                    return;
                }
            }
        }
    }
}