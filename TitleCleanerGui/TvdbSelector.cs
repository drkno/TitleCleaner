using System;
using System.Windows.Forms;
using MediaFileParser.MediaTypes.TvFile.Tvdb;

namespace TitleCleanerGui
{
    public partial class TvdbSelector : Form
    {
        private TvdbSeries[] _seriesSearch;

        public TvdbSelector()
        {
            InitializeComponent();
        }

        public TvdbSelector(ref TvdbSeries[] seriesSearch, ref string seriesName)
        {
            _seriesSearch = seriesSearch;
            Text = "Which does " + seriesName + " refer to?";
        }

        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        public uint GetResult()
        {
            throw new NotImplementedException();
        }
    }
}
