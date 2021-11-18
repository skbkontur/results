using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class SelectResultValueResultValuePassInternal
    {
        internal static Result<TFault, TResult> Select<TFault, TValue, TResult>(
            Result<TFault, TValue> result,
            Func<TValue, Result<TFault, TResult>> resultFactory)
        {
            return result.Then(resultFactory);
        }

        internal static Task<Result<TFault, TResult>> Select<TFault, TValue, TResult>(
            Result<TFault, TValue> result,
            Func<TValue, Task<Result<TFault, TResult>>> resultFactory)
        {
            return result.Then(resultFactory);
        }

        internal static ValueTask<Result<TFault, TResult>> Select<TFault, TValue, TResult>(
            Result<TFault, TValue> result,
            Func<TValue, ValueTask<Result<TFault, TResult>>> resultFactory)
        {
            return result.Then(resultFactory);
        }
    }
}