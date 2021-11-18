using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    public static class GetValueOrDefaultResultValueExtensions
    {
        [Pure]
        public static TValue? GetValueOrDefault<TFault, TValue>(this IResult<TFault, TValue> result)
        {
            return result.GetValueOrElse(default(TValue));
        }
    }
}