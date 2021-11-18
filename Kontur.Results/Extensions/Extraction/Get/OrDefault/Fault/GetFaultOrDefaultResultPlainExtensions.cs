using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    public static class GetFaultOrDefaultResultPlainExtensions
    {
        [Pure]
        public static TFault? GetFaultOrDefault<TFault>(this IResult<TFault> result)
        {
            return result.GetFaultOrElse(default(TFault));
        }
    }
}