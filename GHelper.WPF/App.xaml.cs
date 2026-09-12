using System.Windows;
using GHelper.Helpers;

namespace GHelper.WPF
{
    /// <summary>
    /// Tray-resident application shell. The process lives in the notification area: the
    /// main window is only ever shown or hidden, and never the thing that keeps it alive.
    /// </summary>
    public partial class App : Application
    {
        private SingleInstance? _singleInstance;
        private TrayIcon? _trayIcon;
        private MainWindow? _window;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _singleInstance = SingleInstance.Claim(() => Dispatcher.BeginInvoke(ShowWindow));
            if (_singleInstance is null)
            {
                Logger.WriteLine("GHelper.WPF is already running; handed over to the existing instance");
                Shutdown();
                return;
            }

            Logger.WriteLine($"GHelper.WPF started. Log: {Logger.logFile}. Running as SYSTEM: {UserIdentity.IsRunningAsSystem()}");

            _window = new MainWindow();

            _trayIcon = new TrayIcon();
            _trayIcon.ToggleRequested += (_, _) => ToggleWindow();
            _trayIcon.OpenRequested += (_, _) => ShowWindow();
            _trayIcon.ExitRequested += (_, _) => Shutdown();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // A duplicate instance never took ownership, so it has nothing to tear down.
            if (_singleInstance is not null)
            {
                _trayIcon?.Dispose();
                _singleInstance.Dispose();
                Logger.WriteLine("GHelper.WPF exited");
            }

            base.OnExit(e);
        }

        private void ShowWindow()
        {
            if (_window is null) return;

            _window.Show();
            if (_window.WindowState == WindowState.Minimized) _window.WindowState = WindowState.Normal;
            _window.Activate();
        }

        private void ToggleWindow()
        {
            if (_window?.IsVisible == true)
            {
                _window.Hide();
            }
            else
            {
                ShowWindow();
            }
        }
    }
}
