using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class SelectManyResultValueValueResultValuePassInternal
    {
        internal static Result<TFault, TResult> SelectMany<TFault, TValue1, TValue2, TResult>(
            Result<TFault, TValue1> result,
            Func<TValue1, TValue2> nextSelector,
            Func<TValue1, TValue2, Result<TFault, TResult>> resultSelector)
        {
            return result.Match(
                Result<TFault, TResult>.Fail,
                value1 => resultSelector(value1, nextSelector(value1)));
        }

        internal static Task<Result<TFault, TResult>> SelectMany<TFault, TValue1, TValue2, TResult>(
            Result<TFault, TValue1> result,
            Func<TValue1, Task<TValue2>> nextSelector,
            Func<TValue1, TValue2, Result<TFault, TResult>> resultSelector)
        {
            return result.Match(
                fault1 => Task.FromResult(Result<TFault, TResult>.Fail(fault1)),
                async value1 =>
                {
                    var result2 = await nextSelector(value1).ConfigureAwait(false);
                    return resultSelector(value1, result2);
                });
        }

        internal static ValueTask<Result<TFault, TResult>> SelectMany<TFault, TValue1, TValue2, TResult>(
            Result<TFault, TValue1> result,
            Func<TValue1, ValueTask<TValue2>> nextSelector,
            Func<TValue1, TValue2, Result<TFault, TResult>> resultSelector)
        {
            return result.Match<ValueTask<Result<TFault, TResult>>>(
                fault1 => new(Result<TFault, TResult>.Fail(fault1)),
                async value1 =>
                {
                    var result2 = await nextSelector(value1).ConfigureAwait(false);
                    return resultSelector(value1, result2);
                });
        }

        internal static Task<Result<TFault, TResult>> SelectMany<TFault, TValue1, TValue2, TResult>(
            Result<TFault, TValue1> result,
            Func<TValue1,  TValue2> nextSelector,
            Func<TValue1, TValue2, Task<Result<TFault, TResult>>> resultSelector)
        {
            return result.Match(
                fault1 => Task.FromResult(Result<TFault, TResult>.Fail(fault1)),
                value1 => resultSelector(value1, nextSelector(value1)));
        }

        internal static Task<Result<TFault, TResult>> SelectMany<TFault, TValue1, TValue2, TResult>(
            Result<TFault, TValue1> result,
            Func<TValue1, Task<TValue2>> nextSelector,
            Func<TValue1, TValue2, Task<Result<TFault, TResult>>> resultSelector)
        {
            return result.Match(
                fault1 => Task.FromResult(Result<TFault, TResult>.Fail(fault1)),
                async value1 =>
                {
                    var result2 = await nextSelector(value1).ConfigureAwait(false);
                    return await resultSelector(value1, result2).ConfigureAwait(false);
                });
        }

        internal static Task<Result<TFault, TResult>> SelectMany<TFault, TValue1, TValue2, TResult>(
            Result<TFault, TValue1> result,
            Func<TValue1, ValueTask<TValue2>> nextSelector,
            Func<TValue1, TValue2, Task<Result<TFault, TResult>>> resultSelector)
        {
            return result.Match(
                fault1 => Task.FromResult(Result<TFault, TResult>.Fail(fault1)),
                async value1 =>
                {
                    var result2 = await nextSelector(value1).ConfigureAwait(false);
                    return await resultSelector(value1, result2).ConfigureAwait(false);
                });
        }

        internal static ValueTask<Result<TFault, TResult>> SelectMany<TFault, TValue1, TValue2, TResult>(
            Result<TFault, TValue1> result,
            Func<TValue1, TValue2> nextSelector,
            Func<TValue1, TValue2, ValueTask<Result<TFault, TResult>>> resultSelector)
        {
            return result.Match(
                fault1 => new(Result<TFault, TResult>.Fail(fault1)),
                value1 => resultSelector(value1, nextSelector(value1)));
        }

        internal static Task<Result<TFault, TResult>> SelectMany<TFault, TValue1, TValue2, TResult>(
            Result<TFault, TValue1> result,
            Func<TValue1, Task<TValue2>> nextSelector,
            Func<TValue1, TValue2, ValueTask<Result<TFault, TResult>>> resultSelector)
        {
            return result.Match<Task<Result<TFault, TResult>>>(
                fault1 => Task.FromResult(Result<TFault, TResult>.Fail(fault1)),
                async value1 =>
                {
                    var result2 = await nextSelector(value1).ConfigureAwait(false);
                    return await resultSelector(value1, result2).ConfigureAwait(false);
                });
        }

        internal static ValueTask<Result<TFault, TResult>> SelectMany<TFault, TValue1, TValue2, TResult>(
            Result<TFault, TValue1> result,
            Func<TValue1, ValueTask<TValue2>> nextSelector,
            Func<TValue1, TValue2, ValueTask<Result<TFault, TResult>>> resultSelector)
        {
            return result.Match<ValueTask<Result<TFault, TResult>>>(
                fault1 => new(Result<TFault, TResult>.Fail(fault1)),
                async value1 =>
                {
                    var result2 = await nextSelector(value1).ConfigureAwait(false);
                    return await resultSelector(value1, result2).ConfigureAwait(false);
                });
        }
    }
}