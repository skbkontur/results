using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class OrElseResultValueOptionalPassInternal
    {
        internal static Result<TFault, TValue> OrElse<TFault, TValue>(
            Result<TFault, TValue> result,
            Func<TFault, Optional<TValue>> onFailureOptionalFactory)
        {
            return result.Match(
                fault => onFailureOptionalFactory(fault).Match(
                    () => Result<TFault, TValue>.Fail(fault),
                    Result<TFault, TValue>.Succeed),
                Result<TFault, TValue>.Succeed);
        }

        internal static Task<Result<TFault, TValue>> OrElse<TFault, TValue>(
            Result<TFault, TValue> result,
            Func<TFault, Task<Optional<TValue>>> onFailureOptionalFactory)
        {
            return result.Match(
                async fault =>
                {
                    var onSuccessOptional = await onFailureOptionalFactory(fault).ConfigureAwait(false);
                    return onSuccessOptional.Match(
                        () => Result<TFault, TValue>.Fail(fault),
                        Result<TFault, TValue>.Succeed);
                },
                value => Task.FromResult(Result<TFault, TValue>.Succeed(value)));
        }

        internal static ValueTask<Result<TFault, TValue>> OrElse<TFault, TValue>(
            Result<TFault, TValue> result,
            Func<TFault, ValueTask<Optional<TValue>>> onFailureOptionalFactory)
        {
            return result.Match<ValueTask<Result<TFault, TValue>>>(
                async fault =>
                {
                    var onSuccessResult = await onFailureOptionalFactory(fault).ConfigureAwait(false);
                    return onSuccessResult.Match(
                        () => Result<TFault, TValue>.Fail(fault),
                        Result<TFault, TValue>.Succeed);
                },
                value => new(Result<TFault, TValue>.Succeed(value)));
        }
    }
}