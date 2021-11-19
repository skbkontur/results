using System;

namespace Kontur.Results
{
    public static class EnsureFailureResultPlainExtensions
    {
        public static void EnsureFailure<TFault>(this IResult<TFault> result, Func<Exception> exceptionFactory)
        {
            result.OnSuccess(() => throw exceptionFactory());
        }

        public static void EnsureFailure<TFault>(this IResult<TFault> result, Exception exception)
        {
            result.EnsureFailure(() => exception);
        }

        public static void EnsureFailure<TFault>(this IResult<TFault> result)
        {
            result.EnsureFailure(() => new ResultSucceedException($"No fault in {result}"));
        }
    }
}