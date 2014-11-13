#region

using System;
using System.Windows.Forms;
using TvdbSeriesB = MediaFileParser.MediaTypes.TvFile.Tvdb.TvdbSeries;

#endregion

namespace TitleCleanerGui
{
    public partial class TvdbSelector : Form
    {
        private uint _selection;

        public TvdbSelector(ref TvdbSeriesB[] seriesSearch, ref string seriesName)
        {
            InitializeComponent();
            Text = "Which does " + seriesName + " refer to?";

            for (var i = 0; i < seriesSearch.Length; i++)
            {
                var seriesControl = new TvdbSeries(seriesSearch[i]);
                seriesControl.SeriesSelected += SeriesSelected;
                tableLayoutPanelSeriesSelection.Controls.Add(seriesControl, 0, i);
                seriesControl.Show();
            }
        }

        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private void SeriesSelected(object sender, EventArgs eventArgs)
        {
            var series = sender as TvdbSeriesB;
            if (series != null)
            {
                _selection = series.Id;
            }
            Close();
        }

        public uint GetResult()
        {
            return _selection;
        }

        private void ButtonNoneClick(object sender, EventArgs e)
        {
            SeriesSelected(null, null);
        }
    }
}