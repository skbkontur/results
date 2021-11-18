using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class SelectManyResultValueValueResultPlainPassInternal
    {
        internal static Result<TFault> SelectMany<TFault, TValue1, TValue2>(
            Result<TFault, TValue1> result,
            Func<TValue1, TValue2> nextSelector,
            Func<TValue1, TValue2, Result<TFault>> resultSelector)
        {
            return result.Match(
                Result<TFault>.Fail,
                value1 => resultSelector(value1, nextSelector(value1)));
        }

        internal static Task<Result<TFault>> SelectMany<TFault, TValue1, TValue2>(
            Result<TFault, TValue1> result,
            Func<TValue1, Task<TValue2>> nextSelector,
            Func<TValue1, TValue2, Result<TFault>> resultSelector)
        {
            return result.Match(
                fault1 => Task.FromResult(Result<TFault>.Fail(fault1)),
                async value1 =>
                {
                    var result2 = await nextSelector(value1).ConfigureAwait(false);
                    return resultSelector(value1, result2);
                });
        }

        internal static ValueTask<Result<TFault>> SelectMany<TFault, TValue1, TValue2>(
            Result<TFault, TValue1> result,
            Func<TValue1, ValueTask<TValue2>> nextSelector,
            Func<TValue1, TValue2, Result<TFault>> resultSelector)
        {
            return result.Match<ValueTask<Result<TFault>>>(
                fault1 => new(Result<TFault>.Fail(fault1)),
                async value1 =>
                {
                    var result2 = await nextSelector(value1).ConfigureAwait(false);
                    return resultSelector(value1, result2);
                });
        }

        internal static Task<Result<TFault>> SelectMany<TFault, TValue1, TValue2>(
            Result<TFault, TValue1> result,
            Func<TValue1, TValue2> nextSelector,
            Func<TValue1, TValue2, Task<Result<TFault>>> resultSelector)
        {
            return result.Match(
                fault1 => Task.FromResult(Result<TFault>.Fail(fault1)),
                value1 => resultSelector(value1, nextSelector(value1)));
        }

        internal static Task<Result<TFault>> SelectMany<TFault, TValue1, TValue2>(
            Result<TFault, TValue1> result,
            Func<TValue1, Task<TValue2>> nextSelector,
            Func<TValue1, TValue2, Task<Result<TFault>>> resultSelector)
        {
            return result.Match(
                fault1 => Task.FromResult(Result<TFault>.Fail(fault1)),
                async value1 =>
                {
                    var result2 = await nextSelector(value1).ConfigureAwait(false);
                    return await resultSelector(value1, result2).ConfigureAwait(false);
                });
        }

        internal static Task<Result<TFault>> SelectMany<TFault, TValue1, TValue2>(
            Result<TFault, TValue1> result,
            Func<TValue1, ValueTask<TValue2>> nextSelector,
            Func<TValue1, TValue2, Task<Result<TFault>>> resultSelector)
        {
            return result.Match(
                fault1 => Task.FromResult(Result<TFault>.Fail(fault1)),
                async value1 =>
                {
                    var result2 = await nextSelector(value1).ConfigureAwait(false);
                    return await resultSelector(value1, result2).ConfigureAwait(false);
                });
        }

        internal static ValueTask<Result<TFault>> SelectMany<TFault, TValue1, TValue2>(
            Result<TFault, TValue1> result,
            Func<TValue1, TValue2> nextSelector,
            Func<TValue1, TValue2, ValueTask<Result<TFault>>> resultSelector)
        {
            return result.Match(
                fault1 => new(Result<TFault>.Fail(fault1)),
                value1 => resultSelector(value1, nextSelector(value1)));
        }

        internal static Task<Result<TFault>> SelectMany<TFault, TValue1, TValue2>(
            Result<TFault, TValue1> result,
            Func<TValue1, Task<TValue2>> nextSelector,
            Func<TValue1, TValue2, ValueTask<Result<TFault>>> resultSelector)
        {
            return result.Match<Task<Result<TFault>>>(
                fault1 => Task.FromResult(Result<TFault>.Fail(fault1)),
                async value1 =>
                {
                    var result2 = await nextSelector(value1).ConfigureAwait(false);
                    return await resultSelector(value1, result2).ConfigureAwait(false);
                });
        }

        internal static ValueTask<Result<TFault>> SelectMany<TFault, TValue1, TValue2>(
            Result<TFault, TValue1> result,
            Func<TValue1, ValueTask<TValue2>> nextSelector,
            Func<TValue1, TValue2, ValueTask<Result<TFault>>> resultSelector)
        {
            return result.Match<ValueTask<Result<TFault>>>(
                fault1 => new(Result<TFault>.Fail(fault1)),
                async value1 =>
                {
                    var result2 = await nextSelector(value1).ConfigureAwait(false);
                    return await resultSelector(value1, result2).ConfigureAwait(false);
                });
        }
    }
}