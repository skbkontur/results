using System;

namespace Kontur.Results
{
    public static class EnsureFailureResultValueExtensions
    {
        public static void EnsureFailure<TFault, TValue>(this IResult<TFault, TValue> result, Func<TValue, Exception> exceptionFactory)
        {
            result.OnSuccess(value => throw exceptionFactory(value));
        }

        public static void EnsureFailure<TFault, TValue>(this IResult<TFault, TValue> result, Func<Exception> exceptionFactory)
        {
            result.EnsureFailure(_ => exceptionFactory());
        }

        public static void EnsureFailure<TFault, TValue>(this IResult<TFault, TValue> result, Exception exception)
        {
            result.EnsureFailure(() => exception);
        }

        public static void EnsureFailure<TFault, TValue>(this IResult<TFault, TValue> result)
        {
            result.EnsureFailure(new ResultSucceedException($"No fault in {result}"));
        }
    }
}