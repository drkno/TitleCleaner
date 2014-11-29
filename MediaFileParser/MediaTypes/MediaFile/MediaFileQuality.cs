namespace MediaFileParser.MediaTypes.MediaFile
{
    /// <summary>
    /// Represents the quality of a media file.
    /// </summary>
    public enum MediaFileQuality
    {
        /// <summary>
        /// Low quality files, such as a CAM or TS.
        /// </summary>
        LowQuality,
        /// <summary>
        /// Low to medium quality files, such as [types needed].
        /// </summary>
        LowToMediumQuality,
        /// <summary>
        /// Medium quality files, such as 480p or DVD-SCR.
        /// </summary>
        MediumQuality,
        /// <summary>
        /// Medium to high quality files, such as DVD or 720p.
        /// </summary>
        MediumToHighQuality,
        /// <summary>
        /// High quality files, such as BlueRay, 1080p or HD.
        /// </summary>
        HighQuality
    }
}
