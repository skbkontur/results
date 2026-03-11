using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    public static class GetValueOrNullOptionalExtensions
    {
        /// <summary>Extracts the value as nullable. Constrained to value types where <c>default</c> may be a valid value.</summary>
        /// <returns>The contained value, or <c>null</c> if None.</returns>
        [Pure]
        public static TValue? GetValueOrNull<TValue>(this IOptional<TValue> optional)
            where TValue : struct
        {
            return optional.Match<TValue?>(() => null, value => value);
        }
    }
}