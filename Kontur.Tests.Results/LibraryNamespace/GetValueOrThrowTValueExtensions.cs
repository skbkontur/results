using JetBrains.Annotations;
using Kontur.Results;
using Kontur.Tests.Results.Inheritance;

namespace Kontur.Tests.Results.LibraryNamespace
{
    internal static class GetValueOrThrowTValueExtensions
    {
        [Pure]
        public static TValue GetValueOrThrow<TValue>(this IResult<CustomFault, TValue> result)
        {
            return result.GetValueOrThrow(LibraryException.Instance);
        }

        [Pure]
        public static CustomValue GetValueOrThrow<TFault>(this IResult<TFault, CustomValue> result)
        {
            return result.GetValueOrThrow(LibraryException.Instance);
        }

        [Pure]
        public static CustomValue GetValueOrThrow(this StringFaultResult<CustomValue> result)
        {
            return result.GetValueOrThrow(LibraryException.Instance);
        }
    }
}
