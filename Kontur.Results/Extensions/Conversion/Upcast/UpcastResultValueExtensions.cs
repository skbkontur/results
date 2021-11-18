using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    public static class UpcastResultValueExtensions
    {
        [Pure]
        public static Result<TFault, TValue> Upcast<TFault, TValue>(this IResult<TFault, TValue> result)
        {
            return result.Match(Result<TFault, TValue>.Fail, Result<TFault, TValue>.Succeed);
        }
    }
}
