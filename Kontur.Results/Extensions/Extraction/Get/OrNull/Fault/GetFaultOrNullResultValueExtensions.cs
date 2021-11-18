using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    public static class GetFaultOrNullResultValueExtensions
    {
        [Pure]
        public static TFault? GetFaultOrNull<TFault, TValue>(this IResult<TFault, TValue> result)
            where TFault : struct
        {
            return result.Match<TFault?>(fault => fault, () => null);
        }
    }
}