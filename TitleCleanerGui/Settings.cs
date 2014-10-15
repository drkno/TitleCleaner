using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MediaFileParser.MediaTypes.MediaFile;
using MediaFileParser.MediaTypes.MovieFile;
using MediaFileParser.MediaTypes.TvFile;

namespace TitleCleanerGui
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
            comboBoxMode.SelectedIndex = 0;
            comboBoxFileType.SelectedIndex = 0;
            textBoxCommonName.Text = MediaFile.DefaultFormatString;
            textBoxTvName.Text = TvFile.DefaultFormatString;
            textBoxMovieName.Text = MovieFile.DefaultFormatString;
        }

        private void ButtonDirectoryClick(object sender, EventArgs e)
        {
            var res = "None";
            switch (comboBoxMode.SelectedIndex)
            {
                case 0:
                case 1:
                {
                    var directoryDialog = new FolderBrowserDialog
                    {
                        Description = "Select directory to output files to.",
                        ShowNewFolderButton = false
                    };

                    if (directoryDialog.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    res = directoryDialog.SelectedPath;
                    break;
                }
                /*case 1:
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

                    res = openFileDialog.FileName;
                    break;
                }*/
                case 2:
                {
                    var openFileDialog = new SaveFileDialog
                    {
                        CheckPathExists = true,
                        CheckFileExists = true,
                        AutoUpgradeEnabled = true,
                        Title = "Select output test file.",
                        Filter = "*.csv|*.csv"
                    };

                    if (openFileDialog.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    res = openFileDialog.FileName;
                    break;
                }
            }
            textBoxOutputDir.Text = res;
        }

        private void textBoxCommonName_TextChanged(object sender, EventArgs e)
        {
            MediaFile.DefaultFormatString = textBoxCommonName.Text;
            textBoxTvName.Text = TvFile.DefaultFormatString;
            textBoxMovieName.Text = MovieFile.DefaultFormatString;
        }

        private void textBoxTvName_TextChanged(object sender, EventArgs e)
        {
            if (MediaFile.DefaultFormatString != textBoxTvName.Text)
            {
                TvFile.DefaultFormatString = textBoxTvName.Text;
            }
        }

        private void textBoxMovieName_TextChanged(object sender, EventArgs e)
        {
            if (MediaFile.DefaultFormatString != textBoxTvName.Text)
            {
                MovieFile.DefaultFormatString = textBoxMovieName.Text;
            }
        }

        private void comboBoxMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxMode.SelectedIndex)
            {
                case 0:
                {
                    labelInputDir.Text = "Input Directory";
                    labelInputDirDesc.Text = "Directory to read files from.";
                    labelOutput.Text = "Output Directory";
                    labelDirFiles.Text = "Directory to move files to.";
                    break;
                }
                case 1:
                {
                    labelInputDir.Text = "Input File";
                    labelInputDirDesc.Text = "File to clean the name of.";
                    labelOutput.Text = "Output Directory";
                    labelDirFiles.Text = "Directory to move files to.";
                    break;
                }
                case 2:
                {
                    labelInputDir.Text = "Tests File";
                    labelInputDirDesc.Text = "File containing the tests to be run.";
                    labelOutput.Text = "Test CSV Output";
                    labelDirFiles.Text = "Location to output a CSV file summarising the tests.";
                    break;
                }
            }
            textBoxInputDir.Text = "Current Directory";
            textBoxOutputDir.Text = "None";
        }

        private void buttonInputDir_Click(object sender, EventArgs e)
        {
            var res = "Current Directory";
            switch (comboBoxMode.SelectedIndex)
            {
                case 0:
                    {
                        var directoryDialog = new FolderBrowserDialog
                        {
                            Description = "Select directory to clean files in.",
                            ShowNewFolderButton = false
                        };

                        if (directoryDialog.ShowDialog() != DialogResult.OK)
                        {
                            return;
                        }

                        res = directoryDialog.SelectedPath;
                        break;
                    }
                case 1:
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

                    res = openFileDialog.FileName;
                    break;
                }
                case 2:
                    {
                        var openFileDialog = new OpenFileDialog
                        {
                            CheckPathExists = true,
                            CheckFileExists = true,
                            AutoUpgradeEnabled = true,
                            Multiselect = false,
                            Title = "Select test file.",
                            Filter = "*.csv|*.csv"
                        };

                        if (openFileDialog.ShowDialog() != DialogResult.OK)
                        {
                            return;
                        }

                        res = openFileDialog.FileName;
                        break;
                    }
            }
            textBoxInputDir.Text = res;
        }
    }
}
