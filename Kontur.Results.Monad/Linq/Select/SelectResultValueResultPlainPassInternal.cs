using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class SelectResultValueResultPlainPassInternal
    {
        internal static Result<TFault> Select<TFault, TValue>(
            Result<TFault, TValue> result,
            Func<TValue, Result<TFault>> onSuccessResultFactory)
        {
            return result.Match(Result<TFault>.Fail, onSuccessResultFactory);
        }

        internal static Task<Result<TFault>> Select<TFault, TValue>(
            Result<TFault, TValue> result,
            Func<TValue, Task<Result<TFault>>> onSuccessResultFactory)
        {
            return result.Match(
                fault => Task.FromResult(Result<TFault>.Fail(fault)),
                onSuccessResultFactory);
        }

        internal static ValueTask<Result<TFault>> Select<TFault, TValue>(
            Result<TFault, TValue> result,
            Func<TValue, ValueTask<Result<TFault>>> onSuccessResultFactory)
        {
            return result.Match(
                fault => new(Result<TFault>.Fail(fault)),
                onSuccessResultFactory);
        }
    }
}