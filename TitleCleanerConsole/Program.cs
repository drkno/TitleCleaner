using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using MediaFileParser.MediaTypes;
using MediaFileParser.MediaTypes.MediaFile;
using MediaFileParser.MediaTypes.MovieFile;
using MediaFileParser.MediaTypes.TvFile;

namespace TitleCleanerConsole
{
    public class Program
    {
        private static bool _help;
        private static bool _confirm;
        private static Type _type;
        private static string _mode;
        private static string _inputDir;
        private static string _outputDir;
        /* API Key for this program and the program it was
           designed to be used as a part of (TitleCleaner) ONLY
           If you modify this program, or use parts of its code
           get your own API key. */
        private const string TvdbApiKey = "F9D98CE470B5ABAE";

        public static void Main(string[] args)
        {
            try
            {
                var p = SetupOptions();
                p.ParseExceptionally(args);

                if (_help)
                {
                    ShowHelp(p, _confirm);
                    Environment.Exit(0);
                }

                TvFile.TvdbApiKey = TvdbApiKey;
                if (_confirm)
                {
                    TvFile.TvdbLookupConfirm = true;
                }

                MediaTypeTester.SetMediaType(_type);

                switch (_mode)
                {
                    case "normal":
                    {
                        NormalMode.Run(_confirm, _inputDir, _outputDir, _type);
                        break;
                    }
                    case "test":
                    {
                        TestMode.Run(_confirm, _inputDir, _outputDir, _type);
                        break;
                    }
                    default: goto case "normal";
                }
            }
            catch (Exception e)
            {
                var name = AppDomain.CurrentDomain.FriendlyName;
                if (e is OptionException)
                {
                    Console.WriteLine(name + ": " + e.Message);
                }
                else
                {
                    Console.Error.WriteLine("A fatal error occured while running " + name +
                        ". One of your options was probably malformed. ");
                }

                Console.WriteLine("Try '" + name + " --help' for more information.");

                if (_confirm)
                {
                    Console.ReadKey(true);
                }

                Environment.Exit(0);
            }
        }

        private static Type GetType(string mode)
        {
            Type mediaType;
            switch (mode)
            {
                case "auto":    { mediaType = null; break; }
                case "tv":      { mediaType = typeof(TvFile); break; }
                case "movie":   { mediaType = typeof(MovieFile); break; }
                default:        { goto case "auto"; }
            }
            return mediaType;
        }

        private static OptionSet SetupOptions()
        {
            var p = new OptionSet
            {
                { "m|mode",         "The {mode} to run this application in (normal/test).",     v => _mode = v.ToLower().Trim() },
                { "t|type",         "The {type} of media files being input (tv/movie/auto).",   v => _type = GetType(v.ToLower().Trim()) },
                { "i|tvdb",         "Retreives new missing tv episode names from the TVDB.",    v => TvFile.TvdbLookup = true },
                { "c|confirm",      "Asks for confirmation on rename/move/test.",               v => _confirm = true },
                { "o|out",          "Move media files to {directory}. In test mode this " +
                                    "outputs the outcomes of the test cases to the specified " +
                                    "file.",                                                    v => _outputDir = v },
                { "s|format",       "{Format string} to use for output file naming.",           v => MediaFile.DefaultFormatString = v },
                { "v|tformat",      "{Format string} to use for output TV file naming. This " +
                                    "option overrides the -s option.",                          v => TvFile.DefaultFormatString = v },
                { "e|mformat",      "{Format string} to use for output movie file naming. " +
                                    "This option overrides the -s option.",                     v => MovieFile.DefaultFormatString = v },
                { "h|help",         "Display this help.",                                       v => _help = true },
                { "w|tvdir",        "Sets the output directory subdirectory for tv files.",     v => TvFile.TypeDirectory = v},
                { "r|movdir",       "Sets the output directory subdirectory for movie files.",  v => MovieFile.TypeDirectory = v},
                { "d|directory",    "The {directory} to search for files. When used the " +
                                    "directory provided will be searched instead of the current" +
                                    " directory for media files. This option is mutually " +
                                    "exclusive with -f.",
                    v => {
                        if (!string.IsNullOrWhiteSpace(_inputDir))
                        {
                            throw new OptionException("-d or -f already specified. These options are incompatible.", "-d");
                        }
                        _inputDir = v;
                    }
                },
                { "f|file",        "Cleans an individual {file} instead of searching. The behaviour " +
                                    "of the application is otherwise unchanged. This option is mutually" +
                                    " exclusive with -d.\n" +
                                    "In test mode this specifies a test .csv file to run.",
                    v => {
                        if (!string.IsNullOrWhiteSpace(_inputDir))
                        {
                            throw new OptionException("-d or -f already specified. These options are incompatible.", "-f");
                        }
                        _inputDir = v;
                    }
                }
            };
            return p;
        }

        private static void ShowHelp(OptionSet p, bool conf)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

            var build = new DateTime(2000 + fvi.FilePrivatePart / 1000, 1, 1);
            build = build.AddDays(int.Parse(fvi.FilePrivatePart.ToString(CultureInfo.InvariantCulture).Substring(2)) - 1);

            p.ShowHelp("Tidy up and relocate media files.","{appName} [OPTION]...",
                       "If no options are specified defaults will be used.",
                       null,
                       "Written by Matthew Knox.",
                       "Version:\t" + fvi.ProductVersion + " " + ((Environment.Is64BitProcess) ? "x64" : "x32") +
                       "\nBuild:\t\tr" + fvi.FileBuildPart + " " + build.ToString("d") +
                       "\nCLR Version:\t" + Environment.Version +
                       "\nOS Version:\t" + Environment.OSVersion.VersionString +
                       "\nReport {appName} bugs and above information to the bug tracker at\n" +
                       "<https://github.com/mrkno/TitleCleaner>",
                       "Copyright © " + DateTime.Now.Year + " Knox Enterprises.\n"
                       + "The MIT License (MIT) <http://opensource.org/licenses/MIT>\n"
                       + "This is free software: you are free to change and redistribute it.\n"
                       + "There is NO WARRANTY, to the extent permitted by law.",
                       conf
            );
        }
    }
}
