namespace MediaFileParser.MediaTypes.MovieFile
{
    /// <summary>
    /// Represents a movie media file.
    /// </summary>
    public class MovieFile : MediaFile.MediaFile
    {
        public MovieFile(string file) : base(file)
        {
            // No further parsing required.
        }

        /// <summary>
        /// Generates the cleaned file title.
        /// </summary>
        public override string Cleaned
        {
            get
            {
                var result = "";
                for (var i = 0; i < SectorList.Count; i++)
                {
                    if (SectorList[i] == "-") continue;
                    result += SectorList[i] + " ";
                }
                result = result.Trim();
                return result;
            }
        }

        /// <summary>
        /// Tests if a media file is a movie.
        /// </summary>
        /// <returns>True. All media files will be movie files in some way.</returns>
        public override bool Test()
        {
            return true;
        }
    }
}