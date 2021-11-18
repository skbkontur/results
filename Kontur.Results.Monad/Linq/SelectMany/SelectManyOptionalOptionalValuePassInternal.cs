using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class SelectManyOptionalOptionalValuePassInternal
    {
        internal static Optional<TResult> SelectMany<TValue1, TValue2, TResult>(
            Optional<TValue1> result,
            Func<TValue1, Optional<TValue2>> nextSelector,
            Func<TValue1, TValue2, TResult> resultSelector)
        {
            return result.Match(
                Optional<TResult>.None,
                value1 =>
                {
                    var result2 = nextSelector(value1);
                    return result2.Match(
                        Optional<TResult>.None,
                        value2 => Optional<TResult>.Some(resultSelector(value1, value2)));
                });
        }

        internal static Task<Optional<TResult>> SelectMany<TValue1, TValue2, TResult>(
            Optional<TValue1> result,
            Func<TValue1, Task<Optional<TValue2>>> nextSelector,
            Func<TValue1, TValue2, TResult> resultSelector)
        {
            return result.Match(
                () => Task.FromResult(Optional<TResult>.None()),
                async value1 =>
                {
                    var result2 = await nextSelector(value1).ConfigureAwait(false);
                    return result2.Match(
                        Optional<TResult>.None,
                        value2 => Optional<TResult>.Some(resultSelector(value1, value2)));
                });
        }

        internal static ValueTask<Optional<TResult>> SelectMany<TValue1, TValue2, TResult>(
            Optional<TValue1> result,
            Func<TValue1, ValueTask<Optional<TValue2>>> nextSelector,
            Func<TValue1, TValue2, TResult> resultSelector)
        {
            return result.Match<ValueTask<Optional<TResult>>>(
                () => new(Optional<TResult>.None()),
                async value1 =>
                {
                    var result2 = await nextSelector(value1).ConfigureAwait(false);
                    return result2.Match(
                        Optional<TResult>.None,
                        value2 => Optional<TResult>.Some(resultSelector(value1, value2)));
                });
        }

        internal static Task<Optional<TResult>> SelectMany<TValue1, TValue2, TResult>(
            Optional<TValue1> result,
            Func<TValue1, Optional<TValue2>> nextSelector,
            Func<TValue1, TValue2, Task<TResult>> resultSelector)
        {
            return result.Match(
                () => Task.FromResult(Optional<TResult>.None()),
                value1 =>
                {
                    var result2 = nextSelector(value1);
                    return result2.Match(
                        () => Task.FromResult(Optional<TResult>.None()),
                        async value2 => Optional<TResult>.Some(await resultSelector(value1, value2).ConfigureAwait(false)));
                });
        }

        internal static Task<Optional<TResult>> SelectMany<TValue1, TValue2, TResult>(
            Optional<TValue1> result,
            Func<TValue1, Task<Optional<TValue2>>> nextSelector,
            Func<TValue1, TValue2, Task<TResult>> resultSelector)
        {
            return result.Match(
                () => Task.FromResult(Optional<TResult>.None()),
                async value1 =>
                {
                    var result2 = await nextSelector(value1).ConfigureAwait(false);
                    return await result2.Match(
                        () => Task.FromResult(Optional<TResult>.None()),
                        async value2 => Optional<TResult>.Some(await resultSelector(value1, value2).ConfigureAwait(false))).ConfigureAwait(false);
                });
        }

        internal static Task<Optional<TResult>> SelectMany<TValue1, TValue2, TResult>(
            Optional<TValue1> result,
            Func<TValue1, ValueTask<Optional<TValue2>>> nextSelector,
            Func<TValue1, TValue2, Task<TResult>> resultSelector)
        {
            return result.Match(
                () => Task.FromResult(Optional<TResult>.None()),
                async value1 =>
                {
                    var result2 = await nextSelector(value1).ConfigureAwait(false);
                    return await result2.Match(
                            () => Task.FromResult(Optional<TResult>.None()),
                            async value2 => Optional<TResult>.Some(await resultSelector(value1, value2).ConfigureAwait(false)))
                        .ConfigureAwait(false);
                });
        }

        internal static ValueTask<Optional<TResult>> SelectMany<TValue1, TValue2, TResult>(
            Optional<TValue1> result,
            Func<TValue1, Optional<TValue2>> nextSelector,
            Func<TValue1, TValue2, ValueTask<TResult>> resultSelector)
        {
            return result.Match(
                () => new(Optional<TResult>.None()),
                value1 =>
                {
                    var result2 = nextSelector(value1);
                    return result2.Match<ValueTask<Optional<TResult>>>(
                        () => new(Optional<TResult>.None()),
                        async value2 => Optional<TResult>.Some(await resultSelector(value1, value2).ConfigureAwait(false)));
                });
        }

        internal static Task<Optional<TResult>> SelectMany<TValue1, TValue2, TResult>(
            Optional<TValue1> result,
            Func<TValue1, Task<Optional<TValue2>>> nextSelector,
            Func<TValue1, TValue2, ValueTask<TResult>> resultSelector)
        {
            return result.Match<Task<Optional<TResult>>>(
                () => Task.FromResult(Optional<TResult>.None()),
                async value1 =>
                {
                    var result2 = await nextSelector(value1).ConfigureAwait(false);
                    return await result2.Match<ValueTask<Optional<TResult>>>(
                            () => new(Optional<TResult>.None()),
                            async value2 => Optional<TResult>.Some(await resultSelector(value1, value2).ConfigureAwait(false)))
                        .ConfigureAwait(false);
                });
        }

        internal static ValueTask<Optional<TResult>> SelectMany<TValue1, TValue2, TResult>(
            Optional<TValue1> result,
            Func<TValue1, ValueTask<Optional<TValue2>>> nextSelector,
            Func<TValue1, TValue2, ValueTask<TResult>> resultSelector)
        {
            return result.Match<ValueTask<Optional<TResult>>>(
                () => new(Optional<TResult>.None()),
                async value1 =>
                {
                    var result2 = await nextSelector(value1).ConfigureAwait(false);
                    return await result2.Match<ValueTask<Optional<TResult>>>(
                        () => new(Optional<TResult>.None()),
                        async value2 => Optional<TResult>.Some(await resultSelector(value1, value2).ConfigureAwait(false))).ConfigureAwait(false);
                });
        }
    }
}