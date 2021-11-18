using JetBrains.Annotations;
using Kontur.Results;
using Kontur.Tests.Results.Inheritance;

namespace Kontur.Tests.Results.LibraryNamespace
{
    internal static class GetFaultOrThrowExtensions
    {
        [Pure]
        public static CustomFault GetFaultOrThrow(this IResult<CustomFault> result)
        {
            return result.GetFaultOrThrow(LibraryException.Instance);
        }

        [Pure]
        public static CustomFault GetFaultOrThrow<TValue>(this IResult<CustomFault, TValue> result)
        {
            return result.GetFaultOrThrow(LibraryException.Instance);
        }

        [Pure]
        public static TFault GetFaultOrThrow<TFault>(this IResult<TFault, CustomValue> result)
        {
            return result.GetFaultOrThrow(LibraryException.Instance);
        }

        [Pure]
        public static StringFault GetFaultOrThrow(this StringFaultResult<CustomValue> result)
        {
            return result.GetFaultOrThrow(LibraryException.Instance);
        }
    }
}
