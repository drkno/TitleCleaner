using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using MediaFileParser.MediaTypes;
using MediaFileParser.MediaTypes.MediaFile;

namespace MediaFileParser.ModeManagers
{
    /// <summary>
    /// Manages media files and their locations on the HD.
    /// </summary>
    public class FileManager
    {
        /// <summary>
        /// Asks for confirmation of an automatic move/rename.
        /// </summary>
        /// <param name="file">MediaFile that is being moved.</param>
        /// <param name="destination">Destination it is being moved to.</param>
        /// <returns>Wheather the user wants this move to occur.</returns>
        public delegate bool ConfirmAutomaticMoveDelegate(MediaFile file, string destination);
        /// <summary>
        /// Asks for confirmation of a file move.
        /// </summary>
        public event ConfirmAutomaticMoveDelegate ConfirmAutomaticMove;
        /// <summary>
        /// A move event occured for a media file.
        /// </summary>
        /// <param name="file">MediaFile being moved/renamed.</param>
        /// <param name="destination">Destination it is being moved to.</param>
        public delegate void MoveDelegate(MediaFile file, string destination);
        /// <summary>
        /// Called when a file is being moved.
        /// </summary>
        public event MoveDelegate OnFileMove;
        /// <summary>
        /// Called when a file move failed.
        /// </summary>
        public event MoveDelegate OnFileMoveFailed;
        /// <summary>
        /// Known media file extensions.
        /// </summary>
        private static readonly string[] FileExts =
        {
            "mov", "mkv", "flv", "avi",
            "mp4", "mpg", "vob", "m4v",
            "mpeg", "ogg", "swf", "wmv",
            "wtv", "h264"
        };
        /// <summary>
        /// Ask for confirmation.
        /// </summary>
        private readonly bool _confirm;

        /// <summary>
        /// Instantiates a new FileManager
        /// </summary>
        /// <param name="confirm">Ask for confirmation on move</param>
        public FileManager(bool confirm)
        {
            _confirm = confirm;
        }

        /// <summary>
        /// Notify using the events on a file move.
        /// </summary>
        /// <param name="file">The MediaFile affected.</param>
        /// <param name="destination">The destination that the file was/is being moved/renamed to.</param>
        /// <param name="success">Wheather the operation was successful.</param>
        private void NotifyOnMove(MediaFile file, string destination, bool success)
        {
            if (success && OnFileMove != null)
            {
                OnFileMove(file, destination);
            }
            else if (!success && OnFileMoveFailed != null)
            {
                OnFileMoveFailed(file, destination);
            }
        }

        /// <summary>
        /// Asks for confirmation of a file move/rename.
        /// </summary>
        /// <param name="file">MediaFile that is being moved/renamed.</param>
        /// <param name="destination">Destination the file is being moved/renamed to.</param>
        /// <returns>Wheather of not to move the file.</returns>
        private bool ConfirmMove(MediaFile file, string destination)
        {
            if (ConfirmAutomaticMove == null)
            {
                throw new NoNullAllowedException("For confirm to be enabled, a callback must be specified.");
            }
            return ConfirmAutomaticMove(file, destination);
        }

        /// <summary>
        /// Moves/renames all files in a list.
        /// </summary>
        /// <param name="files">Files to move/rename.</param>
        /// <param name="destination">Destination to move files to.</param>
        public void MoveFiles(IEnumerable<MediaFile> files, string destination = "")
        {
            foreach (var mediaFile in files)
            {
                MoveFile(mediaFile, destination);
            }
        }

        /// <summary>
        /// Moves/renames a MediaFile.
        /// </summary>
        /// <param name="file">MediaFile to move/rename.</param>
        /// <param name="destination">Destination to move/rename the file to.</param>
        public void MoveFile(MediaFile file, string destination = "")
        {
            if (string.IsNullOrWhiteSpace(destination))
            {
                destination = file.Location;
            }
            else
            {
                var subdir = GetMediaTypeDirectory(ref file);
                destination = NormalisePath(destination) + subdir;
            }

            var clea = PathSanitiseName(file.ToString());
            var move = true;
            if (_confirm || !file.Test())
            {
                var orig = file.ToString("O.E");    // prevent repetitive ToString()
                if (orig != clea || !String.Equals(orig, clea, StringComparison.CurrentCultureIgnoreCase) ||
                    NormalisePath(destination) != NormalisePath(file.Location))
                {
                    move = ConfirmMove(file, destination);
                }
            }
            if (!move)
            {
                NotifyOnMove(file, destination, false);
                return;
            }

            try
            {
                var path = Path.Combine(destination, clea);
                Directory.CreateDirectory(destination);
                File.Move(Path.Combine(file.Location, file.ToString("O.E")), path);
                NotifyOnMove(file, destination, true);
            }
            catch (Exception e)
            {
                Debug.WriteLine("File cannot be moved because of error: " + e);
                NotifyOnMove(file, destination, false);
            }
        }

        /// <summary>
        /// Converts the path to one that can be compared if possible.
        /// Based on http://stackoverflow.com/questions/2281531/how-can-i-compare-directory-paths-in-c
        /// </summary>
        /// <param name="path">Directory path</param>
        /// <returns>Normalised Path or origional path if not possible.</returns>
        private static string NormalisePath(string path)
        {
            try
            {
                if (path.EndsWith("\""))
                {
                    path = path.Remove(path.Length - 1);
                }

                //var p = new Uri(path, UriKind.RelativeOrAbsolute).LocalPath;
                var p = Path.GetFullPath(path);
                p = p.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                p = p.ToUpperInvariant();
                return p;
            }
            catch (Exception)
            {
                return path;
            }
        }

        /// <summary>
        /// Removes non file system compatible chars from file name.
        /// </summary>
        /// <param name="file">File name to clean.</param>
        /// <returns>Resulting filename.</returns>
        private static string PathSanitiseName(string file)
        {
            return Path.GetInvalidFileNameChars().Aggregate(file,
                (current, invalid) => current.Replace(invalid.ToString(CultureInfo.InvariantCulture), ""));
        }

        /// <summary>
        /// Gets MediaFile objects for all media files in the current directory.
        /// </summary>
        /// <param name="type">MediaFile type to use. Null for an automatic selection.</param>
        /// <returns>A list of MediaFiles.</returns>
        public MediaFile[] GetMediaFileList(Type type = null)
        {
            return GetMediaFileList(String.Empty, type);
        }

        /// <summary>
        /// Gets MediaFile objects for all media files in a directory.
        /// </summary>
        /// <param name="fileFolder">Directory/file to get the MediaFile(s) for.
        /// If is empty this will default to current directory.</param>
        /// <param name="type">MediaFile type to use. Null for an automatic selection.</param>
        /// <returns>A list of MediaFiles.</returns>
        public MediaFile[] GetMediaFileList(string fileFolder = "", Type type = null)
        {
            if (string.IsNullOrWhiteSpace(fileFolder))
            {
                fileFolder = Environment.CurrentDirectory;
            }
            var files = GetFileList(fileFolder);
            return files.Select(file => GetMediaFile(file, type)).ToArray();
        }

        /// <summary>
        /// Gets a media file for the provided file.
        /// </summary>
        /// <param name="file">File to get MediaFile for.</param>
        /// <param name="type">Type of the media file to create. If null will use the MediaTypeTester.</param>
        /// <returns>A new MediaFile</returns>
        public static MediaFile GetMediaFile(string file, Type type = null)
        {
            if (type == null)
            {
                return MediaTypeTester.GetMediaFile(file);
            }
            return (MediaFile)Activator.CreateInstance(type, file);
        }

        /// <summary>
        /// Gets a list of files in a directory or a list including the file provided.
        /// </summary>
        /// <param name="fileFolder">File or directory.</param>
        /// <returns>List of paths.</returns>
        private IEnumerable<string> GetFileList(string fileFolder)
        {
            return File.Exists(fileFolder) ? new[] { Path.GetFullPath(fileFolder) } : Directory.GetFiles(fileFolder, "*.*", SearchOption.AllDirectories).Where(file => FileExts.Any(file.EndsWith));
        }

        private static string GetMediaTypeDirectory(ref MediaFile file)
        {
            var result = "";
            var sp = file.OutputDirectory.Split('|');
            foreach (var s in sp)
            {
                var sub = s;
                while (Regex.IsMatch(sub, @".*\[ts\(.*\)\].*"))
                {
                    try
                    {
                        var ind1 = s.IndexOf("[ts(", StringComparison.Ordinal);
                        var sub2 = s.Substring(ind1);
                        var ind2 = sub2.IndexOf(")]", StringComparison.Ordinal);
                        var ts = sub2.Substring(4, ind2 - 4);
                        sub2 = file.ToString(ts) + sub2.Substring(ind2 + 2);
                        sub = sub.Substring(0, ind1) + sub2;
                    }
                    catch (Exception) { Debug.WriteLine("Parsing media directory tostring failed."); }
                }
                result += Path.DirectorySeparatorChar + sub;
            }
            return result;
        }
    }
}
