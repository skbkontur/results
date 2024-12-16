using System.IO;

namespace dotnetcementrefs;

internal static class FileUtilities
{
    public static string? GetDirectoryNameOfDirectoryAbove(string startingDirectory, string directoryName)
    {
        var lookInDirectory = Path.GetFullPath(startingDirectory);

        do
        {
            var possibleDirectory = Path.Combine(lookInDirectory, directoryName);

            if (Directory.Exists(possibleDirectory))
            {
                return lookInDirectory;
            }

            lookInDirectory = Path.GetDirectoryName(lookInDirectory);
        } while (lookInDirectory != null);

        return null;
    }
}