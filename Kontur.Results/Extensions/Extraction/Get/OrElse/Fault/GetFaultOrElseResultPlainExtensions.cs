using System;
using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    public static class GetFaultOrElseResultPlainExtensions
    {
        public static TFault GetFaultOrElse<TFault>(this IResult<TFault> result, Func<TFault> onSuccessValueFactory)
        {
            return result.Match(fault => fault, onSuccessValueFactory);
        }

        [Pure]
        public static TFault GetFaultOrElse<TFault>(this IResult<TFault> result, TFault onSuccessValue)
        {
            return result.GetFaultOrElse(() => onSuccessValue);
        }
    }
}