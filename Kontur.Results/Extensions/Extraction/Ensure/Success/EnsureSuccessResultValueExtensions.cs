using System;

namespace Kontur.Results
{
    public static class EnsureSuccessResultValueExtensions
    {
        public static void EnsureSuccess<TFault, TValue>(this IResult<TFault, TValue> result, Func<TFault, Exception> exceptionFactory)
        {
            result.OnFailure(fault => throw exceptionFactory(fault));
        }

        public static void EnsureSuccess<TFault, TValue>(this IResult<TFault, TValue> result, Func<Exception> exceptionFactory)
        {
            result.EnsureSuccess(_ => exceptionFactory());
        }

        public static void EnsureSuccess<TFault, TValue>(this IResult<TFault, TValue> result, Exception exception)
        {
            result.EnsureSuccess(() => exception);
        }

        public static void EnsureSuccess<TFault, TValue>(this IResult<TFault, TValue> result)
        {
            result.EnsureSuccess(fault => new ResultFailedException<TFault>(fault, $"{result} is failure"));
        }
    }
}