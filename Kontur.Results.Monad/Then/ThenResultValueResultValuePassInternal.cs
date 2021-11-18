using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class ThenResultValueResultValuePassInternal
    {
        internal static Result<TFault, TResult> Then<TFault, TValue, TResult>(
            Result<TFault, TValue> result,
            Func<TValue, Result<TFault, TResult>> onSuccessResultFactory)
        {
            return result.Match(Result<TFault, TResult>.Fail, onSuccessResultFactory);
        }

        internal static Task<Result<TFault, TResult>> Then<TFault, TValue, TResult>(
            Result<TFault, TValue> result,
            Func<TValue, Task<Result<TFault, TResult>>> onSuccessResultFactory)
        {
            return result.Match(
                fault => Task.FromResult(Result<TFault, TResult>.Fail(fault)),
                onSuccessResultFactory);
        }

        internal static ValueTask<Result<TFault, TResult>> Then<TFault, TValue, TResult>(
            Result<TFault, TValue> result,
            Func<TValue, ValueTask<Result<TFault, TResult>>> onSuccessResultFactory)
        {
            return result.Match(
                fault => new(Result<TFault, TResult>.Fail(fault)),
                onSuccessResultFactory);
        }
    }
}