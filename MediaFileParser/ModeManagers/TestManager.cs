using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using MediaFileParser.MediaTypes.MediaFile;

namespace MediaFileParser.ModeManagers
{
    /// <summary>
    /// Manages testing the TitleCleanerLib and proposed outputs.
    /// </summary>
    class TestManager
    {
        /// <summary>
        /// MediaFile type to use for tests.
        /// </summary>
        private readonly Type _type;
        /// <summary>
        /// File to use for testing.
        /// </summary>
        public string TestsFile { get; protected set; }
        /// <summary>
        /// List of test cases.
        /// </summary>
        private readonly List<TestCase> _testCases; 

        /// <summary>
        /// Instantiates a new test manager.
        /// </summary>
        /// <param name="testsFile">CSV file that contains the tests.</param>
        /// <param name="type">Type of MediaFile to use.</param>
        public TestManager(string testsFile, Type type = null)
        {
            _type = type;
            TestsFile = testsFile;
            _testCases = new List<TestCase>();
            GetTestCases();
        }

        /// <summary>
        /// Generates all the test cases.
        /// </summary>
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

        /// <summary>
        /// Class to store test cases.
        /// </summary>
        public class TestCase
        {
            /// <summary>
            /// Index of the test case.
            /// </summary>
            public int Index { get; protected set; }
            /// <summary>
            /// Origional name of the file.
            /// </summary>
            public string OrigionalName { get; protected set; }
            /// <summary>
            /// New name of the file.
            /// </summary>
            public string DestinationName { get; protected set; }
            /// <summary>
            /// Media file used for the conversion.
            /// </summary>
            public MediaFile MediaFile { get; protected set; }
            /// <summary>
            /// Instantiates a new TestCase.
            /// </summary>
            /// <param name="index">index of the test case</param>
            /// <param name="orig">origional name</param>
            /// <param name="dest">new name</param>
            public TestCase(int index, string orig, string dest)
            {
                Index = index;
                OrigionalName = orig;
                DestinationName = dest;
            }

            /// <summary>
            /// Runs the test case.
            /// </summary>
            /// <param name="type">Type MediaFile to use. If is null will use automatic selection.</param>
            /// <returns>The test success.</returns>
            public bool RunTest(Type type = null)
            {
                MediaFile = FileManager.GetMediaFile(OrigionalName, type);
                return MediaFile.ToString() == DestinationName;
            }
        }

        /// <summary>
        /// Gets an individual test case.
        /// </summary>
        /// <param name="param">Index of the test case.</param>
        /// <returns>The test case.</returns>
        public TestCase this[int param]
        {
            get { return _testCases[param]; }
        }

        /// <summary>
        /// Gets the total number of test cases.
        /// </summary>
        public int Count { get { return _testCases.Count; } }
        /// <summary>
        /// A test case status event.
        /// </summary>
        /// <param name="testCase">The test case that this is the status for.</param>
        public delegate void TestCaseRun(TestCase testCase);
        /// <summary>
        /// Fired on a passed test case.
        /// </summary>
        public event TestCaseRun TestCaseDidPass;
        /// <summary>
        /// Fired on a failed test case.
        /// </summary>
        public event TestCaseRun TestCaseDidFail;
        /// <summary>
        /// Fired on an error during a test case.
        /// </summary>
        public event TestCaseRun TestCaseEncounteredError;

        /// <summary>
        /// Runs tests on the files.
        /// </summary>
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
