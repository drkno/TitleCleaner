using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TitleCleanerConsole
{
    public class OptionSet : IEnumerable
    {
        private readonly List<Option> _options = new List<Option>();
        private readonly Dictionary<string, int> _lookupDictionary = new Dictionary<string, int>(); 
        private struct Option
        {
            public string[] OptionStrings { get; private set; }
            public string Description { get; private set; }
            public Action<string> SetupFunction { get; private set; }

            public Option(string options, string description, Action<string> func)
                : this()
            {
                Description = description;
                SetupFunction = func;

                const string shortOptionsPrefix = "-";
                const string longOptionsPrefix = "--";

                var spl = options.Split(new[]{'|'}, StringSplitOptions.RemoveEmptyEntries);
                var opt = new List<string>();

                foreach (var s in spl)
                {
                    if (s.Length == 1)
                    {
                        opt.Add(shortOptionsPrefix + s);
                    }
                    else
                    {
                        opt.Add(longOptionsPrefix + s);
                    }
                }
                OptionStrings = opt.ToArray();
            }
        }

        public IEnumerator GetEnumerator()
        {
            return _options.GetEnumerator();
        }

        public void Add(string cliOptions, string description, Action<string> func, bool conflictSilent = true)
        {
            var option = new Option(cliOptions, description, func);
            _options.Add(option);
            var ind = _options.Count - 1;
            foreach (var opt in option.OptionStrings)
            {
                try
                {
                    _lookupDictionary.Add(opt, ind);
                }
                catch (Exception e)
                {
                    if (conflictSilent)
                    {
                        continue;
                    }
                    var opt1 = opt;
                    foreach (var op in option.OptionStrings.TakeWhile(op => op != opt1))
                    {
                        _lookupDictionary.Remove(op);
                    }
                    _options.Remove(option);
                    throw new OptionException("Option " + opt + " already specified for another option.", e, opt);
                }
            }
        }

        public List<string> Parse(IEnumerable<string> arguments)
        {
            var optionsInError = new List<string>();
            var temp = new List<string>();
            bool readForOption = false;
            int optionRead = -1;

            var enumerable = arguments.ToArray();

            for (var i = 0; i < enumerable.Count(); i++)
            {
                if (enumerable[i].StartsWith("-") || !readForOption)
                {
                    if (readForOption)
                    {
                        var arg = temp.Aggregate("", (current, t) => current + (t + " "));
                        arg = arg.Trim();

                        try
                        {
                            _options[optionRead].SetupFunction(arg);
                        }
                        catch (Exception)
                        {
                            optionsInError.Add(enumerable[i - 1 - temp.Count]);
                            optionsInError.AddRange(temp);
                        }
                        temp.Clear();
                    }

                    try
                    {
                        var ind = _lookupDictionary[enumerable[i]];
                        optionRead = ind;
                        readForOption = true;
                    }
                    catch (Exception)
                    {
                        optionsInError.Add(enumerable[i]);
                        readForOption = false;
                    }
                }
                else
                {
                    temp.Add(enumerable[i]);
                }
            }
            if (readForOption)
            {
                var arg = temp.Aggregate("", (current, t) => current + (t + " "));
                arg = arg.Trim();

                try
                {
                    _options[optionRead].SetupFunction(arg);
                }
                catch (Exception)
                {
                    optionsInError.Add(enumerable[enumerable.Length - 1 - temp.Count]);
                    optionsInError.AddRange(temp);
                }
            }
            return optionsInError;
        }

        public void ParseExceptionally(IEnumerable<string> arguments)
        {
            var result = Parse(arguments);
            if (result.Count <= 0) return;
            var options = "";
            options = result.Aggregate(options, (current, r) => current + (" " + r));
            throw new OptionException("Unknown option" + (result.Count > 1 ? "s" : "") + " " + options, result.ToArray());
        }

        public void ShowHelp(string programNameDescription,
                              string programSynopsis,
                              string optionDescriptionPrefix,
                              string optionDescriptionPostfix,
                              string programAuthor,
                              string programReportBugs,
                              string programCopyright,
                              bool confirm)
        {
            WriteProgramName(programNameDescription);
            WriteProgramSynopsis(programSynopsis);
            WriteOptionDescriptions(optionDescriptionPrefix, optionDescriptionPostfix);
            WriteProgramAuthor(programAuthor);
            WriteProgramReportingBugs(programReportBugs);
            WriteProgramCopyrightLicense(programCopyright);

            if (confirm)
            {
                Console.ReadKey(true);
            }
        }

        private void WriteProgramName(string description)
        {
            var origColour = Console.ForegroundColor;
            var appName = AppDomain.CurrentDomain.FriendlyName;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("NAME");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine('\t' + appName + " - " + description + '\n');
            Console.ForegroundColor = origColour;
        }

        private void WriteProgramSynopsis(string synopsis)
        {
            var origColour = Console.ForegroundColor;
            var appName = AppDomain.CurrentDomain.FriendlyName;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("SYNOPSIS");
            Console.ForegroundColor = ConsoleColor.Gray;
            synopsis = synopsis.Replace("{appName}", appName);
            Console.WriteLine('\t' + synopsis + '\n');
            Console.ForegroundColor = origColour;
        }

        private void WriteProgramAuthor(string authorByString)
        {
            var origColour = Console.ForegroundColor;
            var appName = AppDomain.CurrentDomain.FriendlyName;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("AUTHOR");
            Console.ForegroundColor = ConsoleColor.Gray;
            authorByString = authorByString.Replace("{appName}", appName);
            Console.WriteLine('\t' + authorByString + '\n');
            Console.ForegroundColor = origColour;
        }

        private void WriteProgramReportingBugs(string reportString)
        {
            var origColour = Console.ForegroundColor;
            var appName = AppDomain.CurrentDomain.FriendlyName;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("REPORTING BUGS");
            Console.ForegroundColor = ConsoleColor.Gray;
            reportString = reportString.Replace("{appName}", appName);
            var spl = reportString.Split(new[] { "\n", "\r\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var s in spl)
            {
                Console.WriteLine('\t' + s);
            }
            Console.WriteLine();
            Console.ForegroundColor = origColour;
        }

        private void WriteProgramCopyrightLicense(string copyrightLicense)
        {
            var origColour = Console.ForegroundColor;
            var appName = AppDomain.CurrentDomain.FriendlyName;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("COPYRIGHT");
            Console.ForegroundColor = ConsoleColor.Gray;
            copyrightLicense = copyrightLicense.Replace("{appName}", appName);
            var spl = copyrightLicense.Split(new[] { "\n", "\r\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var s in spl)
            {
                Console.WriteLine('\t' + s);
            }
            Console.WriteLine();
            Console.ForegroundColor = origColour;
        }

        private void WriteOptionDescriptions(string prefixText, string postText)
        {
            var origColour = Console.ForegroundColor;
            var appName = AppDomain.CurrentDomain.FriendlyName;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("DESCRIPTION");
            Console.ForegroundColor = ConsoleColor.Gray;
            if (!string.IsNullOrWhiteSpace(prefixText))
            {
                prefixText = prefixText.Replace("{appName}", appName);
                var spl = prefixText.Split(new[] { "\n", "\r\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var s in spl)
                {
                    Console.WriteLine('\t' + s);
                }
            }

            var buffWid = Console.BufferWidth;
            foreach (var p in _options)
            {
                Console.Write('\t');
                for (var j = 0; j < p.OptionStrings.Length; j++)
                {
                    Console.Write(p.OptionStrings[j]);
                    if (j + 1 != p.OptionStrings.Length)
                    {
                        Console.Write(", ");
                    }
                    else
                    {
                        Console.WriteLine();
                    }
                }

                Console.Write("\t\t");
                var len = buffWid - Console.CursorLeft;

                foreach (var l in p.Description.Split(new[] { "\n", "\r\n", "\r" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var lenP = 0;
                    foreach (var w in l.Split(' '))
                    {
                        var word = w;
                        if (lenP != 0)
                        {
                            word = ' ' + word;
                        }
                        if (lenP + word.Length > len)
                        {
                            Console.Write("\n\t\t");
                            lenP = 0;
                        }
                        Console.Write(word);
                        lenP += word.Length;
                    }
                    Console.Write("\n\t\t");
                }
                Console.WriteLine();
            }

            if (!string.IsNullOrWhiteSpace(postText))
            {
                postText = postText.Replace("{appName}", appName);
                var spl = postText.Split(new[] { "\n", "\r\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var s in spl)
                {
                    Console.WriteLine('\t' + s);
                }
            }
            Console.WriteLine();
            Console.ForegroundColor = origColour;
        }
    }

    public class OptionException : Exception
    {
        public string[] ErrorOptions { get; private set; }

        public OptionException(string errorText, params string[] errorOptions) : base(errorText)
        {
            ErrorOptions = errorOptions;
        }

        public OptionException(string errorText, Exception innerException, params string[] errorOptions) : base(errorText, innerException)
        {
            ErrorOptions = errorOptions;
        }
    }
}
