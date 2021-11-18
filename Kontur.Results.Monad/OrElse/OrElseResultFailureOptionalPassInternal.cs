using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class OrElseResultFailureOptionalPassInternal
    {
        internal static Result<TFault, TValue> OrElse<TFault, TValue>(
            ResultFailure<TFault> result,
            Func<TFault, Optional<TValue>> onFailureOptionalFactory)
        {
            Result<TFault, TValue> match = result;
            return match.Match(
                fault => onFailureOptionalFactory(fault).Match(
                        () => Result<TFault, TValue>.Fail(fault),
                        Result<TFault, TValue>.Succeed),
                Result<TFault, TValue>.Succeed);
        }

        internal static Task<Result<TFault, TValue>> OrElse<TFault, TValue>(
            ResultFailure<TFault> result,
            Func<TFault, Task<Optional<TValue>>> onFailureOptionalFactory)
        {
            Result<TFault, TValue> match = result;
            return match.Match(
                async fault =>
                {
                    var optional = await onFailureOptionalFactory(fault).ConfigureAwait(false);
                    return optional.Match(
                        () => Result<TFault, TValue>.Fail(fault),
                        Result<TFault, TValue>.Succeed);
                },
                value => Task.FromResult(Result<TFault, TValue>.Succeed(value)));
        }

        internal static ValueTask<Result<TFault, TValue>> OrElse<TFault, TValue>(
            ResultFailure<TFault> result,
            Func<TFault, ValueTask<Optional<TValue>>> onFailureOptionalFactory)
        {
            Result<TFault, TValue> match = result;
            return match.Match<ValueTask<Result<TFault, TValue>>>(
                async fault =>
                {
                    var optional = await onFailureOptionalFactory(fault).ConfigureAwait(false);
                    return optional.Match(
                        () => Result<TFault, TValue>.Fail(fault),
                        Result<TFault, TValue>.Succeed);
                },
                value => new(Result<TFault, TValue>.Succeed(value)));
        }
    }
}