using JetBrains.Annotations;
using Kontur.Results;

namespace Kontur.Tests.Results.LibraryNamespace
{
    internal static class GetValueOrThrowOptionalExtensions
    {
        [Pure]
        public static CustomValue GetValueOrThrow(this IOptional<CustomValue> optional)
        {
            return optional.GetValueOrThrow(LibraryException.Instance);
        }
    }
}
