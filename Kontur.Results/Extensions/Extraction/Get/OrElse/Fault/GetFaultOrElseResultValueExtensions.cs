using System;
using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    public static class GetFaultOrElseResultValueExtensions
    {
        public static TFault GetFaultOrElse<TFault, TValue>(
            this IResult<TFault, TValue> result,
            Func<TValue, TFault> faultFactory)
        {
            return result.Match(fault => fault, faultFactory);
        }

        public static TFault GetFaultOrElse<TFault, TValue>(this IResult<TFault, TValue> result, Func<TFault> faultFactory)
        {
            return result.GetFaultOrElse(_ => faultFactory());
        }

        [Pure]
        public static TFault GetFaultOrElse<TFault, TValue>(this IResult<TFault, TValue> result, TFault fault)
        {
            return result.GetFaultOrElse(() => fault);
        }
    }
}