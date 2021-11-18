using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class ThenResultFailureResultFailureFactoryInternal
    {
        internal static ResultFailure<T> Then<T, TFault>(
            ResultFailure<T> result,
            Func<ResultFailure<TFault>> onSomeResultFactory)
        {
            _ = onSomeResultFactory;
            return result;
        }

        internal static Task<ResultFailure<T>> Then<T, TFault>(
            ResultFailure<T> result,
            Func<Task<ResultFailure<TFault>>> onSomeResultFactory)
        {
            _ = onSomeResultFactory;
            return Task.FromResult(result);
        }

        internal static ValueTask<ResultFailure<T>> Then<T, TFault>(
            ResultFailure<T> result,
            Func<ValueTask<ResultFailure<TFault>>> onSomeResultFactory)
        {
            _ = onSomeResultFactory;
            return new(result);
        }
    }
}