using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class ThenOptionalResultPlainPassInternal
    {
        internal static Optional<TValue> Then<TValue, TFault>(
            Optional<TValue> optional,
            Func<TValue, Result<TFault>> onSomeResultFactory)
        {
            return optional.Match(
                Optional<TValue>.None,
                value => onSomeResultFactory(value).Match(
                    Optional<TValue>.None,
                    optional));
        }

        internal static Task<Optional<TValue>> Then<TValue, TFault>(
            Optional<TValue> optional,
            Func<TValue, Task<Result<TFault>>> onSomeResultFactory)
        {
            return optional.Match(
                () => Task.FromResult(Optional<TValue>.None()),
                async value =>
                {
                    var onSuccessResult = await onSomeResultFactory(value).ConfigureAwait(false);
                    return onSuccessResult.Match(
                        Optional<TValue>.None,
                        optional);
                });
        }

        internal static ValueTask<Optional<TValue>> Then<TValue, TFault>(
            Optional<TValue> optional,
            Func<TValue, ValueTask<Result<TFault>>> onSomeResultFactory)
        {
            return optional.Match<ValueTask<Optional<TValue>>>(
                () => new(Optional<TValue>.None()),
                async value =>
                {
                    var onSuccessResult = await onSomeResultFactory(value).ConfigureAwait(false);
                    return onSuccessResult.Match(
                        Optional<TValue>.None,
                        optional);
                });
        }
    }
}