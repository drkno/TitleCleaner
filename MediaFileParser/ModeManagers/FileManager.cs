using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using MediaFileParser.MediaTypes;
using MediaFileParser.MediaTypes.MediaFile;

namespace MediaFileParser.ModeManagers
{
    public class FileManager
    {
        public delegate bool ConfirmAutomaticMoveDelegate(MediaFile file, string destination);
        public event ConfirmAutomaticMoveDelegate ConfirmAutomaticMove;

        public delegate void MoveDelegate(MediaFile file, string destination);
        public event MoveDelegate OnFileMove;
        public event MoveDelegate OnFileMoveFailed;

        private static readonly string[] FileExts =
        {
            "mov", "mkv", "flv", "avi",
            "mp4", "mpg", "vob", "m4v",
            "mpeg", "ogg", "swf", "wmv",
            "wtv", "h264"
        };
        private readonly bool _confirm;

        public FileManager(bool confirm)
        {
            _confirm = confirm;
        }

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

        private bool ConfirmMove(MediaFile file, string destination)
        {
            if (ConfirmAutomaticMove == null)
            {
                throw new NoNullAllowedException("For confirm to be enabled, a callback must be specified.");
            }
            return ConfirmAutomaticMove(file, destination);
        }

        public void MoveFiles(IEnumerable<MediaFile> files, string destination = "")
        {
            foreach (var mediaFile in files)
            {
                MoveFile(mediaFile, destination);
            }
        }

        public void MoveFile(MediaFile file, string destination = "")
        {
            if (string.IsNullOrWhiteSpace(destination))
            {
                destination = file.Location;
            }
            var path = Path.Combine(destination, file.ToString());
            var move = true;
            if (_confirm)
            {
                move = ConfirmMove(file, destination);
            }
            if (!move)
            {
                NotifyOnMove(file, destination, false);
                return;
            }

            try
            {
                Directory.CreateDirectory(destination);
                File.Move(file.Origional, path);
                NotifyOnMove(file, destination, true);
            }
            catch (Exception)
            {
                NotifyOnMove(file, destination, false);
            }
        }

        public MediaFile[] GetMediaFileList(Type type = null)
        {
            return GetMediaFileList(String.Empty, type);
        }

        public MediaFile[] GetMediaFileList(string fileFolder, Type type = null)
        {
            if (string.IsNullOrWhiteSpace(fileFolder))
            {
                fileFolder = Environment.CurrentDirectory;
            }
            var files = GetFileList(fileFolder);
            return files.Select(file => GetMediaFile(file, type)).ToArray();
        }

        public static MediaFile GetMediaFile(string file, Type type = null)
        {
            if (type == null)
            {
                return MediaTypeTester.GetMediaFile(file);
            }
            return (MediaFile)Activator.CreateInstance(type, file);
        }

        private IEnumerable<string> GetFileList(string fileFolder)
        {
            return File.Exists(fileFolder) ? new[] { Path.GetFullPath(fileFolder) } : Directory.GetFiles(fileFolder, "*.*", SearchOption.AllDirectories).Where(file => FileExts.Any(file.EndsWith));
        }
    }
}
