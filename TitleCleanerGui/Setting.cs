using System;
using System.Windows.Forms;

namespace TitleCleanerGui
{
    public partial class Setting : UserControl
    {
        private readonly Func<bool> _setValues;
        public Setting(string name, string desc, Control ctrl, Func<bool> setValues)
        {
            InitializeComponent();
            labelSettingName.Text = name;
            labelDesc.Text = desc;
            tableLayoutPanel1.SetColumn(ctrl, 0);
            ctrl.Show();
            _setValues = setValues;
        }

        public bool SetValues()
        {
            return _setValues.Invoke();
        }
    }
}
