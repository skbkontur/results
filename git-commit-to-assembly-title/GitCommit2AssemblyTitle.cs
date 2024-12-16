using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Microsoft.Build.Framework;
using Task = System.Threading.Tasks.Task;

// ReSharper disable AccessToDisposedClosure

namespace Vostok.Tools.GitCommit2AssemblyTitle
{
    public class GitCommit2AssemblyTitle : Microsoft.Build.Utilities.Task
    {
        private static readonly TimeSpan CommandTimeout = TimeSpan.FromSeconds(30);
        private static readonly TimeSpan StreamTimeout = TimeSpan.FromSeconds(5);

        public override bool Execute()
        {
            void LogMessageFunction(string str, object[] args) => Log.LogMessage(str, args);

            var assemblyTitleContent = GetAssemblyTitleContent(LogMessageFunction, AssemblyVersion);

            WriteAssemblyTitleContent(LogMessageFunction, assemblyTitleContent);

            return true;
        }

        [Required]
        public string AssemblyVersion { get; set; }

        public delegate void LogMessageFunction(string command, params object[] arguments);

        private static string GetAssemblyTitleContent(LogMessageFunction log, string assemblyVersion)
        {
            var gitMessage = GetGitCommitMessage(log)?.Trim();
            var gitCommitHash = GetGitCommitHash(log)?.Trim();

            if (string.IsNullOrEmpty(gitMessage))
            {
                log("Git commit message is empty.");
                return string.Empty;
            }

            if (string.IsNullOrEmpty(gitCommitHash))
            {
                log("Git commit hash is empty.");
                return string.Empty;
            }

            var titleBuilder = new StringBuilder();
            var contentBuilder = new StringBuilder();

            titleBuilder.AppendLine();
            titleBuilder.AppendLine(gitMessage);
            titleBuilder.Append($"Build date: {DateTime.Now:O}");

            var title = titleBuilder.ToString().Replace("\"", "'");

            var informationalVersion = $"{assemblyVersion}-{gitCommitHash?.Substring(0, 8)}";

            contentBuilder.AppendLine("using System.Reflection;");
            contentBuilder.AppendLine();
            contentBuilder.AppendLine($@"[assembly: AssemblyTitle(@""{title}"")]");
            contentBuilder.AppendLine();
            contentBuilder.AppendLine($@"[assembly: AssemblyInformationalVersion(""{informationalVersion}"")]");

            return contentBuilder.ToString();
        }

        private static string GetGitCommitMessage(LogMessageFunction log)
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? GetCommandOutput("cmd", "/C git log --pretty=\"Commit: %H %nAuthor: %an %nDate: %ai %nRef names: %d%n\" -1", log)
                : GetCommandOutput("git", "log --pretty=\"Commit: %H %nAuthor: %an %nDate: %ai %nRef names: %d%n\" -1", log);
        }

        private static string GetGitCommitHash(LogMessageFunction log)
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? GetCommandOutput("cmd", "/C git log --pretty=\"%H\" -1", log)
                : GetCommandOutput("git", "log --pretty=\"%H\" -1", log);
        }

        private static void WriteAssemblyTitleContent(LogMessageFunction log, string newContent)
        {
            const string properties = "Properties";

            var assemblyTitleFileName = Path.Combine(properties, "AssemblyTitle.cs");

            if (!Directory.Exists(properties))
                Directory.CreateDirectory(properties);

            log("{0} updated", assemblyTitleFileName);

            const int attempts = 10;

            var random = new Random(Guid.NewGuid().GetHashCode());

            for (var i = 1; i <= attempts; i++)
            {
                try
                {
                    var lastWriteTime = File.Exists(assemblyTitleFileName)
                        ? File.GetLastWriteTime(assemblyTitleFileName)
                        : DateTime.Now;

                    File.WriteAllText(assemblyTitleFileName, newContent);
                    File.SetLastWriteTime(assemblyTitleFileName, lastWriteTime);

                    return;
                }
                catch (IOException)
                {
                    log($"File {assemblyTitleFileName} is locked.");

                    if (i == attempts)
                        throw;

                    log("Wait...");
                    Thread.Sleep(random.Next(500, 1000));
                }
            }
        }

        private static string GetCommandOutput(string command, string args, LogMessageFunction log)
        {
            log(command + " " + args);

            var startInfo = new ProcessStartInfo
            {
                FileName = command,
                Arguments = args,
                WorkingDirectory = Directory.GetCurrentDirectory(),
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                ErrorDialog = false,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            var stdout = new StringBuilder();
            var stderr = new StringBuilder();

            try
            {
                using (var process = new Process {StartInfo = startInfo})
                {
                    if (!process.Start())
                        throw new Exception("Failed to start Git process.");

                    var stdoutTask = Task.Run(() => ReadStreamAsync(process.StandardOutput, stdout));
                    var stderrTask = Task.Run(() => ReadStreamAsync(process.StandardError, stderr));

                    if (!process.WaitForExit((int) CommandTimeout.TotalMilliseconds))
                    {
                        try
                        {
                            process.Kill();
                            log("process killed");
                        }
                        catch (Exception)
                        {
                            log("killing already exited process");
                        }

                        process.WaitForExit();
                    }

                    log("exit code:" + process.ExitCode);

                    stdoutTask.Wait(StreamTimeout);
                    stderrTask.Wait(StreamTimeout);

                    return stdout.Length > 0 ? stdout.ToString() : stderr.ToString();
                }
            }
            catch (Exception error)
            {
                log(error.Message);

                return string.Empty;
            }
        }

        private static async Task ReadStreamAsync(StreamReader reader, StringBuilder buffer)
        {
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync().ConfigureAwait(false);

                buffer.AppendLine(line);

                Console.Out.WriteLine(line);
            }
        }
    }
}