using System;
using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    public static class GetValueOrThrowResultValueExtensions
    {
        public static TValue GetValueOrThrow<TFault, TValue>(
            this IResult<TFault, TValue> result,
            Func<TFault, Exception> exceptionFactory)
        {
            return result.Match(fault => throw exceptionFactory(fault), value => value);
        }

        public static TValue GetValueOrThrow<TFault, TValue>(
            this IResult<TFault, TValue> result,
            Func<Exception> exceptionFactory)
        {
            return result.GetValueOrThrow(_ => throw exceptionFactory());
        }

        [Pure]
        public static TValue GetValueOrThrow<TFault, TValue>(this IResult<TFault, TValue> result, Exception exception)
        {
            return result.GetValueOrThrow(() => exception);
        }

        [Pure]
        public static TValue GetValueOrThrow<TFault, TValue>(this IResult<TFault, TValue> result)
        {
            return result.GetValueOrThrow(fault => new ResultFailedException<TFault>(fault, $"Can not get value from {result}"));
        }
    }
}