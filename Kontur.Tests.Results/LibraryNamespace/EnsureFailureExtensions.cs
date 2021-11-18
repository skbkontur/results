using Kontur.Results;
using Kontur.Tests.Results.Inheritance;

namespace Kontur.Tests.Results.LibraryNamespace
{
    internal static class EnsureFailureExtensions
    {
        public static void EnsureFailure(this Result<CustomFault> result)
        {
            result.EnsureFailure(LibraryException.Instance);
        }

        public static void EnsureFailure<TValue>(this Result<CustomFault, TValue> result)
        {
            result.EnsureFailure(LibraryException.Instance);
        }

        public static void EnsureFailure<TFault>(this Result<TFault, CustomValue> result)
        {
            result.EnsureFailure(LibraryException.Instance);
        }

        public static void EnsureFailure(this StringFaultResult<CustomValue> result)
        {
            result.EnsureFailure(LibraryException.Instance);
        }
    }
}