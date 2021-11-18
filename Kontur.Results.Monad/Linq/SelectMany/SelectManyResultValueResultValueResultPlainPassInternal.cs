using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class SelectManyResultValueResultValueResultPlainPassInternal
    {
        internal static Result<TFault> SelectMany<TFault, TValue1, TValue2>(
            Result<TFault, TValue1> result,
            Func<TValue1, Result<TFault, TValue2>> nextSelector,
            Func<TValue1, TValue2, Result<TFault>> resultSelector)
        {
            return result.Match(
                Result<TFault>.Fail,
                value1 =>
                {
                    var result2 = nextSelector(value1);
                    return result2.Match(
                        Result<TFault>.Fail,
                        value2 => resultSelector(value1, value2));
                });
        }

        internal static Task<Result<TFault>> SelectMany<TFault, TValue1, TValue2>(
            Result<TFault, TValue1> result,
            Func<TValue1, Task<Result<TFault, TValue2>>> nextSelector,
            Func<TValue1, TValue2, Result<TFault>> resultSelector)
        {
            return result.Match(
                fault1 => Task.FromResult(Result<TFault>.Fail(fault1)),
                async value1 =>
                {
                    var result2 = await nextSelector(value1).ConfigureAwait(false);
                    return result2.Match(
                        Result<TFault>.Fail,
                        value2 => resultSelector(value1, value2));
                });
        }

        internal static ValueTask<Result<TFault>> SelectMany<TFault, TValue1, TValue2>(
            Result<TFault, TValue1> result,
            Func<TValue1, ValueTask<Result<TFault, TValue2>>> nextSelector,
            Func<TValue1, TValue2, Result<TFault>> resultSelector)
        {
            return result.Match<ValueTask<Result<TFault>>>(
                fault1 => new(Result<TFault>.Fail(fault1)),
                async value1 =>
                {
                    var result2 = await nextSelector(value1).ConfigureAwait(false);
                    return result2.Match(
                        Result<TFault>.Fail,
                        value2 => resultSelector(value1, value2));
                });
        }

        internal static Task<Result<TFault>> SelectMany<TFault, TValue1, TValue2>(
            Result<TFault, TValue1> result,
            Func<TValue1, Result<TFault, TValue2>> nextSelector,
            Func<TValue1, TValue2, Task<Result<TFault>>> resultSelector)
        {
            return result.Match(
                fault1 => Task.FromResult(Result<TFault>.Fail(fault1)),
                value1 =>
                {
                    var result2 = nextSelector(value1);
                    return result2.Match(
                        fault2 => Task.FromResult(Result<TFault>.Fail(fault2)),
                        value2 => resultSelector(value1, value2));
                });
        }

        internal static Task<Result<TFault>> SelectMany<TFault, TValue1, TValue2>(
            Result<TFault, TValue1> result,
            Func<TValue1, Task<Result<TFault, TValue2>>> nextSelector,
            Func<TValue1, TValue2, Task<Result<TFault>>> resultSelector)
        {
            return result.Match(
                fault1 => Task.FromResult(Result<TFault>.Fail(fault1)),
                async value1 =>
                {
                    var result2 = await nextSelector(value1).ConfigureAwait(false);
                    return await result2.Match(
                        fault2 => Task.FromResult(Result<TFault>.Fail(fault2)),
                        value2 => resultSelector(value1, value2)).ConfigureAwait(false);
                });
        }

        internal static Task<Result<TFault>> SelectMany<TFault, TValue1, TValue2>(
            Result<TFault, TValue1> result,
            Func<TValue1, ValueTask<Result<TFault, TValue2>>> nextSelector,
            Func<TValue1, TValue2, Task<Result<TFault>>> resultSelector)
        {
            return result.Match(
                fault1 => Task.FromResult(Result<TFault>.Fail(fault1)),
                async value1 =>
                {
                    var result2 = await nextSelector(value1).ConfigureAwait(false);
                    return await result2.Match(
                        fault2 => Task.FromResult(Result<TFault>.Fail(fault2)),
                        value2 => resultSelector(value1, value2)).ConfigureAwait(false);
                });
        }

        internal static ValueTask<Result<TFault>> SelectMany<TFault, TValue1, TValue2>(
            Result<TFault, TValue1> result,
            Func<TValue1, Result<TFault, TValue2>> nextSelector,
            Func<TValue1, TValue2, ValueTask<Result<TFault>>> resultSelector)
        {
            return result.Match(
                fault1 => new(Result<TFault>.Fail(fault1)),
                value1 =>
                {
                    var result2 = nextSelector(value1);
                    return result2.Match(
                        fault2 => new(Result<TFault>.Fail(fault2)),
                        value2 => resultSelector(value1, value2));
                });
        }

        internal static Task<Result<TFault>> SelectMany<TFault, TValue1, TValue2>(
            Result<TFault, TValue1> result,
            Func<TValue1, Task<Result<TFault, TValue2>>> nextSelector,
            Func<TValue1, TValue2, ValueTask<Result<TFault>>> resultSelector)
        {
            return result.Match<Task<Result<TFault>>>(
                fault1 => Task.FromResult(Result<TFault>.Fail(fault1)),
                async value1 =>
                {
                    var result2 = await nextSelector(value1).ConfigureAwait(false);
                    return await result2.Match(
                        fault2 => new(Result<TFault>.Fail(fault2)),
                        value2 => resultSelector(value1, value2))
                        .ConfigureAwait(false);
                });
        }

        internal static ValueTask<Result<TFault>> SelectMany<TFault, TValue1, TValue2>(
            Result<TFault, TValue1> result,
            Func<TValue1, ValueTask<Result<TFault, TValue2>>> nextSelector,
            Func<TValue1, TValue2, ValueTask<Result<TFault>>> resultSelector)
        {
            return result.Match<ValueTask<Result<TFault>>>(
                fault1 => new(Result<TFault>.Fail(fault1)),
                async value1 =>
                {
                    var result2 = await nextSelector(value1).ConfigureAwait(false);
                    return await result2.Match(
                        fault2 => new(Result<TFault>.Fail(fault2)),
                        value2 => resultSelector(value1, value2)).ConfigureAwait(false);
                });
        }
    }
}