using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class SelectManyOptionalValueValuePassInternal
    {
        internal static Optional<TResult> SelectMany<TValue1, TValue2, TResult>(
            Optional<TValue1> result,
            Func<TValue1, TValue2> nextSelector,
            Func<TValue1, TValue2, TResult> resultSelector)
        {
            return result.Match(
                Optional<TResult>.None,
                value1 => Optional<TResult>.Some(resultSelector(value1, nextSelector(value1))));
        }

        internal static Task<Optional<TResult>> SelectMany<TValue1, TValue2, TResult>(
            Optional<TValue1> result,
            Func<TValue1, Task<TValue2>> nextSelector,
            Func<TValue1, TValue2, TResult> resultSelector)
        {
            return result.Match(
                () => Task.FromResult(Optional<TResult>.None()),
                async value1 =>
                {
                    var result2 = await nextSelector(value1).ConfigureAwait(false);
                    return Optional<TResult>.Some(resultSelector(value1, result2));
                });
        }

        internal static ValueTask<Optional<TResult>> SelectMany<TValue1, TValue2, TResult>(
            Optional<TValue1> result,
            Func<TValue1, ValueTask<TValue2>> nextSelector,
            Func<TValue1, TValue2, TResult> resultSelector)
        {
            return result.Match<ValueTask<Optional<TResult>>>(
                () => new(Optional<TResult>.None()),
                async value1 =>
                {
                    var result2 = await nextSelector(value1).ConfigureAwait(false);
                    return Optional<TResult>.Some(resultSelector(value1, result2));
                });
        }

        internal static Task<Optional<TResult>> SelectMany<TValue1, TValue2, TResult>(
            Optional<TValue1> result,
            Func<TValue1, TValue2> nextSelector,
            Func<TValue1, TValue2, Task<TResult>> resultSelector)
        {
            return result.Match(
                () => Task.FromResult(Optional<TResult>.None()),
                async value1 =>
                {
                    var selector = await resultSelector(value1, nextSelector(value1)).ConfigureAwait(false);
                    return Optional<TResult>.Some(selector);
                });
        }

        internal static Task<Optional<TResult>> SelectMany<TValue1, TValue2, TResult>(
            Optional<TValue1> result,
            Func<TValue1, Task<TValue2>> nextSelector,
            Func<TValue1, TValue2, Task<TResult>> resultSelector)
        {
            return result.Match(
                () => Task.FromResult(Optional<TResult>.None()),
                async value1 =>
                {
                    var result2 = await nextSelector(value1).ConfigureAwait(false);
                    var selector = await resultSelector(value1, result2).ConfigureAwait(false);
                    return Optional<TResult>.Some(selector);
                });
        }

        internal static Task<Optional<TResult>> SelectMany<TValue1, TValue2, TResult>(
            Optional<TValue1> result,
            Func<TValue1, ValueTask<TValue2>> nextSelector,
            Func<TValue1, TValue2, Task<TResult>> resultSelector)
        {
            return result.Match(
                () => Task.FromResult(Optional<TResult>.None()),
                async value1 =>
                {
                    var result2 = await nextSelector(value1).ConfigureAwait(false);
                    var selector = await resultSelector(value1, result2).ConfigureAwait(false);
                    return Optional<TResult>.Some(selector);
                });
        }

        internal static ValueTask<Optional<TResult>> SelectMany<TValue1, TValue2, TResult>(
            Optional<TValue1> result,
            Func<TValue1, TValue2> nextSelector,
            Func<TValue1, TValue2, ValueTask<TResult>> resultSelector)
        {
            return result.Match<ValueTask<Optional<TResult>>>(
                () => new(Optional<TResult>.None()),
                async value1 =>
                {
                    var selector = await resultSelector(value1, nextSelector(value1)).ConfigureAwait(false);
                    return Optional<TResult>.Some(selector);
                });
        }

        internal static Task<Optional<TResult>> SelectMany<TValue1, TValue2, TResult>(
            Optional<TValue1> result,
            Func<TValue1, Task<TValue2>> nextSelector,
            Func<TValue1, TValue2, ValueTask<TResult>> resultSelector)
        {
            return result.Match<Task<Optional<TResult>>>(
                () => Task.FromResult(Optional<TResult>.None()),
                async value1 =>
                {
                    var result2 = await nextSelector(value1).ConfigureAwait(false);
                    var selector = await resultSelector(value1, result2).ConfigureAwait(false);
                    return Optional<TResult>.Some(selector);
                });
        }

        internal static ValueTask<Optional<TResult>> SelectMany<TValue1, TValue2, TResult>(
            Optional<TValue1> result,
            Func<TValue1, ValueTask<TValue2>> nextSelector,
            Func<TValue1, TValue2, ValueTask<TResult>> resultSelector)
        {
            return result.Match<ValueTask<Optional<TResult>>>(
                () => new(Optional<TResult>.None()),
                async value1 =>
                {
                    var result2 = await nextSelector(value1).ConfigureAwait(false);
                    var selector = await resultSelector(value1, result2).ConfigureAwait(false);
                    return Optional<TResult>.Some(selector);
                });
        }
    }
}