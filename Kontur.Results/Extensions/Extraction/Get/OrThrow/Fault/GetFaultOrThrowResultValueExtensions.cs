using System;
using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    public static class GetFaultOrThrowResultValueExtensions
    {
        public static TFault GetFaultOrThrow<TFault, TValue>(
            this IResult<TFault, TValue> result,
            Func<TValue, Exception> exceptionFactory)
        {
            return result.Match(fault => fault, value => throw exceptionFactory(value));
        }

        public static TFault GetFaultOrThrow<TFault, TValue>(
            this IResult<TFault, TValue> result,
            Func<Exception> exceptionFactory)
        {
            return result.GetFaultOrThrow(_ => throw exceptionFactory());
        }

        [Pure]
        public static TFault GetFaultOrThrow<TFault, TValue>(this IResult<TFault, TValue> result, Exception exception)
        {
            return result.GetFaultOrThrow(() => exception);
        }

        [Pure]
        public static TFault GetFaultOrThrow<TFault, TValue>(this IResult<TFault, TValue> result)
        {
            return result.GetFaultOrThrow(value => new ResultSucceedException<TValue>(value, $"Can not get fault from {result}"));
        }
    }
}