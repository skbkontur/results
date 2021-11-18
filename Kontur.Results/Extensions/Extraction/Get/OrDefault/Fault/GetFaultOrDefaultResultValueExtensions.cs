using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    public static class GetFaultOrDefaultResultValueExtensions
    {
        [Pure]
        public static TFault? GetFaultOrDefault<TFault, TValue>(this IResult<TFault, TValue> result)
        {
            return result.GetFaultOrElse(default(TFault));
        }
    }
}