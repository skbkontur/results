using System;

namespace Kontur.Results
{
    public static class EnsureFailureResultPlainExtensions
    {
        /// <summary>Throws if the result is successful.</summary>
        public static void EnsureFailure<TFault>(this IResult<TFault> result, Func<Exception> exceptionFactory)
        {
            result.OnSuccess(() => throw exceptionFactory());
        }

        /// <inheritdoc cref="EnsureFailure{TFault}(IResult{TFault}, Func{Exception})"/>
        public static void EnsureFailure<TFault>(this IResult<TFault> result, Exception exception)
        {
            result.EnsureFailure(() => exception);
        }

        /// <inheritdoc cref="EnsureFailure{TFault}(IResult{TFault}, Func{Exception})"/>
        /// <exception cref="ResultSucceedException"/>
        public static void EnsureFailure<TFault>(this IResult<TFault> result)
        {
            result.EnsureFailure(() => new ResultSucceedException($"No fault in {result}"));
        }
    }
}