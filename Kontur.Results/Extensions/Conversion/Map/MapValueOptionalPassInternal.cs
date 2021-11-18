using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class MapValueOptionalPassInternal
    {
        internal static Optional<TResult> MapValue<TValue, TResult>(
            IOptional<TValue> optional,
            Func<TValue, TResult> valueFactory)
        {
            return optional.Match(Optional<TResult>.None, value => Optional<TResult>.Some(valueFactory(value)));
        }

        internal static Task<Optional<TResult>> MapValue<TValue, TResult>(
            IOptional<TValue> optional,
            Func<TValue, Task<TResult>> valueFactory)
        {
            return optional.Match(
                () => Task.FromResult(Optional<TResult>.None()),
                async value =>
                {
                    var createdValue = await valueFactory(value).ConfigureAwait(false);
                    return Optional<TResult>.Some(createdValue);
                });
        }

        internal static ValueTask<Optional<TResult>> MapValue<TValue, TResult>(
            IOptional<TValue> optional,
            Func<TValue, ValueTask<TResult>> valueFactory)
        {
            return optional.Match<ValueTask<Optional<TResult>>>(
                () => new(Optional<TResult>.None()),
                async value =>
                {
                    var createdValue = await valueFactory(value).ConfigureAwait(false);
                    return Optional<TResult>.Some(createdValue);
                });
        }
    }
}