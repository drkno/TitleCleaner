namespace MediaFileParser.MediaTypes.MovieFile
{
    /// <summary>
    /// Represents a movie media file.
    /// </summary>
    public class MovieFile : MediaFile.MediaFile
    {
        /// <summary>
        /// Default constructor for movie file.
        /// </summary>
        /// <param name="file">File to base this instance on.</param>
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

        /// <summary>
        /// Storage for a MovieFile specific ToString format.
        /// </summary>
        private static string _defaultFormatToString;

        /// <summary>
        /// Gets or sets the default ToString() output format for MovieFile and its inheritors.
        /// </summary>
        public new static string DefaultFormatString
        {
            get
            {
                return string.IsNullOrWhiteSpace(_defaultFormatToString)
                    ? DefaultFormatToString
                    : _defaultFormatToString;
            }
            set { _defaultFormatToString = value; }
        }

        /// <summary>
        /// ToString implementation of the Movie File.
        /// </summary>
        /// <returns>The default string representation of this object.</returns>
        public override string ToString()
        {
            return ToString(DefaultFormatString);
        }

        /// <summary>
        /// Storage of the output directory for this media type.
        /// </summary>
        protected new static string TypeOutDirectory = "Movies";

        /// <summary>
        /// Gets or sets the default directory name that this
        /// media file type should be stored in if it moves directory.
        /// </summary>
        public new static string TypeDirectory
        {
            get { return TypeOutDirectory; }
            set { TypeOutDirectory = value; }
        }

        /// <summary>
        /// Gets the directory name that this media file type
        /// should be stored in if it moves directory.
        /// See static member TypeDirectory for manipulation.
        /// </summary>
        public override string OutputDirectory
        {
            get { return TypeOutDirectory; }
        }
    }
}