using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class OrElseResultPlainOptionalPassInternal
    {
        internal static Result<TFault> OrElse<TFault, TValue>(
            Result<TFault> result,
            Func<TFault, Optional<TValue>> onFailureOptionalFactory)
        {
            return result.Match(
                fault => onFailureOptionalFactory(fault).Match(
                    () => Result<TFault>.Fail(fault),
                    () => Result<TFault>.Succeed()),
                Result<TFault>.Succeed);
        }

        internal static Task<Result<TFault>> OrElse<TFault, TValue>(
            Result<TFault> result,
            Func<TFault, Task<Optional<TValue>>> onFailureOptionalFactory)
        {
            return result.Match(
                async fault =>
                {
                    var onSuccessOptional = await onFailureOptionalFactory(fault).ConfigureAwait(false);
                    return onSuccessOptional.Match(
                        () => Result<TFault>.Fail(fault),
                        () => Result<TFault>.Succeed());
                },
                () => Task.FromResult(Result<TFault>.Succeed()));
        }

        internal static ValueTask<Result<TFault>> OrElse<TFault, TValue>(
            Result<TFault> result,
            Func<TFault, ValueTask<Optional<TValue>>> onFailureOptionalFactory)
        {
            return result.Match<ValueTask<Result<TFault>>>(
                async fault =>
                {
                    var onSuccessResult = await onFailureOptionalFactory(fault).ConfigureAwait(false);
                    return onSuccessResult.Match(
                        () => Result<TFault>.Fail(fault),
                        () => Result<TFault>.Succeed());
                },
                () => new(Result<TFault>.Succeed()));
        }
    }
}