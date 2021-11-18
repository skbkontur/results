using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    public static class GetValueOrNullResultValueExtensions
    {
        [Pure]
        public static TValue? GetValueOrNull<TFault, TValue>(this IResult<TFault, TValue> result)
            where TValue : struct
        {
            return result.Match<TValue?>(() => null, value => value);
        }
    }
}