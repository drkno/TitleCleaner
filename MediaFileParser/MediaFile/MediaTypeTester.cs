#region

using System;

#endregion

namespace MediaFileParser.MediaFile
{
    internal class MediaTypeTester
    {
        protected static Type[] MediaTypes =
        {
            typeof (TvFile.TvFile),
            typeof (MovieFile)
        };

        protected MediaTypeTester()
        {
        }

        public static MediaFile GetMediaFile(string file)
        {
            foreach (var mediaType in MediaTypes)
            {
                var mediaFile = (MediaFile) Activator.CreateInstance(mediaType, file);
                if (mediaFile.Test())
                {
                    return mediaFile;
                }
            }
            return null;
        }
    }
}