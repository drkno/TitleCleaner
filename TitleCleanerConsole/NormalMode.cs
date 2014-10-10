using System;
using MediaFileParser.MediaTypes.TvFile;
using MediaFileParser.MediaTypes.TvFile.Tvdb;
using MediaFileParser.ModeManagers;
using MediaFileParser.MediaTypes.MediaFile;

namespace TitleCleanerConsole
{
    class NormalMode
    {
        private static bool _conf;
        public static void Run(bool confirm, string inputDir, string outputDir, Type mediaType)
        {
            _conf = confirm;
            var consoleColor = Console.ForegroundColor;

            var fileManager = new FileManager(_conf);
            fileManager.ConfirmAutomaticMove += FileManagerConfirmAutomaticMove;
            fileManager.OnFileMove += fileManager_OnFileMove;
            fileManager.OnFileMoveFailed += fileManager_OnFileMoveFailed;

            TvFile.TvdbSearchSelectionRequired += TvFileTvdbSearchSelectionRequired;
            
            var mediaFiles = fileManager.GetMediaFileList(inputDir);
            fileManager.MoveFiles(mediaFiles, outputDir);

            Console.ForegroundColor = consoleColor;
        }

        private static uint TvFileTvdbSearchSelectionRequired(TvdbSeries[] seriesSearch)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Which TV Series does the file refer to?");
            int num = 0;
            foreach (var series in seriesSearch)
            {
                num++;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("(" + num + "): " + series.SeriesName);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(series.Overview + "\n");
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("(number/enter for none): ");
            Console.ForegroundColor = ConsoleColor.Gray;
            int result;
            string temp;
            while (!int.TryParse((temp = Console.ReadLine()), out result) || temp != "" || result < 0 || result > num)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Please type a valid number/press enter for none: ");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            return result == 0 ? 0 : seriesSearch[result-1].Id;
        }

        private static bool GetBool(string b, out bool res)
        {
            switch (b.ToLower().Trim())
            {
                case "true":
                case "yes":
                case "y":
                {
                    res = true;
                    return true;
                }
                case "false":
                case "no":
                case "n":
                {
                    res = false;
                    return true;
                }
                default:
                {
                    res = false;
                    return false;
                }
            }
        }

        private static void fileManager_OnFileMoveFailed(MediaFile file, string destination)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write('[');
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Fail");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("] ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(file.ToString("O.E"));
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("   -> ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(file.ToString());
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" DIR: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(destination + Environment.NewLine);

            if (_conf)
            {
                Console.ReadKey();
            }
        }

        private static void fileManager_OnFileMove(MediaFile file, string destination)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write('[');
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Done");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("] ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(file + Environment.NewLine);

            if (_conf)
            {
                Console.ReadKey();
            }
        }

        private static bool FileManagerConfirmAutomaticMove(MediaFile file, string destination)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Move\t");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(file.ToString("O.E"));
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("To\t");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(file.ToString());
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("(yes/no): ");
            Console.ForegroundColor = ConsoleColor.Gray;
            bool result;
            while (!GetBool(Console.ReadLine(), out result))
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Please type yes or no: ");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            return result;
        }
    }
}
