using Kontur.Results;
using Kontur.Tests.Results.Inheritance;

namespace Kontur.Tests.Results.LibraryNamespace
{
    internal static class EnsureSuccessExtensions
    {
        public static void EnsureSuccess(this Result<CustomFault> result)
        {
            result.EnsureSuccess(LibraryException.Instance);
        }

        public static void EnsureSuccess<TValue>(this Result<CustomFault, TValue> result)
        {
            result.EnsureSuccess(LibraryException.Instance);
        }

        public static void EnsureSuccess<TFault>(this Result<TFault, CustomValue> result)
        {
            result.EnsureSuccess(LibraryException.Instance);
        }

        public static void EnsureSuccess(this StringFaultResult<CustomValue> result)
        {
            result.EnsureSuccess(LibraryException.Instance);
        }
    }
}