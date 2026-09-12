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

            Logger.WriteLine("GHelper.WPF shell started");

            AssemblyName shared = typeof(UserIdentity).Assembly.GetName();
            SharedAssemblyText.Text = $"Shared assembly: {shared.Name} {shared.Version}";
            LogPathText.Text = $"Shared log file: {Logger.logFile}";
            RunningAsSystemText.Text = $"Running as SYSTEM: {UserIdentity.IsRunningAsSystem()}";
            GpuStatusText.Text = $"GPU device status: {DeviceHelper.GetGpuError() ?? "no problem reported"}";
        }
    }
}
