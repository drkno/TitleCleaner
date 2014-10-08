using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using MediaFileParser.MediaTypes.MediaFile;

namespace TitleCleaner.Ui
{
    public partial class TitleItem : UserControl
    {
        public bool Checked
        {
            get { return checkBox.Checked; }
            set { checkBox.Checked = value; }
        }

        public uint Index { get; set; }

        public TitleItem(uint index, string origional, string renamed, string type)
        {
            InitializeComponent();
            Index = index;
            labelIndex.Text = index.ToString(CultureInfo.InvariantCulture);
            labelOrigional.Text = origional;
            labelRenamed.Text = renamed;
            labelType.Text = type;
        }

        public TitleItem(uint index, MediaFile mediaFile)
        {
            InitializeComponent();
            Index = index;
            labelIndex.Text = index.ToString(CultureInfo.InvariantCulture);
            labelOrigional.Text = mediaFile.Origional;
            labelRenamed.Text = mediaFile.Cleaned;
            labelType.Text = mediaFile.GetType().ToString();
        }

        public void SetSuccess(bool success)
        {
            checkBox.Enabled = false;
            var color = success ? Color.PaleGreen : Color.PaleVioletRed;
            labelIndex.BackColor = color;
            labelType.BackColor = color;
        }
    }
}
