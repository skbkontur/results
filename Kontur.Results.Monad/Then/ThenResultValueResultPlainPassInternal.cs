using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class ThenResultValueResultPlainPassInternal
    {
        internal static Result<TFault, TValue> Then<TFault, TValue>(
            Result<TFault, TValue> result,
            Func<TValue, Result<TFault>> onSuccessResultFactory)
        {
            return result.Match(Result<TFault, TValue>.Fail, value => onSuccessResultFactory(value).Then(result));
        }

        internal static Task<Result<TFault, TValue>> Then<TFault, TValue>(
            Result<TFault, TValue> result,
            Func<TValue, Task<Result<TFault>>> onSuccessResultFactory)
        {
            return result.Match(
                fault => Task.FromResult(Result<TFault, TValue>.Fail(fault)),
                async value =>
                {
                    var onSuccessResult = await onSuccessResultFactory(value).ConfigureAwait(false);
                    return onSuccessResult.Then(result);
                });
        }

        internal static ValueTask<Result<TFault, TValue>> Then<TFault, TValue>(
            Result<TFault, TValue> result,
            Func<TValue, ValueTask<Result<TFault>>> onSuccessResultFactory)
        {
            return result.Match<ValueTask<Result<TFault, TValue>>>(
                fault => new(Result<TFault, TValue>.Fail(fault)),
                async value =>
                {
                    var onSuccessResult = await onSuccessResultFactory(value).ConfigureAwait(false);
                    return onSuccessResult.Then(result);
                });
        }
    }
}