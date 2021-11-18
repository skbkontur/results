using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class ThenResultValueResultFailurePassInternal
    {
        internal static ResultFailure<TFault> Then<TFault, TValue>(
            Result<TFault, TValue> result,
            Func<TValue, ResultFailure<TFault>> onSuccessResultFactory)
        {
            return result.Match(Result.Fail, value => onSuccessResultFactory(value));
        }

        internal static Task<ResultFailure<TFault>> Then<TFault, TValue>(
            Result<TFault, TValue> result,
            Func<TValue, Task<ResultFailure<TFault>>> onSuccessResultFactory)
        {
            return result.Match(
                fault => Task.FromResult(Result.Fail(fault)),
                onSuccessResultFactory);
        }

        internal static ValueTask<ResultFailure<TFault>> Then<TFault, TValue>(
            Result<TFault, TValue> result,
            Func<TValue, ValueTask<ResultFailure<TFault>>> onSuccessResultFactory)
        {
            return result.Match(
                fault => new(Result.Fail(fault)),
                onSuccessResultFactory);
        }
    }
}