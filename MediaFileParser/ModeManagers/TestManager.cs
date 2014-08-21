using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using MediaFileParser.MediaTypes.MediaFile;

namespace MediaFileParser.ModeManagers
{
    

    class TestManager
    {
        private readonly Type _type;
        public string TestsFile { get; protected set; }

        private readonly List<TestCase> _testCases; 

        public TestManager(string testsFile, Type type = null)
        {
            _type = type;
            TestsFile = testsFile;
            _testCases = new List<TestCase>();
            GetTestCases();
        }

        private void GetTestCases()
        {
            var reader = new StreamReader(TestsFile);
            var index = 0;
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;
                var split = line.Split(',');
                _testCases.Add(new TestCase(++index, split[0], split[1]));
            }
            reader.Close();
        }

        public class TestCase
        {
            public int Index { get; protected set; }
            public string OrigionalName { get; protected set; }
            public string DestinationName { get; protected set; }
            public MediaFile MediaFile { get; protected set; }
            public TestCase(int index, string orig, string dest)
            {
                Index = index;
                OrigionalName = orig;
                DestinationName = dest;
            }

            public bool RunTest(Type type = null)
            {
                MediaFile = FileManager.GetMediaFile(OrigionalName, type);
                return MediaFile.ToString() == DestinationName;
            }
        }

        public TestCase this[int param]
        {
            get { return _testCases[param]; }
        }

        public int Count { get { return _testCases.Count; } }

        public delegate void TestCaseRun(TestCase testCase);

        public event TestCaseRun TestCaseDidPass;
        public event TestCaseRun TestCaseDidFail;
        public event TestCaseRun TestCaseEncounteredError;

        public void RunTests()
        {
            foreach (var testCase in _testCases)
            {
                try
                {
                    var result = testCase.RunTest(_type);
                    if (result && TestCaseDidPass != null)
                    {
                        TestCaseDidPass(testCase);
                    }
                    else if (!result && TestCaseDidFail != null)
                    {
                        TestCaseDidFail(testCase);
                    }
                    else if (TestCaseDidPass != null && TestCaseDidFail != null && TestCaseEncounteredError != null)
                    {
                        TestCaseEncounteredError(testCase);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Test case " + testCase.Index + " threw an exception:\n" + e.Message);
                    if (TestCaseEncounteredError != null)
                    {
                        TestCaseEncounteredError(testCase);
                    }
                }
            }
        }
    }
}
