using System.ComponentModel;
using System.Reflection;
using System.Windows;
using GHelper.Helpers;

namespace GHelper.WPF
{
    /// <summary>
    /// Placeholder shell window. Its only job right now is to show that the shared
    /// layer resolves and executes from the WPF process; real UI arrives in Milestone 3.
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            AssemblyName shared = typeof(UserIdentity).Assembly.GetName();
            SharedAssemblyText.Text = $"Shared assembly: {shared.Name} {shared.Version}";
            LogPathText.Text = $"Shared log file: {Logger.logFile}";
            RunningAsSystemText.Text = $"Running as SYSTEM: {UserIdentity.IsRunningAsSystem()}";
            GpuStatusText.Text = $"GPU device status: {DeviceHelper.GetGpuError() ?? "no problem reported"}";
        }

        /// <summary>
        /// Closing the window hides it instead of ending the process; the application is
        /// tray-resident and only quits from the tray menu.
        /// </summary>
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
            base.OnClosing(e);
        }
    }
}
