using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class OrElseResultFailureResultValuePassInternal
    {
        internal static Result<TResult, TValue> OrElse<TFault, TResult, TValue>(
            ResultFailure<TFault> result,
            Func<TFault, Result<TResult, TValue>> onFailureResultFactory)
        {
            Result<TFault, TValue> converted = result;
            return converted.Match(
                onFailureResultFactory,
                Result<TResult, TValue>.Succeed);
        }

        internal static Task<Result<TResult, TValue>> OrElse<TFault, TResult, TValue>(
            ResultFailure<TFault> result,
            Func<TFault, Task<Result<TResult, TValue>>> onFailureResultFactory)
        {
            Result<TFault, TValue> converted = result;
            return converted.Match(
                onFailureResultFactory,
                value => Task.FromResult(Result<TResult, TValue>.Succeed(value)));
        }

        internal static ValueTask<Result<TResult, TValue>> OrElse<TFault, TResult, TValue>(
            ResultFailure<TFault> result,
            Func<TFault, ValueTask<Result<TResult, TValue>>> onFailureResultFactory)
        {
            Result<TFault, TValue> converted = result;
            return converted.Match(
                onFailureResultFactory,
                value => new(Result<TResult, TValue>.Succeed(value)));
        }
    }
}