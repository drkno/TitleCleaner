#region

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using MediaFileParser.MediaTypes;
using MediaFileParser.MediaTypes.TvFile;
using MediaFileParser.MediaTypes.TvFile.Tvdb;

#endregion

namespace MediaFileParser
{
    internal class Program
    {
        private static void Main()
        {
            TvFile.TvdbLookup = true;
            TvFile.TvdbApiKey = "F9D98CE470B5ABAE";
            Tvdb.SeriesSelectCallback += Tvdb_SeriesSelectCallback;

            var reader = new StreamReader("../tv.csv");
            var i = 0;
            var f = 0;
            while (!reader.EndOfStream)
            {
                i++;
                if (i == 27)
                {
                    Debug.Write("");
                }
                var line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                var test = line.Split(',');
                var mediaFile = MediaTypeTester.GetMediaFile(test[0]);
                //var mediaFile = new TvFile(test[0]);
                test[0] = mediaFile.ToString("C.E");
                if (test[0].Trim() != test[1].Trim())
                {
                    f++;
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(i + ":\t" + mediaFile.Origional);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\t" + test[0]);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\t" + test[1]);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("Type:\t"+mediaFile.GetType().FullName.Split('.').Last());
                    Console.WriteLine("---");
                }
            }
            reader.Close();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Failures: " + f + "/" + i);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.ReadKey(true);
        }

        static TvdbSeries Tvdb_SeriesSelectCallback(TvdbSeriesSearch seriesSearch)
        {
            var colour = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Which show?");
            for (int i = 0; i < seriesSearch.Length; i++)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("[" + i + "] - " + seriesSearch[i].SeriesName);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(seriesSearch[i].Overview);
                Console.WriteLine();
            }
            Console.Write(" : ");
            Console.ForegroundColor = colour;
            var result = int.Parse(Console.ReadLine());
            return seriesSearch[result];
        }
    }
}