using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using MediaFileParser.Gui;
using MediaFileParser.Gui.Windowed;
using MediaFileParser.MediaTypes;
using MediaFileParser.MediaTypes.MovieFile;
using MediaFileParser.MediaTypes.TvFile;

namespace MediaFileParser.Arguments
{
    public class SetupParser
    {
        private static RunMode _runMode;
        public static void SetRunMode(RunMode runMode)
        {
            _runMode = runMode;
        }

        private class TvdbArgument : ISetupArgument
        {
            public bool Run(string args)
            {
                var b = MediaFileParser.Arguments.Arguments.GetBool(args);
                if (!b) return false;
                // This Api Key is for TitleCleaner ONLY
                TvFile.TvdbApiKey = "F9D98CE470B5ABAE";
                TvFile.TvdbLookup = true;
                return false;
            }
            public string Description { get { return "Enables online TVDB searching for missing TV episode titles."; } }
            public string Arguments { get { return "t|tvdb|internet"; } }
        }

        private class ConfirmArgument : ISetupArgument
        {
            public bool Run(string args)
            {
                var b = MediaFileParser.Arguments.Arguments.GetBool(args);
                HelpWait = b;
                return false;
            }
            public string Description { get { return "Asks for confirmation before renaming files or after each test."; } }
            public string Arguments { get { return "c|confirm|ask"; } }
        }

        public static bool HelpWait { get; set; }

        private class MoveArgument : ISetupArgument
        {
            public bool Run(string args)
            {
                
                return false;
            }
            public string Description { get { return "Directory to create a media directory structure and move files to."; } }
            public string Arguments { get { return "m|move"; } }
        }

        private class TypeArgument : ISetupArgument
        {
            public bool Run(string args)
            {
                Type type;
                switch (args)
                {
                    case "auto":
                        type = null;break;
                    case "tv":
                        type = typeof (TvFile); break;
                    case "movie":
                        type = typeof (MovieFile); break;
                    default: throw new ArgumentException("Invalid file type provided.", "args");
                }
                MediaTypeTester.SetMediaType(type);
                return false;
            }
            public string Description { get { return "Type of files to parse (tv|movie|auto)."; } }
            public string Arguments { get { return "y|type"; } }
        }

        public enum RunMode
        {
            File = 0,
            Test = 1
        }

        private class ModeArgument : ISetupArgument
        {
            public bool Run(string args)
            {
                args = args.ToLower().Trim();
                RunMode runMode;
                switch (args)
                {
                    case "file":
                        runMode = RunMode.File; break;
                    case "test":
                        runMode = RunMode.Test; break;
                    default:
                        goto case "file";
                }
                SetRunMode(runMode);
                return false;
            }
            public string Description { get { return "Type of files to parse (tv|movie|auto)."; } }
            public string Arguments { get { return "o|mode"; } }
        }

        private class FileArgument : ISetupArgument
        {
            public bool Run(string args)
            {

                return false;
            }
            public string Description { get { return "Folder or file to clean or file to test ."; } }
            public string Arguments { get { return "f|folder|file"; } }
        }

        private static IGui _runGui;

        private class UiArgument : ISetupArgument
        {
            public bool Run(string args)
            {
                args = args.ToLower().Trim();
                switch (args)
                {
                    case "gui":
                    case "g":
                        _runGui = new MainWindow();
                        break;
                    case "cli":
                    case "c":
                        _runGui = new Cli();
                        break;
                    default:
                        goto case "c";
                }
                return false;
            }

            public string Description { get { return "The UI type to tun the program as."; } }
            public string Arguments { get { return "u|ui"; } }
        }

        private interface ISetupArgument
        {
            bool Run(string args);
            string Description { get; }
            string Arguments { get; }
        }

        private readonly ISetupArgument[] _programArguments =
        {
            new TvdbArgument(), 
            new ConfirmArgument(), 
            new MoveArgument(), 
            new TypeArgument(), 
            new ModeArgument(), 
            new FileArgument(), 
            new UiArgument()
        };

        public SetupParser(Arguments args)
        {
            foreach (var programArgument in _programArguments)
            {
                var arg = programArgument.Arguments.Split('|');
                var argument = programArgument;
                foreach (var result in arg.Where(args.ContainsKey).Select(s => argument.Run(args[s])))
                {
                    if (result) return;
                    break;
                }
            }

            if (Arguments.GetBool(args["help"]) || Arguments.GetBool(args["h"]) || Arguments.GetBool(args["?"]))
            {
                PrintHelp();
                return;
            }

            
        }

        /// <summary>
        /// Prints CLI Arguments Help
        /// </summary>
        private void PrintHelp()
        {
            var foregroundColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("TitleCleaner - Fix Scene Release Titles");
            Console.ForegroundColor = ConsoleColor.Gray;
            var assembly = Assembly.GetExecutingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            Console.WriteLine("Version:\t" + fvi.ProductVersion + " " + ((Environment.Is64BitProcess) ? "x64" : "x32"));
            Console.WriteLine("CLR Version:\t" + Environment.Version);
            Console.WriteLine("OS Version:\t" + Environment.OSVersion.VersionString);
            Console.WriteLine("Running As:\t" + Environment.UserName + "\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Defaults to current directory, normal mode, auto.\n");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Command Line Switches:");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("These switches can be used to change how the program operates.");
            foreach (var individualSetting in _programArguments)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("   ");
                var args = individualSetting.Arguments.Replace('|', ',');
                Console.Write(args);
                Console.CursorLeft = 20;
                Console.Write(" : ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(individualSetting.Description);
            }
            Console.WriteLine();
            Console.ForegroundColor = foregroundColor;
            if (HelpWait)
            {
                Console.ReadKey();
            }
            Environment.Exit(0);
        }

        public void Run()
        {
            _runGui.Start();
        }
    }
}
