using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    public static class GetValueOrNullResultValueExtensions
    {
        /// <summary>Extracts the value as nullable. Constrained to value types where <c>default</c> may be a valid value.</summary>
        /// <returns>The contained value, or <c>null</c> if failed.</returns>
        [Pure]
        public static TValue? GetValueOrNull<TFault, TValue>(this IResult<TFault, TValue> result)
            where TValue : struct
        {
            return result.Match<TValue?>(() => null, value => value);
        }
    }
}