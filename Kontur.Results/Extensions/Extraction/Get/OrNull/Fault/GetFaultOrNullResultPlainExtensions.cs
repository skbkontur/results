using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    public static class GetFaultOrNullResultPlainExtensions
    {
        [Pure]
        public static TFault? GetFaultOrNull<TFault>(this IResult<TFault> result)
            where TFault : struct
        {
            return result.Match<TFault?>(fault => fault, () => null);
        }
    }
}