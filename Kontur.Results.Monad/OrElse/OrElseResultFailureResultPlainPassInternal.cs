using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class OrElseResultFailureResultPlainPassInternal
    {
        internal static Result<TResult> OrElse<TFault, TResult>(
            ResultFailure<TFault> result,
            Func<TFault, Result<TResult>> onFailureResultFactory)
        {
            return result.Match(
                fault => onFailureResultFactory(fault),
                Result<TResult>.Succeed);
        }

        internal static Task<Result<TResult>> OrElse<TFault, TResult>(
            ResultFailure<TFault> result,
            Func<TFault, Task<Result<TResult>>> onFailureResultFactory)
        {
            return result.Match(
                onFailureResultFactory,
                () => Task.FromResult(Result<TResult>.Succeed()));
        }

        internal static ValueTask<Result<TResult>> OrElse<TFault, TResult>(
            ResultFailure<TFault> result,
            Func<TFault, ValueTask<Result<TResult>>> onFailureResultFactory)
        {
            return result.Match(
                onFailureResultFactory,
                () => new(Result<TResult>.Succeed()));
        }
    }
}