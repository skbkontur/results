using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class ThenResultPlainResultValueFactoryInternal
    {
        internal static Result<TFault, TValue> Then<TFault, TValue>(
            Result<TFault> result,
            Func<Result<TFault, TValue>> onSuccessResultFactory)
        {
            return result.Match(Result<TFault, TValue>.Fail, onSuccessResultFactory);
        }

        internal static Task<Result<TFault, TValue>> Then<TFault, TValue>(
            Result<TFault> result,
            Func<Task<Result<TFault, TValue>>> onSuccessResultFactory)
        {
            return result.Match(
                    fault => Task.FromResult(Result<TFault, TValue>.Fail(fault)),
                    onSuccessResultFactory);
        }

        internal static ValueTask<Result<TFault, TValue>> Then<TFault, TValue>(
            Result<TFault> result,
            Func<ValueTask<Result<TFault, TValue>>> onSuccessResultFactory)
        {
            return result.Match(
                    fault => new(Result<TFault, TValue>.Fail(fault)),
                    onSuccessResultFactory);
        }
    }
}