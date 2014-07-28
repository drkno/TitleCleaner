#region

using System;

#endregion

namespace MediaFileParser.MediaTypes
{
    internal class MediaTypeTester
    {
        protected static Type[] MediaTypes =
        {
            typeof (TvFile.TvFile),
            typeof (MovieFile.MovieFile)
        };

        protected MediaTypeTester() {}

        public static MediaFile.MediaFile GetMediaFile(string file)
        {
            foreach (var mediaType in MediaTypes)
            {
                var mediaFile = (MediaFile.MediaFile) Activator.CreateInstance(mediaType, file);
                if (mediaFile.Test())
                {
                    return mediaFile;
                }
            }
            return null;
        }
    }
}