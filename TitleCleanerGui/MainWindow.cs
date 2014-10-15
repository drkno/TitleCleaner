using System;
using System.Windows.Forms;
using MediaFileParser.MediaTypes.MovieFile;
using MediaFileParser.MediaTypes.TvFile;
using MediaFileParser.ModeManagers;

namespace TitleCleanerGui
{
    public partial class MainWindow : Form
    {
        private bool _confirm = false;
        private string _inputDir;
        private string _outputDir;
        private Type _type;
        private int _mode;
        private FileManager fileManager;

        public MainWindow()
        {
            InitializeComponent();

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

            toolStripMenuItem1.Text = _mode != 2 ? "Move/Rename" : "Test";

            

            if (_mode != 2)
            {
                fileManager = new FileManager(_confirm);
                fileManager.ConfirmAutomaticMove += fileManager_ConfirmAutomaticMove;
                fileManager.OnFileMove += fileManager_OnFileMove;
                fileManager.OnFileMoveFailed += fileManager_OnFileMoveFailed;

                
            }
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

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var settings = new Settings();
            if (settings.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            _confirm = settings.GetConfirm();
            _inputDir = settings.GetInput();
            _outputDir = settings.GetOuput();
            _type = settings.GetMediaType();
            _mode = settings.GetMode();

            listView1.Items.Clear();
        }

        private void checkAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.Checked = true;
            }
        }

        private void checkNoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.Checked = false;
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
