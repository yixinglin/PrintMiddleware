using System.Windows.Forms;

namespace PrintMiddleware.Utils
{
    public static class TrayNotifier
    {
        private static NotifyIcon _notifyIcon;

        public static void Init(NotifyIcon icon)
        {
            _notifyIcon = icon;
        }

        public static void Show(string title, string message, ToolTipIcon icon = ToolTipIcon.Info)
        {
            _notifyIcon?.ShowBalloonTip(4000, title, message, icon);
        }
    }
}
