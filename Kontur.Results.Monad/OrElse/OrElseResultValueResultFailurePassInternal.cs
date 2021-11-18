using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class OrElseResultValueResultFailurePassInternal
    {
        internal static Result<TResult, TValue> OrElse<TFault, TValue, TResult>(
            Result<TFault, TValue> result,
            Func<TFault, ResultFailure<TResult>> onFailureResultFactory)
        {
            return result.Match(
                fault =>
                {
                    Result<TResult, TValue> resultFailure = onFailureResultFactory(fault);
                    return resultFailure;
                },
                Result<TResult, TValue>.Succeed);
        }

        internal static Task<Result<TResult, TValue>> OrElse<TFault, TValue, TResult>(
            Result<TFault, TValue> result,
            Func<TFault, Task<ResultFailure<TResult>>> onFailureResultFactory)
        {
            return result.Match<Task<Result<TResult, TValue>>>(
                async fault =>
                {
                    Result<TResult, TValue> resultFailure = await onFailureResultFactory(fault).ConfigureAwait(false);
                    return resultFailure;
                },
                value => Task.FromResult(Result<TResult, TValue>.Succeed(value)));
        }

        internal static ValueTask<Result<TResult, TValue>> OrElse<TFault, TValue, TResult>(
            Result<TFault, TValue> result,
            Func<TFault, ValueTask<ResultFailure<TResult>>> onFailureResultFactory)
        {
            return result.Match<ValueTask<Result<TResult, TValue>>>(
                async fault =>
                {
                    Result<TResult, TValue> resultFailure = await onFailureResultFactory(fault).ConfigureAwait(false);
                    return resultFailure;
                },
                value => new(Result<TResult, TValue>.Succeed(value)));
        }
    }
}