using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class OrElseResultPlainResultFailurePassInternal
    {
        internal static Result<TResult> OrElse<TFault, TResult>(
            Result<TFault> result,
            Func<TFault, ResultFailure<TResult>> onFailureResultFactory)
        {
            return result.Match(onFailureResultFactory, () => Result<TResult>.Succeed());
        }

        internal static Task<Result<TResult>> OrElse<TFault, TResult>(
            Result<TFault> result,
            Func<TFault, Task<ResultFailure<TResult>>> onFailureResultFactory)
        {
            return result.Match<Task<Result<TResult>>>(
                async fault => await onFailureResultFactory(fault).ConfigureAwait(false),
                () => Task.FromResult(Result<TResult>.Succeed()));
        }

        internal static ValueTask<Result<TResult>> OrElse<TFault, TResult>(
            Result<TFault> result,
            Func<TFault, ValueTask<ResultFailure<TResult>>> onFailureResultFactory)
        {
            return result.Match<ValueTask<Result<TResult>>>(
                async fault => await onFailureResultFactory(fault).ConfigureAwait(false),
                () => new(Result<TResult>.Succeed()));
        }
    }
}