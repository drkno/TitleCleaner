#region

using System;
using System.IO;

#endregion

namespace MediaFileParser
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var reader = new StreamReader("../tests.csv");
            var i = 0;
            var f = 0;
            while (!reader.EndOfStream)
            {
                i++;
                var line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                var test = line.Split(',');
                var mediaFile = new TvFile(test[0]);
                test[0] = mediaFile.ToString("C.E");
                if (test[0].Trim() != test[1].Trim())
                {
                    f++;
                    Console.WriteLine(i + ":\t" + test[0]);
                    Console.WriteLine("\t" + test[1]);
                    var ep = "";
                    if (mediaFile.Episode != null)
                    {
                        foreach (var episode in mediaFile.Episode)
                        {
                            ep += episode + ",";
                        }
                    }
                    Console.WriteLine("\tS: " + mediaFile.Season + "\tE: " + ep);
                    Console.WriteLine("---");
                }
            }
            reader.Close();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Failures: " + f + "/" + i);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.ReadKey(true);
        }
    }
}