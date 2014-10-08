using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TitleCleaner.Ui.File;

namespace TitleCleaner.Ui
{
    public partial class GuiTest : Form
    {
        public GuiTest()
        {
            InitializeComponent();
            for (var i = 0; i < 30; i++)
            {
                var item = new TitleItem((uint)i, "Origional.avi", "Renamed.avi", "TvFile");
                flowLayoutPanel.Controls.Add(item);
                item.Show();
                item.SetSuccess(i%2==0);
            }
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ToolStripOpenClick(object sender, EventArgs e)
        {
            

            var openFileDialog = new FolderBrowserDialogEx();
            /*openFileDialog.CheckFileExists = false;
            openFileDialog.ValidateNames = false;
            openFileDialog.CheckPathExists = true;
            openFileDialog.FileName = "Folder Selection.";*/
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
               // openFileDialog.
            }
        }

        private void ExitToolStripMenuItemClick(object sender, EventArgs e)
        {
            Close();
            Environment.Exit(0);
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConfirmDialog.ShowDialog("test1.avi", "test2.avi");
        }
    }
}
