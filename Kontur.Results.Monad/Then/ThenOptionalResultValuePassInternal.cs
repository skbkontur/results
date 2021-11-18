using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class ThenOptionalResultValuePassInternal
    {
        internal static Optional<TResult> Then<TValue, TFault, TResult>(
            Optional<TValue> optional,
            Func<TValue, Result<TFault, TResult>> onSomeResultFactory)
        {
            return optional.Match(
                Optional<TResult>.None,
                value => onSomeResultFactory(value).Match(
                    Optional<TResult>.None,
                    Optional<TResult>.Some));
        }

        internal static Task<Optional<TResult>> Then<TValue, TFault, TResult>(
            Optional<TValue> optional,
            Func<TValue, Task<Result<TFault, TResult>>> onSomeResultFactory)
        {
            return optional.Match(
                () => Task.FromResult(Optional<TResult>.None()),
                async value =>
                {
                    var onSuccessResult = await onSomeResultFactory(value).ConfigureAwait(false);
                    return onSuccessResult.Match(
                        Optional<TResult>.None,
                        Optional<TResult>.Some);
                });
        }

        internal static ValueTask<Optional<TResult>> Then<TValue, TFault, TResult>(
            Optional<TValue> optional,
            Func<TValue, ValueTask<Result<TFault, TResult>>> onSomeResultFactory)
        {
            return optional.Match<ValueTask<Optional<TResult>>>(
                () => new(Optional<TResult>.None()),
                async value =>
                {
                    var onSuccessResult = await onSomeResultFactory(value).ConfigureAwait(false);
                    return onSuccessResult.Match(
                        Optional<TResult>.None,
                        Optional<TResult>.Some);
                });
        }
    }
}