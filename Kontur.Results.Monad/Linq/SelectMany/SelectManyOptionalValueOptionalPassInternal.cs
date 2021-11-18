using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class SelectManyOptionalValueOptionalPassInternal
    {
        internal static Optional<TResult> SelectMany<TValue1, TValue2, TResult>(
            Optional<TValue1> result,
            Func<TValue1, TValue2> nextSelector,
            Func<TValue1, TValue2, Optional<TResult>> resultSelector)
        {
            return result.Match(
                Optional<TResult>.None,
                value1 => resultSelector(value1, nextSelector(value1)));
        }

        internal static Task<Optional<TResult>> SelectMany<TValue1, TValue2, TResult>(
            Optional<TValue1> result,
            Func<TValue1, Task<TValue2>> nextSelector,
            Func<TValue1, TValue2, Optional<TResult>> resultSelector)
        {
            return result.Match(
                () => Task.FromResult(Optional<TResult>.None()),
                async value1 =>
                {
                    var result2 = await nextSelector(value1).ConfigureAwait(false);
                    return resultSelector(value1, result2);
                });
        }

        internal static ValueTask<Optional<TResult>> SelectMany<TValue1, TValue2, TResult>(
            Optional<TValue1> result,
            Func<TValue1, ValueTask<TValue2>> nextSelector,
            Func<TValue1, TValue2, Optional<TResult>> resultSelector)
        {
            return result.Match<ValueTask<Optional<TResult>>>(
                () => new(Optional<TResult>.None()),
                async value1 =>
                {
                    var result2 = await nextSelector(value1).ConfigureAwait(false);
                    return resultSelector(value1, result2);
                });
        }

        internal static Task<Optional<TResult>> SelectMany<TValue1, TValue2, TResult>(
            Optional<TValue1> result,
            Func<TValue1, TValue2> nextSelector,
            Func<TValue1, TValue2, Task<Optional<TResult>>> resultSelector)
        {
            return result.Match(
                () => Task.FromResult(Optional<TResult>.None()),
                value1 => resultSelector(value1, nextSelector(value1)));
        }

        internal static Task<Optional<TResult>> SelectMany<TValue1, TValue2, TResult>(
            Optional<TValue1> result,
            Func<TValue1, Task<TValue2>> nextSelector,
            Func<TValue1, TValue2, Task<Optional<TResult>>> resultSelector)
        {
            return result.Match(
                () => Task.FromResult(Optional<TResult>.None()),
                async value1 =>
                {
                    var result2 = await nextSelector(value1).ConfigureAwait(false);
                    return await resultSelector(value1, result2).ConfigureAwait(false);
                });
        }

        internal static Task<Optional<TResult>> SelectMany<TValue1, TValue2, TResult>(
            Optional<TValue1> result,
            Func<TValue1, ValueTask<TValue2>> nextSelector,
            Func<TValue1, TValue2, Task<Optional<TResult>>> resultSelector)
        {
            return result.Match(
                () => Task.FromResult(Optional<TResult>.None()),
                async value1 =>
                {
                    var result2 = await nextSelector(value1).ConfigureAwait(false);
                    return await resultSelector(value1, result2).ConfigureAwait(false);
                });
        }

        internal static ValueTask<Optional<TResult>> SelectMany<TValue1, TValue2, TResult>(
            Optional<TValue1> result,
            Func<TValue1, TValue2> nextSelector,
            Func<TValue1, TValue2, ValueTask<Optional<TResult>>> resultSelector)
        {
            return result.Match(
                () => new(Optional<TResult>.None()),
                value1 => resultSelector(value1, nextSelector(value1)));
        }

        internal static Task<Optional<TResult>> SelectMany<TValue1, TValue2, TResult>(
            Optional<TValue1> result,
            Func<TValue1, Task<TValue2>> nextSelector,
            Func<TValue1, TValue2, ValueTask<Optional<TResult>>> resultSelector)
        {
            return result.Match<Task<Optional<TResult>>>(
                () => Task.FromResult(Optional<TResult>.None()),
                async value1 =>
                {
                    var result2 = await nextSelector(value1).ConfigureAwait(false);
                    return await resultSelector(value1, result2).ConfigureAwait(false);
                });
        }

        internal static ValueTask<Optional<TResult>> SelectMany<TValue1, TValue2, TResult>(
            Optional<TValue1> result,
            Func<TValue1, ValueTask<TValue2>> nextSelector,
            Func<TValue1, TValue2, ValueTask<Optional<TResult>>> resultSelector)
        {
            return result.Match<ValueTask<Optional<TResult>>>(
                () => new(Optional<TResult>.None()),
                async value1 =>
                {
                    var result2 = await nextSelector(value1).ConfigureAwait(false);
                    return await resultSelector(value1, result2).ConfigureAwait(false);
                });
        }
    }
}