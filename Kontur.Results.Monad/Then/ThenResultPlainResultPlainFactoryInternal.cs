using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class ThenResultPlainResultPlainFactoryInternal
    {
        internal static Result<TFault> Then<TFault>(
            Result<TFault> result,
            Func<Result<TFault>> onSuccessResultFactory)
        {
            return result.Match(Result<TFault>.Fail, onSuccessResultFactory);
        }

        internal static Task<Result<TFault>> Then<TFault>(
            Result<TFault> result,
            Func<Task<Result<TFault>>> onSuccessResultFactory)
        {
            return result.Match(
                fault => Task.FromResult(Result<TFault>.Fail(fault)),
                onSuccessResultFactory);
        }

        internal static ValueTask<Result<TFault>> Then<TFault>(
            Result<TFault> result,
            Func<ValueTask<Result<TFault>>> onSuccessResultFactory)
        {
            return result.Match(
                fault => new(Result<TFault>.Fail(fault)),
                onSuccessResultFactory);
        }
    }
}