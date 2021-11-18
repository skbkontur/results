using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    public static class UpcastResultPlainExtensions
    {
        [Pure]
        public static Result<TFault> Upcast<TFault>(this IResult<TFault> result)
        {
            return result.Match(Result<TFault>.Fail, Result<TFault>.Succeed);
        }
    }
}
