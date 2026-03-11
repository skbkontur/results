using System;

namespace Kontur.Results
{
    public static class EnsureFailureResultValueExtensions
    {
        /// <summary>Throws if the result is successful.</summary>
        public static void EnsureFailure<TFault, TValue>(this IResult<TFault, TValue> result, Func<TValue, Exception> exceptionFactory)
        {
            result.OnSuccess(value => throw exceptionFactory(value));
        }

        /// <inheritdoc cref="EnsureFailure{TFault, TValue}(IResult{TFault, TValue}, Func{TValue, Exception})"/>
        public static void EnsureFailure<TFault, TValue>(this IResult<TFault, TValue> result, Func<Exception> exceptionFactory)
        {
            result.EnsureFailure(_ => exceptionFactory());
        }

        /// <inheritdoc cref="EnsureFailure{TFault, TValue}(IResult{TFault, TValue}, Func{TValue, Exception})"/>
        public static void EnsureFailure<TFault, TValue>(this IResult<TFault, TValue> result, Exception exception)
        {
            result.EnsureFailure(() => exception);
        }

        /// <inheritdoc cref="EnsureFailure{TFault, TValue}(IResult{TFault, TValue}, Func{TValue, Exception})"/>
        /// <exception cref="ResultSucceedException{TValue}"/>
        public static void EnsureFailure<TFault, TValue>(this IResult<TFault, TValue> result)
        {
            result.EnsureFailure(value => new ResultSucceedException<TValue>(value, $"No fault in {result}"));
        }
    }
}