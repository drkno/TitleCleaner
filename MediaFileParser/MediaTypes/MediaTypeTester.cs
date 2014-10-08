#region

using System;

#endregion

namespace MediaFileParser.MediaTypes
{
    /// <summary>
    /// Tests media types and creates an appropriate one for the file provided.
    /// </summary>
    public class MediaTypeTester
    {
        /// <summary>
        /// The set media type.
        /// </summary>
        protected static Type MediaType = null;

        /// <summary>
        /// Media types to search through.
        /// </summary>
        protected static Type[] MediaTypes =
        {
            typeof (TvFile.TvFile),
            typeof (MovieFile.MovieFile)
        };

        /// <summary>
        /// This is a static class so an instance should not be instantiated
        /// unless a derived type works differently.
        /// </summary>
        protected MediaTypeTester() {}

        /// <summary>
        /// Gets a MediaFile of the appropriate type.
        /// </summary>
        /// <param name="file">The file to create a MediaFile for.</param>
        /// <returns>A new media file or null if no appropriate MediaFile can be found.</returns>
        public static MediaFile.MediaFile GetMediaFile(string file)
        {
            if (MediaType != null)
            {   // use the set type
                return (MediaFile.MediaFile) Activator.CreateInstance(MediaType, file);
            }

            foreach (var mediaType in MediaTypes)
            {   // search through each type
                var mediaFile = (MediaFile.MediaFile) Activator.CreateInstance(mediaType, file);
                if (mediaFile.Test())
                {
                    return mediaFile;   // type found, create object
                }
            }
            return null;    // no type can be found
        }

        /// <summary>
        /// Sets a specific MediaFile type to always instantiate.
        /// </summary>
        /// <param name="type">MediaFile type to instantiate.</param>
        public static void SetMediaType(Type type)
        {
            MediaType = type;
        }

        /// <summary>
        /// Sets the MediaFile types to search through.
        /// </summary>
        /// <param name="mediaTypes">MediaFile types to search.</param>
        public static void SetMediaTypes(Type[] mediaTypes)
        {
            MediaTypes = mediaTypes;
        }
    }
}