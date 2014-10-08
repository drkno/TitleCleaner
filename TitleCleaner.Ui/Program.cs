using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TitleCleaner.Ui
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var parser = new CommandParser("Fix Scene Release Titles");

            parser.Argument("u", "ui", "The UI type to tun the program as.", (commandParser, s) =>
                                                                             {
                                                                                 MessageBox.Show(s);
                                                                             });



            parser.Parse();











            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GuiFile());
        }
    }
}
