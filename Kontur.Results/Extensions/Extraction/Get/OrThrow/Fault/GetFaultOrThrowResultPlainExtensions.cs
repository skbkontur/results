using System;
using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    public static class GetFaultOrThrowResultPlainExtensions
    {
        public static TFault GetFaultOrThrow<TFault>(this IResult<TFault> result, Func<Exception> exceptionFactory)
        {
            return result.Match(fault => fault, () => throw exceptionFactory());
        }

        [Pure]
        public static TFault GetFaultOrThrow<TFault>(this IResult<TFault> result, Exception exception)
        {
            return result.GetFaultOrThrow(() => exception);
        }

        [Pure]
        public static TFault GetFaultOrThrow<TFault>(this IResult<TFault> result)
        {
            return result.GetFaultOrThrow(() => new ResultSucceedException($"Can not get fault from {result}"));
        }
    }
}