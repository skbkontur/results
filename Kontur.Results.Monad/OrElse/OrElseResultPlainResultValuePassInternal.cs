using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class OrElseResultPlainResultValuePassInternal
    {
        internal static Result<TResult> OrElse<TFault, TResult, TValue>(
            Result<TFault> result,
            Func<TFault, Result<TResult, TValue>> onFailureResultFactory)
        {
            return result.Match(fault => onFailureResultFactory(fault), Result<TResult>.Succeed);
        }

        internal static Task<Result<TResult>> OrElse<TFault, TResult, TValue>(
            Result<TFault> result,
            Func<TFault, Task<Result<TResult, TValue>>> onFailureResultFactory)
        {
            return result.Match<Task<Result<TResult>>>(
                async fault => await onFailureResultFactory(fault).ConfigureAwait(false),
                () => Task.FromResult(Result<TResult>.Succeed()));
        }

        internal static ValueTask<Result<TResult>> OrElse<TFault, TResult, TValue>(
            Result<TFault> result,
            Func<TFault, ValueTask<Result<TResult, TValue>>> onFailureResultFactory)
        {
            return result.Match<ValueTask<Result<TResult>>>(
                async fault => await onFailureResultFactory(fault).ConfigureAwait(false),
                () => new(Result<TResult>.Succeed()));
        }
    }
}