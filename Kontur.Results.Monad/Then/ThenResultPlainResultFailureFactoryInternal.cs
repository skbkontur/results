using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class ThenResultPlainResultFailureFactoryInternal
    {
        internal static ResultFailure<TFault> Then<TFault>(
            Result<TFault> result,
            Func<ResultFailure<TFault>> onSuccessResultFactory)
        {
            return result.Match(Result.Fail, onSuccessResultFactory);
        }

        internal static Task<ResultFailure<TFault>> Then<TFault>(
            Result<TFault> result,
            Func<Task<ResultFailure<TFault>>> onSuccessResultFactory)
        {
            return result.Match(
                fault => Task.FromResult(Result.Fail(fault)),
                onSuccessResultFactory);
        }

        internal static ValueTask<ResultFailure<TFault>> Then<TFault>(
            Result<TFault> result,
            Func<ValueTask<ResultFailure<TFault>>> onSuccessResultFactory)
        {
            return result.Match(
                fault => new(Result.Fail(fault)),
                onSuccessResultFactory);
        }
    }
}