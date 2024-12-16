using System;
using System.IO;

namespace launchpad
{
    internal class TemporaryDirectory : IDisposable
    {
        private readonly DirectoryInfo info;

        public TemporaryDirectory()
        {
            info = new DirectoryInfo(Path.Combine(Path.GetTempPath(), "launchpad", Guid.NewGuid().ToString().Substring(0, 8)));
            info.Create();
            info.Refresh();
        }

        public string FullPath => info.FullName;

        public void Dispose()
        {
            try
            {
                Directory.Delete(FullPath, true);
            }
            catch (IOException)
            {
            }
        }

        public void CopyContentsTo(string directory)
        {
            info.Refresh();

            foreach (var subDirectory in info.GetDirectories(string.Empty, SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(Path.Combine(directory, Path.GetRelativePath(FullPath, subDirectory.FullName)));
            }

            foreach (var file in info.GetFiles(string.Empty, SearchOption.AllDirectories))
            {
                file.CopyTo(Path.Combine(directory, Path.GetRelativePath(FullPath, file.FullName)), true);
            }
        }
    }
}
