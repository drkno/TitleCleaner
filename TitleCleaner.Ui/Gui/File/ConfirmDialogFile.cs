using System;
using System.Windows.Forms;

namespace TitleCleaner.Ui.File
{
    public partial class ConfirmDialog : Form
    {
        private ConfirmDialog(string orig, string renam)
        {
            InitializeComponent();
            labelOrigional.Text = orig;
            labelResult.Text = renam;
        }

        public static DialogResult ShowDialog(string orig, string renam)
        {
            var confirmDlg = new ConfirmDialog(orig, renam);
            confirmDlg.ShowDialog();
            return confirmDlg.Result ? DialogResult.OK : DialogResult.No;
        }

        public bool Result { get; protected set; }

        private void ButtonNoClick(object sender, EventArgs e)
        {
            Close();
            Result = false;
        }

        private void ButtonYesClick(object sender, EventArgs e)
        {
            Close();
            Result = true;
        }
    }
}
