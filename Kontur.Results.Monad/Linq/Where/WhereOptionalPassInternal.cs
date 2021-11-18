using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class WhereOptionalPassInternal
    {
        internal static Optional<TValue> Where<TValue>(
            Optional<TValue> optional,
            Func<TValue, bool> predicate)
        {
            return optional.Match(
                optional,
                value => predicate(value) ? optional : Optional<TValue>.None());
        }

        internal static Task<Optional<TValue>> Where<TValue>(
            Optional<TValue> optional,
            Func<TValue, Task<bool>> predicate)
        {
            return optional.Match(
                () => Task.FromResult(optional),
                async value =>
                {
                    var checkResult = await predicate(value).ConfigureAwait(false);
                    return checkResult ? optional : Optional<TValue>.None();
                });
        }

        internal static ValueTask<Optional<TValue>> Where<TValue>(
            Optional<TValue> optional,
            Func<TValue, ValueTask<bool>> predicate)
        {
            return optional.Match<ValueTask<Optional<TValue>>>(
                () => new(optional),
                async value =>
                {
                    var checkResult = await predicate(value).ConfigureAwait(false);
                    return checkResult ? optional : Optional<TValue>.None();
                });
        }
    }
}