using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TitleCleanerGui
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();

            var p = new SettingsSet
            {
                { "Test","Some setting.", () => new TextBox(), () => true}
            };

            var i = 0;
            foreach (var setting in p.GetControls())
            {
                tableLayoutPanelSettings.SetRow(setting, i);
                setting.Show();
                i++;
            }
        }
    }

    public class SettingsSet : IEnumerable
    {
        private readonly List<Setting> _setting = new List<Setting>();

        public IEnumerator GetEnumerator()
        {
            return _setting.GetEnumerator();
        }

        public void Add(string name, string desc, Func<Control> setup, Func<bool> setValues)
        {
            _setting.Add(new Setting(name, desc, setup(), setValues));
        }

        public IEnumerable<Setting> GetControls()
        {
            return _setting;
        }
    }
}
