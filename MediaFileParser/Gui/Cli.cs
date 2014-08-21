using System;
using MediaFileParser.Arguments;
using MediaFileParser.MediaTypes.TvFile;
using MediaFileParser.MediaTypes.TvFile.Tvdb;
using MediaFileParser.ModeManagers;

namespace MediaFileParser.Gui
{
    class Cli : IGui
    {
        public void Start(SetupParser.RunMode runMode, bool confirm)
        {
            switch (runMode)
            {
                    case SetupParser.RunMode.File: FileMode(confirm); break;
                    case SetupParser.RunMode.Test: TestMode(confirm); break;
            }
        }

        private void FileMode(bool confirm)
        {
            var fileManager = new FileManager(confirm);
            TvFile.TvdbSearchSelectionRequired += TvFileTvdbSearchSelectionRequired;
            fileManager.ConfirmAutomaticMove += FileManagerConfirmAutomaticMove;
            fileManager.OnFileMove += fileManager_OnFileMove;
            fileManager.OnFileMoveFailed += fileManager_OnFileMoveFailed;
            var mediaFiles = fileManager.GetMediaFileList();
            fileManager.MoveFiles(mediaFiles);
            WriteLnColor("----", ConsoleColor.White);
            WriteLnColor("Done.", ConsoleColor.Green);
            if (confirm) Console.ReadKey(true);
        }

        private uint TvFileTvdbSearchSelectionRequired(TvdbSeries[] seriesSearch)
        {
            WriteLnColor("TVDB Series Selection Required.", ConsoleColor.Gray);
            for (int i = 0; i < seriesSearch.Length; i++)
            {
                WriteLnColor(i + ": " + seriesSearch[i].SeriesName, ConsoleColor.Yellow);
                WriteLnColor(seriesSearch[i].Overview, ConsoleColor.White);
                Console.WriteLine();
            }
            WriteLnColor("Enter series number: ", ConsoleColor.Gray);
            uint number;
            if (!uint.TryParse(Console.ReadLine(), out number) || number >= seriesSearch.Length)
            {
                WriteLnColor("No selection made.", ConsoleColor.Yellow);
                number = 0;
            }
            return number;
        }

        private void fileManager_OnFileMoveFailed(MediaTypes.MediaFile.MediaFile file, string destination)
        {
            WriteLnColor("Rename/Move Failed.", ConsoleColor.Red);
            WriteColor("From:\t\t", ConsoleColor.Yellow);
            WriteLnColor(file.Origional, ConsoleColor.White);
            WriteColor("To:\t\t", ConsoleColor.Yellow);
            WriteLnColor(file.Cleaned, ConsoleColor.White);
            WriteColor("Directory:\t", ConsoleColor.Yellow);
            WriteLnColor(destination, ConsoleColor.White);
        }

        private void fileManager_OnFileMove(MediaTypes.MediaFile.MediaFile file, string destination)
        {
            WriteLnColor("Rename/Move Done.", ConsoleColor.Green);
            WriteColor("From:\t\t", ConsoleColor.Yellow);
            WriteLnColor(file.Origional, ConsoleColor.White);
            WriteColor("To:\t\t", ConsoleColor.Yellow);
            WriteLnColor(file.Cleaned, ConsoleColor.White);
            WriteColor("Directory:\t", ConsoleColor.Yellow);
            WriteLnColor(destination, ConsoleColor.White);
        }

        private bool FileManagerConfirmAutomaticMove(MediaTypes.MediaFile.MediaFile file, string destination)
        {
            WriteLnColor("Perform this rename/move? " + destination, ConsoleColor.Gray);
            WriteColor("Origional:\t", ConsoleColor.Yellow);
            WriteLnColor(file.Origional, ConsoleColor.White);
            WriteColor("Cleaned:\t", ConsoleColor.Yellow);
            WriteLnColor(file.Cleaned, ConsoleColor.White);
            WriteLnColor("Rename: [Y/N]\tCancel: [Ctrl-C] ", ConsoleColor.Gray);
            var key = Console.ReadKey(true).Key;
            return key == ConsoleKey.Y;
        }

        private void TestMode(bool confirm)
        {
            
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        private static void WriteColor(string line, ConsoleColor colour)
        {
            var origColour = Console.ForegroundColor;
            Console.ForegroundColor = colour;
            Console.Write(line);
            Console.ForegroundColor = origColour;
        }

        private void WriteLnColor(string line, ConsoleColor colour)
        {
            line += Environment.NewLine;
            WriteColor(line, colour);
        }

        private void RuntimeError(int errorCode = 0, string errorText = "", Exception errorException = null)
        {
            if (errorCode != 0)
            {
                WriteColor("[" + errorCode.ToString().PadRight(3) + "] ", ConsoleColor.Red);
            }
            if (!string.IsNullOrWhiteSpace(errorText))
            {
                WriteColor(errorText + " ", ConsoleColor.Red);
            }
            if (errorException != null)
            {
                WriteColor(" : " + errorException.Message, ConsoleColor.Red);
            }
            Console.WriteLine();
        }
    }
}
