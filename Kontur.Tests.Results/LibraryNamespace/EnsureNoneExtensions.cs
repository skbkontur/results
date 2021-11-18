using Kontur.Results;

namespace Kontur.Tests.Results.LibraryNamespace
{
    internal static class EnsureNoneExtensions
    {
        public static void EnsureNone(this Optional<CustomValue> optional)
        {
            optional.EnsureNone(LibraryException.Instance);
        }
    }
}