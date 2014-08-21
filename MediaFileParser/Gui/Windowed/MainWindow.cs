using System.Windows.Forms;
using MediaFileParser.MediaTypes.TvFile;

namespace MediaFileParser.Gui.Windowed
{
    public partial class MainWindow : Form, IGui
    {
        public MainWindow()
        {
            InitializeComponent();
            TvFile.TvdbSearchSelectionRequired += TvFile_TvdbSearchSelectionRequired;
        }

        private uint TvFile_TvdbSearchSelectionRequired(MediaTypes.TvFile.Tvdb.TvdbSeries[] seriesSearch)
        {
            throw new System.NotImplementedException();
        }

        public void Start()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(this);
        }
    }
}
