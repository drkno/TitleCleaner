using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using MediaFileParser.Common;
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
        /// Used for when accessing a file or directory results in an access denied error.
        /// </summary>
        /// <param name="sender">FileManager that sent this access denied message.</param>
        /// <param name="e">Exception that caused this error.</param>
        public delegate void AccessDeniedDelegate(FileManager sender, UnauthorizedAccessException e);
        /// <summary>
        /// Called when access to a file is denied.
        /// </summary>
        public event AccessDeniedDelegate OnAccessDenied;
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
                if (orig != clea || !String.Equals(orig, clea, StringComparison.OrdinalIgnoreCase) ||
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
            var mediaFiles = new List<MediaFile>();
            var enumerator = files.GetEnumerator();
            while (true)
            {
                try
                {
                    if (!enumerator.MoveNext()) break;
                    mediaFiles.Add(GetMediaFile(enumerator.Current, type));
                }
                catch (UnauthorizedAccessException e)
                {
                    OnAccessDenied.Invoke(this, e);
                }
            }
            return mediaFiles.ToArray();
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
        private static IEnumerable<string> GetFileList(string fileFolder)
        {
            return File.Exists(fileFolder) ? new[] { Path.GetFullPath(fileFolder) } : Directory.EnumerateFileSystemEntries(fileFolder, "*.*", SearchOption.AllDirectories).Where(file => FileExts.Any(file.EndsWith));
        }

        /// <summary>
        /// Gets the directory to store this media type in when moving.
        /// </summary>
        /// <param name="file">Media file that is being moved.</param>
        /// <returns>The path to the directory.</returns>
        private static string GetMediaTypeDirectory(ref MediaFile file)
        {
            var result = new StringBuilder();
            var sp = file.OutputDirectory.Split('|');
            foreach (var s in sp)
            {
                int ind;
                var res = s;
                while ((ind = res.IndexOf("[ts(", StringComparison.Ordinal)) >= 0)
                {
                    var str = res.GetBracketedString(ind + 3);
                    var end = string.IsNullOrWhiteSpace(str) ? 0 : str.Length + 3;
                    str = end == 0 ? "" : file.ToString(str);
                    res = res.Substring(0, ind) + str + res.Substring(ind + 3 + end);
                }
                result.Append(Path.DirectorySeparatorChar);
                result.Append(res);
            }
            return result.ToString();
        }
    }
}
