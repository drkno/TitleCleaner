﻿using System;
using MediaFileParser.MediaTypes;
using MediaFileParser.MediaTypes.MediaFile;
using MediaFileParser.MediaTypes.MovieFile;
using MediaFileParser.MediaTypes.TvFile;

namespace TitleCleanerConsole
{
    class Program
    {
        public static void Main(string[] args)
        {
            bool help = false, confirm = false;
            string mode=null, inputDir=null, outputDir=null;
            Type type = null;

            var p = new OptionSet
            {
                { "m|mode=",        "The {MODE} to run this application in (normal/test).",     v => mode = v.ToLower().Trim() },
                { "t|type=",        "The {TYPE} of media files being input (tv/movie/auto).",   v => type = GetType(v.ToLower().Trim()) },
                { "i|tvdb",         "Retreives new missing tv episode names from the TVDB.",    v => TvFile.TvdbLookup = true },
                { "c|confirm",      "Asks for confirmation on rename/move/test.",               v => confirm = true },
                { "o|out=",         "Move media files to {DIRECTORY}.",                         v => outputDir = v },
                { "s|format=",      "{FORMAT} to use for output file naming.",                  v => MediaFile.DefaultFormatString = v },
                { "v|tformat=",     "{FORMAT} to use for output TV file naming. This option " +
                                    "overrides the -s option.",                                 v => TvFile.DefaultFormatString = v },
                { "e|mformat=",     "{FORMAT} to use for output movie file naming. This option " +
                                    "overrides the -s option.",                                 v => MovieFile.DefaultFormatString = v },
                { "h|help",         "Display this help.",                                       v => help = true },
                { "d|directory=",   "The {DIRECTORY} to search for files. When used the " +
                                    "directory provided will be searched instead of the current" +
                                    " directory for media files. This option is mutually " +
                                    "exclusive with -f.",
                  v =>
                  {
                      if (!string.IsNullOrWhiteSpace(inputDir))
                      {
                          throw new OptionException("-d or -f already specified. These options are incompatible.", "-d");
                      }
                      inputDir = v;
                  }},

                { "f|file=",        "Cleans an individual {FILE} instead of searching. The behaviour " +
                                    "of the application is otherwise unchanged. This option is mutually" +
                                    " exclusive with -d.\n" +
                                    "In test mode this specifies a test .csv file to run.",
                  v =>
                  {
                      if (!string.IsNullOrWhiteSpace(inputDir))
                      {
                          throw new OptionException("-d or -f already specified. These options are incompatible.", "-f");
                      }
                      inputDir = v;
                  }}
            };

            try
            {
                p.ParseExceptionally(args);

                if (help)
                {
                    ShowHelp(p, confirm);
                    Environment.Exit(0);
                }

                if (confirm)
                {
                    TvFile.TvdbLookupConfirm = true;
                }

                MediaTypeTester.SetMediaType(type);

                switch (mode)
                {
                    case "normal":
                    {
                        NormalMode.Run(confirm, inputDir, outputDir, type);
                        break;
                    }
                    case "test":
                    {
                        //TestMode.Run(confirm, inputDir, outputDir, type);
                        break;
                    }
                    default: goto case "normal";
                }
            }
            catch (OptionException e)
            {
                var name = AppDomain.CurrentDomain.FriendlyName;
                Console.Write(name + ": ");
                Console.WriteLine(e.Message);
                Console.WriteLine("Try '" + name + " --help' for more information.");

                if (confirm)
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
                case "auto":
                    {
                        mediaType = null;
                        break;
                    }
                case "tv":
                    {
                        mediaType = typeof(TvFile);
                        break;
                    }
                case "movie":
                    {
                        mediaType = typeof(MovieFile);
                        break;
                    }
                default: goto case "auto";
            }
            return mediaType;
        }

        private static void ShowHelp(OptionSet p, bool conf)
        {
            p.WriteProgramName("Tidy up and relocate media files.");
            p.WriteProgramSynopsis("{appName} [OPTION]...");
            p.WriteOptionDescriptions("If no options are specified defaults will be used.", null);
            p.WriteProgramAuthor("Written by Matthew Knox.");
            p.WriteProgramReportingBugs("Report {appName} bugs to the bug tracker at\n" +
                                        "<https://github.com/mrkno/TitleCleaner>");
            p.WriteProgramCopyrightLicense("Copyright © " + DateTime.Now.Year + " Knox Enterprises.\n"
                + "The MIT License (MIT) <http://opensource.org/licenses/MIT>\n"
                + "This is free software: you are free to change and redistribute it.\n"
                + "There is NO WARRANTY, to the extent permitted by law.");

            if (conf)
            {
                Console.ReadKey(true);
            }
        }
    }
}
