using Kontur.Results;

namespace Kontur.Tests.Results.LibraryNamespace
{
    internal static class EnsureHasValueExtensions
    {
        public static void EnsureHasValue(this Optional<CustomValue> optional)
        {
            optional.EnsureHasValue(LibraryException.Instance);
        }
    }
}