using System.Diagnostics;

namespace GHelper.Helpers
{
    /// <summary>
    /// Process and service utilities that are independent of any UI framework.
    /// The WinForms-specific members of the original ProcessHelper (CheckAlreadyRunning,
    /// RunAsAdmin) stay in the WinForms head, which delegates the rest here.
    /// </summary>
    public static class ProcessUtility
    {
        public static void KillByName(string name)
        {
            var processes = Process.GetProcessesByName(name);
            try
            {
                foreach (var process in processes)
                {
                    try
                    {
                        process.Kill();
                        Logger.WriteLine($"Stopped: {process.ProcessName}");
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLine($"Failed to stop: {process.ProcessName} {ex.Message}");
                    }
                }
            }
            finally
            {
                foreach (var p in processes) p.Dispose();
            }
        }

        public static void KillSmartDisplayControl()
        {
            KillByName("ASUSSmartDisplayControl");
        }

        public static void KillByProcess(Process process)
        {
            try
            {
                process.Kill();
                Logger.WriteLine($"Stopped: {process.ProcessName}");
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"Failed to stop: {process.ProcessName} {ex.Message}");
            }
        }

        public static void StopDisableService(string serviceName, string disable = "Disabled")
        {
            try
            {
                string script = $"Get-Service -Name \"{serviceName}\" | Stop-Service -Force -PassThru | Set-Service -StartupType {disable}";
                Logger.WriteLine(script);
                RunCMD("powershell", script);
            }
            catch (Exception ex)
            {
                Logger.WriteLine(ex.ToString());
            }
        }

        public static void StartEnableService(string serviceName, bool automatic = true)
        {
            try
            {
                string script = $"Set-Service -Name \"{serviceName}\" -Status running" + (automatic ? " -StartupType Automatic" : "");
                Logger.WriteLine(script);
                RunCMD("powershell", script);
            }
            catch (Exception ex)
            {
                Logger.WriteLine(ex.ToString());
            }
        }

        public static string RunCMD(string name, string args, string? directory = null, int timeoutMs = 0)
        {
            using var cmd = new Process();
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            cmd.StartInfo.FileName = (name == "powershell") ? Path.Combine(Environment.SystemDirectory, @"WindowsPowerShell\v1.0\powershell.exe") : name;
            cmd.StartInfo.Arguments = (name == "powershell") ? "-NoProfile " + args : args;
            if (directory != null) cmd.StartInfo.WorkingDirectory = directory;
            cmd.Start();

            var watch = Stopwatch.StartNew();
            string result;

            if (timeoutMs > 0)
            {
                var readTask = cmd.StandardOutput.ReadToEndAsync();
                if (!readTask.Wait(timeoutMs))
                {
                    try { cmd.Kill(entireProcessTree: true); } catch { }
                    watch.Stop();
                    Logger.WriteLine(name + " " + args);
                    Logger.WriteLine($"{watch.ElapsedMilliseconds} ms: TIMEOUT after {timeoutMs} ms");
                    return string.Empty;
                }
                result = readTask.Result.Replace(Environment.NewLine, " ").Trim(' ');
            }
            else
            {
                result = cmd.StandardOutput.ReadToEnd().Replace(Environment.NewLine, " ").Trim(' ');
            }

            watch.Stop();
            Logger.WriteLine(name + " " + args);
            Logger.WriteLine(watch.ElapsedMilliseconds + " ms: " + result);
            cmd.WaitForExit();

            return result;
        }

        public static void SetPriority(ProcessPriorityClass priorityClass = ProcessPriorityClass.Normal)
        {
            try
            {
                using (Process p = Process.GetCurrentProcess())
                    p.PriorityClass = priorityClass;
            }
            catch (Exception ex)
            {
                Logger.WriteLine(ex.ToString());
            }
        }
    }
}
