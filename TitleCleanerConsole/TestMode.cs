using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using MediaFileParser.ModeManagers;

namespace TitleCleanerConsole
{
    class TestMode
    {
        private static bool _conf;
        private static int _passed, _failed;
        private static readonly List<int> FailedCases = new List<int>();
        private static StreamWriter _writer;
        public static void Run(bool confirm, string inputDir, string outputDir, Type type)
        {
            if (!string.IsNullOrWhiteSpace(outputDir) && Directory.Exists(outputDir))
            {
                throw new OptionException("The -o and -d directory options are not supported in test mode.", "-o,-d");
            }

            if (string.IsNullOrWhiteSpace(inputDir) || Directory.Exists(inputDir) || !File.Exists(inputDir))
            {
                throw new OptionException("The -f option must be specified and be a valid tests file in test mode.", "-f");
            }

            if (!string.IsNullOrWhiteSpace(outputDir)) _writer = new StreamWriter(outputDir);

            _conf = confirm;

            var manager = new TestManager(inputDir, type);

            manager.TestCaseDidFail += ManagerTestCaseDidFail;
            manager.TestCaseDidPass += ManagerTestCaseDidPass;
            manager.TestCaseEncounteredError += ManagerTestCaseEncounteredError;

            manager.RunTests();

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\nPassed: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(_passed);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Failed: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(_failed);

            if (_failed != 0)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Press [f] to show list of failed tests or [any key] to quit.");
                if (Console.ReadKey(true).Key == ConsoleKey.F)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write('{');
                    for (var i = 0; i < FailedCases.Count; i++)
                    {
                        Console.Write(FailedCases[i]);
                        if (i + 1 != FailedCases.Count)
                        {
                            Console.Write(", ");
                        }
                    }
                    Console.WriteLine('}');
                }
                else
                {
                    return;
                }
            }

            if (_writer != null) _writer.Close();
            if (!confirm) return;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Press [any key] to quit.");
            Console.ReadKey(true);
        }

        private static void ManagerTestCaseEncounteredError(TestManager.TestCase testCase)
        {
            _failed++;
            FailedCases.Add(testCase.Index);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(testCase.Index + ": [");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Erro");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("] - ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(testCase.DestinationName);

            if (_writer != null) _writer.WriteLine(testCase.OrigionalName + "," + testCase.MediaFile);

            if (_conf)
            {
                Console.ReadKey(true);
            }
        }

        private static void ManagerTestCaseDidFail(TestManager.TestCase testCase)
        {
            _failed++;
            FailedCases.Add(testCase.Index);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(testCase.Index + ": [");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Fail");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("] - ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(testCase.MediaFile);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(new string(' ', testCase.Index.ToString(CultureInfo.InvariantCulture).Length + 11) + testCase.DestinationName);

            if (_writer != null) _writer.WriteLine(testCase.OrigionalName + "," + testCase.MediaFile);

            if (_conf)
            {
                Console.ReadKey(true);
            }
        }

        private static void ManagerTestCaseDidPass(TestManager.TestCase testCase)
        {
            _passed++;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(testCase.Index + ": [");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Pass");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("] - ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(testCase.DestinationName);
            if (_writer != null) _writer.WriteLine(testCase.OrigionalName + "," + testCase.MediaFile);
        }
    }
}
