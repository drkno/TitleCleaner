using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using MediaFileParser.MediaTypes.MediaFile;

namespace TitleCleaner.Ui.Test
{
    public partial class TestItem : UserControl
    {

        public uint Index { get; set; }

        public TestItem(uint index, string origional, string renamed, string type, bool typePass)
        {
            InitializeComponent();
            Index = index;
            labelIndex.Text = index.ToString(CultureInfo.InvariantCulture);
            labelOrigional.Text = origional;
            labelRenamed.Text = renamed;
            labelType.Text = type;
            labelTypePass.Text = typePass.ToString();
        }

        public TestItem(uint index, MediaFile mediaFile)
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
            var color = success ? Color.PaleGreen : Color.PaleVioletRed;
            labelIndex.BackColor = color;
            labelType.BackColor = color;
            labelTypePass.BackColor = color;
        }
    }
}
