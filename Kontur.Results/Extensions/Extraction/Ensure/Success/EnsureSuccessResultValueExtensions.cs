using System;

namespace Kontur.Results
{
    public static class EnsureSuccessResultValueExtensions
    {
        /// <summary>Throws if the result is a failure.</summary>
        public static void EnsureSuccess<TFault, TValue>(this IResult<TFault, TValue> result, Func<TFault, Exception> exceptionFactory)
        {
            result.OnFailure(fault => throw exceptionFactory(fault));
        }

        /// <inheritdoc cref="EnsureSuccess{TFault, TValue}(IResult{TFault, TValue}, Func{TFault, Exception})"/>
        public static void EnsureSuccess<TFault, TValue>(this IResult<TFault, TValue> result, Func<Exception> exceptionFactory)
        {
            result.EnsureSuccess(_ => exceptionFactory());
        }

        /// <inheritdoc cref="EnsureSuccess{TFault, TValue}(IResult{TFault, TValue}, Func{TFault, Exception})"/>
        public static void EnsureSuccess<TFault, TValue>(this IResult<TFault, TValue> result, Exception exception)
        {
            result.EnsureSuccess(() => exception);
        }

        /// <inheritdoc cref="EnsureSuccess{TFault, TValue}(IResult{TFault, TValue}, Func{TFault, Exception})"/>
        /// <exception cref="ResultFailedException{TFault}"/>
        public static void EnsureSuccess<TFault, TValue>(this IResult<TFault, TValue> result)
        {
            result.EnsureSuccess(fault => new ResultFailedException<TFault>(fault, $"{result} is failure"));
        }
    }
}