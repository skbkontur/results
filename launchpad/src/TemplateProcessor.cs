using System.Collections.Generic;
using System.IO;
using Stubble.Core;
using Stubble.Core.Builders;

namespace launchpad
{
    internal class TemplateProcessor
    {
        public void Process(string directoryPath, Dictionary<string, string> variables)
        {
            var stubble = new StubbleBuilder().Build();

            var stack = new Stack<FileSystemInfo>();

            var directory = new DirectoryInfo(directoryPath);

            stack.Push(directory);
            
            while (stack.Count > 0)
            {
                var sysInfo = stack.Pop();
                if (sysInfo.Attributes.HasFlag(FileAttributes.Hidden))
                    continue;

                SubstituteName(sysInfo, stubble, variables);

                if (sysInfo is DirectoryInfo directoryInfo)
                {
                    foreach (var systemInfo in directoryInfo.EnumerateFileSystemInfos())
                    {
                        stack.Push(systemInfo);
                    }
                }

                if (sysInfo is FileInfo fileInfo)
                {
                    var substitutedText = stubble.Render(File.ReadAllText(fileInfo.FullName), variables);
                    File.WriteAllText(fileInfo.FullName, substitutedText);
                }
            }
        }

        private static void SubstituteName(FileSystemInfo info, StubbleVisitorRenderer stubble, Dictionary<string, string> variables)
        {
            var substitutedPath = stubble.Render(info.FullName, variables);
            if (substitutedPath.Equals(info.FullName))
                return;

            (info as DirectoryInfo)?.MoveTo(substitutedPath);
            (info as FileInfo)?.MoveTo(substitutedPath);
        }
    }
}
