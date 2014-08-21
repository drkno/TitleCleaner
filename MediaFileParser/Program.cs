#region

using System;
using MediaFileParser.Arguments;

#endregion

namespace MediaFileParser
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            var arg = new Arguments.Arguments(args);
            var setupParser = new SetupParser(arg);
            setupParser.Run();
        }
    }
}