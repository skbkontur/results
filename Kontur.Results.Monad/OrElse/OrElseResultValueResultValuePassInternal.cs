using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class OrElseResultValueResultValuePassInternal
    {
        internal static Result<TResult, TValue> OrElse<TFault, TValue, TResult>(
            Result<TFault, TValue> result,
            Func<TFault, Result<TResult, TValue>> onFailureResultFactory)
        {
            return result.Match(onFailureResultFactory, Result<TResult, TValue>.Succeed);
        }

        internal static Task<Result<TResult, TValue>> OrElse<TFault, TValue, TResult>(
            Result<TFault, TValue> result,
            Func<TFault, Task<Result<TResult, TValue>>> onFailureResultFactory)
        {
            return result.Match(
                onFailureResultFactory,
                value => Task.FromResult(Result<TResult, TValue>.Succeed(value)));
        }

        internal static ValueTask<Result<TResult, TValue>> OrElse<TFault, TValue, TResult>(
            Result<TFault, TValue> result,
            Func<TFault, ValueTask<Result<TResult, TValue>>> onFailureResultFactory)
        {
            return result.Match(
                onFailureResultFactory,
                value => new(Result<TResult, TValue>.Succeed(value)));
        }
    }
}