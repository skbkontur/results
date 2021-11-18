using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class ThenResultFailureOptionalFactoryInternal
    {
        internal static ResultFailure<TFault> Then<TFault, TValue>(
            ResultFailure<TFault> result,
            Func<Optional<TValue>> onSuccessResultFactory)
        {
            _ = onSuccessResultFactory;
            return result;
        }

        internal static Task<ResultFailure<TFault>> Then<TFault, TValue>(
            ResultFailure<TFault> result,
            Func<Task<Optional<TValue>>> onSuccessResultFactory)
        {
            _ = onSuccessResultFactory;
            return Task.FromResult(result);
        }

        internal static ValueTask<ResultFailure<TFault>> Then<TFault, TValue>(
            ResultFailure<TFault> result,
            Func<ValueTask<Optional<TValue>>> onSuccessResultFactory)
        {
            _ = onSuccessResultFactory;
            return new(result);
        }
    }
}