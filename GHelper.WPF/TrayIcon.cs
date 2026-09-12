using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace GHelper.WPF
{
    /// <summary>
    /// The application's notification-area presence. It wraps the shell tray icon and
    /// raises intent events; it owns no application state and decides nothing about the
    /// window it stands for.
    /// </summary>
    internal sealed class TrayIcon : IDisposable
    {
        private const string IconResourceUri = "pack://application:,,,/Resources/standard.ico";

        private readonly NotifyIcon _notifyIcon;

        public TrayIcon()
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            menu.Items.Add("Open G-Helper", null, (_, _) => OpenRequested?.Invoke(this, EventArgs.Empty));
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add("Exit", null, (_, _) => ExitRequested?.Invoke(this, EventArgs.Empty));

            _notifyIcon = new NotifyIcon
            {
                Text = "G-Helper WPF",
                Icon = LoadIcon(),
                ContextMenuStrip = menu,
                Visible = true
            };

            _notifyIcon.MouseClick += OnMouseClick;
        }

        /// <summary>Raised when the user explicitly asks for the window, from the context menu.</summary>
        public event EventHandler? OpenRequested;

        /// <summary>Raised on a left click, which flips the window between shown and hidden.</summary>
        public event EventHandler? ToggleRequested;

        /// <summary>Raised when the user asks to quit the application.</summary>
        public event EventHandler? ExitRequested;

        public void Dispose()
        {
            _notifyIcon.Visible = false;
            _notifyIcon.ContextMenuStrip?.Dispose();
            _notifyIcon.Icon?.Dispose();
            _notifyIcon.Dispose();
        }

        /// <summary>
        /// Loads the tray artwork at the shell's current small-icon size, so the icon stays
        /// crisp instead of being scaled down from the largest frame in the file.
        /// </summary>
        private static Icon LoadIcon()
        {
            using Stream stream = System.Windows.Application.GetResourceStream(new Uri(IconResourceUri)).Stream;
            return new Icon(stream, SystemInformation.SmallIconSize);
        }

        private void OnMouseClick(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ToggleRequested?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
