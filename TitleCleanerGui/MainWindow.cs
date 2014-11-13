#region

using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using MediaFileParser.MediaTypes.MediaFile;
using MediaFileParser.MediaTypes.TvFile;
using MediaFileParser.ModeManagers;
using TvdbSeriesB = MediaFileParser.MediaTypes.TvFile.Tvdb.TvdbSeries;

#endregion

namespace TitleCleanerGui
{
    public partial class MainWindow : Form
    {
        private bool _confirm;
        private uint _fail;
        private FileManager _fileManager;
        private bool _initialRun = true;
        private string _inputDir;
        private MediaFile[] _mediaFiles;
        private int _mode;
        private string _outputDir;
        private uint _pass;
        private TestManager _testManager;
        private Type _type;

        public MainWindow()
        {
            InitializeComponent();
            TvFile.TvdbSearchSelectionRequired += TvFileTvdbSearchSelectionRequired;
        }

        private static uint TvFileTvdbSearchSelectionRequired(TvdbSeriesB[] seriesSearch, string seriesName)
        {
            var dialog = new TvdbSelector(ref seriesSearch, ref seriesName);
            dialog.ShowDialog();
            return dialog.GetResult();
        }

        private void RefreshView()
        {
            var settings = new Settings();
            if (settings.ShowDialog() != DialogResult.OK)
            {
                if (string.IsNullOrWhiteSpace(_inputDir))
                {
                    Environment.Exit(-1);
                }
                return;
            }

            _confirm = settings.GetConfirm();
            _inputDir = settings.GetInput();
            _outputDir = settings.GetOuput();
            _type = settings.GetMediaType();
            _mode = settings.GetMode();

            if (_mode != 2)
            {
                statusLabel.Text = "";
                toolStripMenuItemPerform.Enabled = true;
                _fileManager = new FileManager(_confirm);
                _fileManager.ConfirmAutomaticMove += FileManagerConfirmAutomaticMove;
                _fileManager.OnFileMove += fileManager_OnFileMove;
                _fileManager.OnFileMoveFailed += fileManager_OnFileMoveFailed;

                _mediaFiles = _fileManager.GetMediaFileList(_inputDir, _type);
                foreach (var mediaFile in _mediaFiles)
                {
                    var listViewItem = new ListViewItem();
                    listViewItem.SubItems.Add(mediaFile.ToString("O.E"));
                    listViewItem.SubItems.Add(mediaFile.ToString(mediaFile.ToString()));
                    listViewMediaFiles.Items.Add(listViewItem);
                }
            }
            else
            {
                toolStripMenuItemPerform.Enabled = false;
                _testManager = new TestManager(_inputDir, _type);
                _testManager.TestCaseDidFail += TestManagerTestCaseDidFail;
                _testManager.TestCaseDidPass += TestManagerTestCaseDidPass;
                _testManager.TestCaseEncounteredError += TestManagerTestCaseEncounteredError;
                var thread = new Thread(_testManager.RunTests);
                thread.Start();
            }
        }

        private void TestManagerTestCaseEncounteredError(TestManager.TestCase testCase)
        {
            var listViewItem = new ListViewItem {Checked = false, BackColor = Color.Yellow, Text = "Error"};
            listViewItem.SubItems.Add(testCase.OrigionalName);
            listViewItem.SubItems.Add(testCase.MediaFile.ToString());
            Invoke(new MethodInvoker(() => listViewMediaFiles.Items.Add(listViewItem)));
            _fail++;
            statusLabel.Text = _pass + " Passed, " + _fail + " Failed." +
                               ((_pass + _fail == _testManager.Count) ? " Done." : "");
        }

        private void TestManagerTestCaseDidPass(TestManager.TestCase testCase)
        {
            var listViewItem = new ListViewItem {Checked = true, BackColor = Color.LawnGreen, Text = "Pass"};
            listViewItem.SubItems.Add(testCase.OrigionalName);
            listViewItem.SubItems.Add(testCase.MediaFile.ToString());
            Invoke(new MethodInvoker(() => listViewMediaFiles.Items.Add(listViewItem)));
            _pass++;
            statusLabel.Text = _pass + " Passed, " + _fail + " Failed." +
                               ((_pass + _fail == _testManager.Count) ? " Done." : "");
        }

        private void TestManagerTestCaseDidFail(TestManager.TestCase testCase)
        {
            var listViewItem = new ListViewItem {Checked = false, BackColor = Color.Red, Text = "Fail"};
            listViewItem.SubItems.Add(testCase.OrigionalName);
            listViewItem.SubItems.Add(testCase.MediaFile.ToString());
            Invoke(new MethodInvoker(() => listViewMediaFiles.Items.Add(listViewItem)));
            _fail++;
            statusLabel.Text = _pass + " Passed, " + _fail + " Failed." +
                               ((_pass + _fail == _testManager.Count) ? " Done." : "");
        }

        private void fileManager_OnFileMoveFailed(MediaFile file, string destination)
        {
            listViewMediaFiles.CheckedItems[(int) (_fail + _pass)].BackColor = Color.Red;
            _fail++;
            statusLabel.Text = _pass + " Moved/Renamed, " + _fail + " Failed." +
                               ((_pass + _fail == _testManager.Count) ? " Done." : "");
        }

        private void fileManager_OnFileMove(MediaFile file, string destination)
        {
            listViewMediaFiles.CheckedItems[(int) (_fail + _pass)].BackColor = Color.LawnGreen;
            _pass++;
            statusLabel.Text = _pass + " Moved/Renamed, " + _fail + " Failed." +
                               ((_pass + _fail == _testManager.Count) ? " Done." : "");
        }

        private static bool FileManagerConfirmAutomaticMove(MediaFile file, string destination)
        {
            var result = MessageBox.Show(
                "Do you want to move\n" + file.ToString("O.E") + "\nTo\n" + file + "\nIn location " + destination + "?",
                "Title Cleaner", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return result == DialogResult.Yes;
        }

        private void ExitMenuItemClick(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void OptionsToolStripMenuItemClick(object sender, EventArgs e)
        {
            _pass = 0;
            _fail = 0;
            listViewMediaFiles.Items.Clear();
            RefreshView();
        }

        private void CheckAllToolStripMenuItemClick(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listViewMediaFiles.Items)
            {
                item.Checked = true;
            }
        }

        private void CheckNoneToolStripMenuItemClick(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listViewMediaFiles.Items)
            {
                item.Checked = false;
            }
        }

        private void ToolStripMenuItemPerformClick(object sender, EventArgs e)
        {
            foreach (int i in listViewMediaFiles.CheckedIndices)
            {
                _fileManager.MoveFile(_mediaFiles[i], _outputDir);
            }
        }

        private void MainWindowPaint(object sender, PaintEventArgs e)
        {
            if (!_initialRun) return;
            _initialRun = false;
            RefreshView();
        }

        private void CheckSelectionToolStripMenuItemClick(object sender, EventArgs e)
        {
            foreach (var selectedItem in listViewMediaFiles.SelectedItems)
            {
                ((ListViewItem) selectedItem).Checked = true;
            }
        }

        private void UncheckSelectionToolStripMenuItemClick(object sender, EventArgs e)
        {
            foreach (var selectedItem in listViewMediaFiles.SelectedItems)
            {
                ((ListViewItem) selectedItem).Checked = false;
            }
        }
    }
}