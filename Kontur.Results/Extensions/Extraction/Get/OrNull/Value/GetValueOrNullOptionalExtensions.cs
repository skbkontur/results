using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    public static class GetValueOrNullOptionalExtensions
    {
        [Pure]
        public static TValue? GetValueOrNull<TValue>(this IOptional<TValue> optional)
            where TValue : struct
        {
            return optional.Match<TValue?>(() => null, value => value);
        }
    }
}