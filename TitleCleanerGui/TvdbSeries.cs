#region

using System;
using System.Windows.Forms;

#endregion

namespace TitleCleanerGui
{
    public partial class TvdbSeries : UserControl
    {
        private readonly MediaFileParser.MediaTypes.TvFile.Tvdb.TvdbSeries _series;

        public TvdbSeries(MediaFileParser.MediaTypes.TvFile.Tvdb.TvdbSeries series)
        {
            _series = series;
            InitializeComponent();
            labelTitle.Text = _series.SeriesName;
            richTextBoxDescription.Text = _series.Overview;
        }

        public EventHandler SeriesSelected { get; set; }

        private void ButtonSelectClick(object sender, EventArgs e)
        {
            if (SeriesSelected != null)
            {
                SeriesSelected(_series, null);
            }
        }

        private void RichTextBoxDescriptionEnter(object sender, EventArgs e)
        {
            buttonSelect.Focus();
        }
    }
}