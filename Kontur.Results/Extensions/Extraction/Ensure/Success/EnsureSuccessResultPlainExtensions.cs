using System;

namespace Kontur.Results
{
    public static class EnsureSuccessResultPlainExtensions
    {
        /// <summary>Throws if the result is a failure.</summary>
        public static void EnsureSuccess<TFault>(this IResult<TFault> result, Func<TFault, Exception> exceptionFactory)
        {
            result.OnFailure(fault => throw exceptionFactory(fault));
        }

        /// <inheritdoc cref="EnsureSuccess{TFault}(IResult{TFault}, Func{TFault, Exception})"/>
        public static void EnsureSuccess<TFault>(this IResult<TFault> result, Func<Exception> exceptionFactory)
        {
            result.EnsureSuccess(_ => exceptionFactory());
        }

        /// <inheritdoc cref="EnsureSuccess{TFault}(IResult{TFault}, Func{TFault, Exception})"/>
        public static void EnsureSuccess<TFault>(this IResult<TFault> result, Exception exception)
        {
            result.EnsureSuccess(() => exception);
        }

        /// <inheritdoc cref="EnsureSuccess{TFault}(IResult{TFault}, Func{TFault, Exception})"/>
        /// <exception cref="ResultFailedException{TFault}"/>
        public static void EnsureSuccess<TFault>(this IResult<TFault> result)
        {
            result.EnsureSuccess(fault => new ResultFailedException<TFault>(fault, $"{result} is failure"));
        }
    }
}