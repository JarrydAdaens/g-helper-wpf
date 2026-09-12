using System.Diagnostics;
using System.Security.AccessControl;
using System.Security.Principal;

namespace GHelper.Helpers
{
    public static class ProcessHelper
    {
        private const string ExitEventName = "Global\\GHelperApp-Exit";
        private static EventWaitHandle? exitEvent;
        private static long lastAdmin;

        // Identity checks moved to GHelper.Shared; kept here so existing call sites are untouched
        // while the rest of ProcessHelper still depends on WinForms.
        public static bool IsRunningAsSystem() => UserIdentity.IsRunningAsSystem();

        public static void CheckAlreadyRunning()
        {
            var sec = new EventWaitHandleSecurity();
            sec.AddAccessRule(new EventWaitHandleAccessRule(
                new SecurityIdentifier(WellKnownSidType.AuthenticatedUserSid, null),
                EventWaitHandleRights.Synchronize | EventWaitHandleRights.Modify,
                AccessControlType.Allow));

            bool created = false;
            try
            {
                exitEvent = EventWaitHandleAcl.Create(false, EventResetMode.ManualReset, ExitEventName, out created, sec);
            }
            catch
            {
                try { exitEvent = EventWaitHandle.OpenExisting(ExitEventName); }
                catch { }
            }

            if (!created && exitEvent != null)
            {
                try
                {
                    exitEvent.Set();
                    exitEvent.Reset();
                }
                catch (Exception ex)
                {
                    Logger.WriteLine("Broadcast exit failed: " + ex.Message);
                    exitEvent = null;
                }
            }

            using Process currentProcess = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(currentProcess.ProcessName);
            try
            {
                if (processes.Length > 1)
                {
                    var failed = new List<Process>();
                    foreach (Process process in processes)
                        if (process.Id != currentProcess.Id)
                        {
                            try
                            {
                                process.Kill();
                            }
                            catch (Exception ex)
                            {
                                Logger.WriteLine($"Can't kill PID {process.Id}: {ex.Message}");
                                failed.Add(process);
                            }
                        }

                    if (failed.Count > 0)
                    {
                        Thread.Sleep(2000);

                        foreach (var p in failed)
                        {
                            bool stillAlive;
                            try { stillAlive = !p.HasExited; }
                            catch { stillAlive = true; }

                            if (stillAlive)
                            {
                                MessageBox.Show(Properties.Strings.AppAlreadyRunningText, Properties.Strings.AppAlreadyRunning, MessageBoxButtons.OK);
                                Application.Exit();
                                return;
                            }
                        }
                    }
                }
            }
            finally
            {
                foreach (Process p in processes) p.Dispose();
            }

            if (exitEvent != null)
                ThreadPool.RegisterWaitForSingleObject(exitEvent, (_, _) => Application.Exit(), null, Timeout.Infinite, true);
        }

        public static bool IsUserAdministrator() => UserIdentity.IsUserAdministrator();

        public static void RunAsAdmin(string? param = null, bool force = false)
        {

            if (Math.Abs(DateTimeOffset.Now.ToUnixTimeMilliseconds() - lastAdmin) < 2000) return;
            lastAdmin = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            // Check if the current user is an administrator
            if (!IsUserAdministrator() || force)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.UseShellExecute = true;
                startInfo.WorkingDirectory = Environment.CurrentDirectory;
                startInfo.FileName = Application.ExecutablePath;
                startInfo.Arguments = param;
                startInfo.Verb = "runas";
                try
                {
                    Process.Start(startInfo);
                    Application.Exit();
                }
                catch (Exception ex)
                {
                    Logger.WriteLine(ex.Message);
                }
            }
        }


        // Process/service utilities moved to GHelper.Shared; kept here as delegating shims
        // so existing call sites are untouched.
        public static void KillByName(string name) => ProcessUtility.KillByName(name);

        public static void KillSmartDisplayControl() => ProcessUtility.KillSmartDisplayControl();

        public static void KillByProcess(Process process) => ProcessUtility.KillByProcess(process);

        public static void StopDisableService(string serviceName, string disable = "Disabled") => ProcessUtility.StopDisableService(serviceName, disable);

        public static void StartEnableService(string serviceName, bool automatic = true) => ProcessUtility.StartEnableService(serviceName, automatic);

        public static string RunCMD(string name, string args, string? directory = null, int timeoutMs = 0) => ProcessUtility.RunCMD(name, args, directory, timeoutMs);

        public static void SetPriority(ProcessPriorityClass priorityClass = ProcessPriorityClass.Normal) => ProcessUtility.SetPriority(priorityClass);
    }
}
