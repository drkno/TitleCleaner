using System;
using System.Windows.Forms;
using MediaFileParser.MediaTypes.MovieFile;
using MediaFileParser.MediaTypes.TvFile;
using MediaFileParser.ModeManagers;

namespace TitleCleanerGui
{
    public partial class MainWindow : Form
    {
        private static FileManager _fileManager;

        public MainWindow()
        {
            InitializeComponent();
            _fileManager = new FileManager(false);
        }

        private void ExitMenuItemClick(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void DirectoryMenuItemClick(object sender, EventArgs e)
        {
            var directoryDialog = new FolderBrowserDialog
                                  {
                                      Description = "Select directory to scan for files.",
                                      ShowNewFolderButton = false
                                  };
            if (directoryDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            AddMediaFiles(directoryDialog.SelectedPath);
        }

        private void FileToolStripMenuItem1Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog
                                 {
                                     CheckPathExists = true,
                                     CheckFileExists = true,
                                     AutoUpgradeEnabled = true,
                                     Multiselect = false,
                                     Title = "Select a media file to clean.",
                                     Filter =
                                         "*.mov|*.mov|*.mkv|*.mkv|*.flv|*.flv|*.avi|*.avi|*.mp4|*.mp4|*.mpg|*.mpg|*.vob|*.vob|*.m4v|*.m4v|*.mpeg|*.mpeg|*.ogg|*.ogg|*.swf|*.swf|*.wmv|*.wmv|*.wtv|*.wtv|*.h264|*.h264"
                                 };

            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            AddMediaFiles(openFileDialog.FileName);
        }

        private Type GetMediaType()
        {
            if (tvMenuItem.Checked) { return typeof (TvFile); }
            if (movieMenuItem.Checked) { return typeof(MovieFile); }
            return null;
        }

        private void AddMediaFiles(string directory)
        {
            foreach (var file in _fileManager.GetMediaFileList(directory, GetMediaType()))
            {
                var item = new ListViewItem(new[] { "", file.ToString("O.E"), file.ToString("C.E") });
                listView1.Items.Add(item);
            }
            
        }

        private void TestMenuItemClick(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog
                                 {
                                     CheckPathExists = true,
                                     CheckFileExists = true,
                                     AutoUpgradeEnabled = true,
                                     Multiselect = false,
                                     Title = "Select a test file.",
                                     Filter = "*.csv|*.csv"
                                 };

            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }


        }

        private void TypeMenuItemClick(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem ctrl in typeMenuItem.DropDownItems)
            {
                ctrl.Checked = false;
            }
            ((ToolStripMenuItem)sender).Checked = true;
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings settings = new Settings();
            settings.ShowDialog();
        }
    }
}
