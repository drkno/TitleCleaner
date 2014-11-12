using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using MediaFileParser.MediaTypes.MediaFile;
using MediaFileParser.MediaTypes.TvFile;
using MediaFileParser.MediaTypes.TvFile.Tvdb;
using MediaFileParser.ModeManagers;

namespace TitleCleanerGui
{
    public partial class MainWindow : Form
    {
        private bool _confirm;
        private string _inputDir;
        private string _outputDir;
        private Type _type;
        private int _mode;
        private FileManager _fileManager;
        private TestManager _testManager;
        private MediaFile[] _mediaFiles;
        private uint _pass, _fail;

        public MainWindow()
        {
            InitializeComponent();
            TvFile.TvdbSearchSelectionRequired += TvFileTvdbSearchSelectionRequired;
        }

        private static uint TvFileTvdbSearchSelectionRequired(TvdbSeries[] seriesSearch, string seriesName)
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
                Environment.Exit(-1);
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
                _fileManager.ConfirmAutomaticMove += fileManager_ConfirmAutomaticMove;
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
                _testManager.TestCaseDidFail += _testManager_TestCaseDidFail;
                _testManager.TestCaseDidPass += _testManager_TestCaseDidPass;
                _testManager.TestCaseEncounteredError += _testManager_TestCaseEncounteredError;
                var thread = new Thread(_testManager.RunTests);
                thread.Start();
            }
        }

        void _testManager_TestCaseEncounteredError(TestManager.TestCase testCase)
        {
            var listViewItem = new ListViewItem { Checked = false, BackColor = Color.Yellow, Text = "Erro" };
            listViewItem.SubItems.Add(testCase.OrigionalName);
            listViewItem.SubItems.Add(testCase.MediaFile.ToString());
            Invoke(new MethodInvoker(() => listViewMediaFiles.Items.Add(listViewItem)));
            _fail++;
            if ((_fail + _pass) % 500 != 0 && (_fail + _pass) != _testManager.Count) return;
            statusLabel.Text = _pass + " Passed, " + _fail + " Failed.";
        }

        void _testManager_TestCaseDidPass(TestManager.TestCase testCase)
        {
            var listViewItem = new ListViewItem { Checked = true, BackColor = Color.LawnGreen, Text = "Pass" };
            listViewItem.SubItems.Add(testCase.OrigionalName);
            listViewItem.SubItems.Add(testCase.MediaFile.ToString());
            Invoke(new MethodInvoker(() => listViewMediaFiles.Items.Add(listViewItem)));
            _pass++;
            if ((_fail + _pass)%500 != 0 && (_fail + _pass) != _testManager.Count) return;
            statusLabel.Text = _pass + " Passed, " + _fail + " Failed.";
        }

        void _testManager_TestCaseDidFail(TestManager.TestCase testCase)
        {
            var listViewItem = new ListViewItem { Checked = false, BackColor = Color.Red, Text = "Fail" };
            listViewItem.SubItems.Add(testCase.OrigionalName);
            listViewItem.SubItems.Add(testCase.MediaFile.ToString());
            Invoke(new MethodInvoker(() => listViewMediaFiles.Items.Add(listViewItem)));
            _fail++;
            if ((_fail + _pass)%500 != 0 && (_fail + _pass) != _testManager.Count) return;
            statusLabel.Text = _pass + " Passed, " + _fail + " Failed.";
        }

        void fileManager_OnFileMoveFailed(MediaFileParser.MediaTypes.MediaFile.MediaFile file, string destination)
        {
            throw new NotImplementedException();
        }

        void fileManager_OnFileMove(MediaFileParser.MediaTypes.MediaFile.MediaFile file, string destination)
        {
                throw new NotImplementedException();
        }

        bool fileManager_ConfirmAutomaticMove(MediaFileParser.MediaTypes.MediaFile.MediaFile file, string destination)
        {
            throw new NotImplementedException();
        }

        private void ExitMenuItemClick(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void OptionsToolStripMenuItemClick(object sender, EventArgs e)
        {
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
            if (_mode != 2)
            {
                foreach (int i in listViewMediaFiles.CheckedIndices)
                {
                    _fileManager.MoveFile(_mediaFiles[i], _outputDir);
                }
            }
            else
            {
                for (int i = 0; i < _testManager.Count; i++)
                {
                    
                }
            }
        }

        private bool _initialRun = true;
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
                ((ListViewItem)selectedItem).Checked = false;
            }
        }
    }
}
