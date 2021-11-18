using System;

namespace Kontur.Results
{
    public static class EnsureSuccessResultPlainExtensions
    {
        public static void EnsureSuccess<TFault>(this IResult<TFault> result, Func<TFault, Exception> exceptionFactory)
        {
            result.OnFailure(fault => throw exceptionFactory(fault));
        }

        public static void EnsureSuccess<TFault>(this IResult<TFault> result, Func<Exception> exceptionFactory)
        {
            result.EnsureSuccess(_ => exceptionFactory());
        }

        public static void EnsureSuccess<TFault>(this IResult<TFault> result, Exception exception)
        {
            result.EnsureSuccess(() => exception);
        }

        public static void EnsureSuccess<TFault>(this IResult<TFault> result)
        {
            result.EnsureSuccess(new ResultFailedException($"{result} is failure"));
        }
    }
}