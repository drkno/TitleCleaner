#region

using System.Windows.Forms;

#endregion

namespace TitleCleanerGui
{
    public class ListViewNoFlicker : ListView
    {
        public ListViewNoFlicker()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.EnableNotifyMessage, true);
        }

        protected override void OnNotifyMessage(Message m)
        {
            if (m.Msg != 0x14)
            {
                base.OnNotifyMessage(m);
            }
        }
    }
}