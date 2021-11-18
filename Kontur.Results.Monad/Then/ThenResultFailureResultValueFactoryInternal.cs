using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class ThenResultFailureResultValueFactoryInternal
    {
        internal static ResultFailure<T> Then<T, TFault, TValue>(
            ResultFailure<T> result,
            Func<Result<TFault, TValue>> onSuccessResultFactory)
        {
            _ = onSuccessResultFactory;
            return result;
        }

        internal static Task<ResultFailure<T>> Then<T, TFault, TValue>(
            ResultFailure<T> result,
            Func<Task<Result<TFault, TValue>>> onSuccessResultFactory)
        {
            _ = onSuccessResultFactory;
            return Task.FromResult(result);
        }

        internal static ValueTask<ResultFailure<T>> Then<T, TFault, TValue>(
            ResultFailure<T> result,
            Func<ValueTask<Result<TFault, TValue>>> onSuccessResultFactory)
        {
            _ = onSuccessResultFactory;
            return new(result);
        }
    }
}