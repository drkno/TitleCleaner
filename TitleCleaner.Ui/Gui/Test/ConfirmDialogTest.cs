using System;
using System.Windows.Forms;

namespace TitleCleaner.Ui.Test
{
    public partial class ConfirmDialogTest : Form
    {
        private ConfirmDialogTest(string orig, string renam)
        {
            InitializeComponent();
            labelOrigional.Text = orig;
            labelResult.Text = renam;
        }

        public static DialogResult ShowDialog(string orig, string renam)
        {
            var confirmDlg = new ConfirmDialogTest(orig, renam);
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
