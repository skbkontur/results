using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class SelectOptionalResultValuePassInternal
    {
        internal static Optional<TResult> Select<TFault, TValue, TResult>(
            Optional<TValue> optional,
            Func<TValue, Result<TFault, TResult>> resultFactory)
        {
            return optional.Then(resultFactory);
        }

        internal static Task<Optional<TResult>> Select<TFault, TValue, TResult>(
            Optional<TValue> optional,
            Func<TValue, Task<Result<TFault, TResult>>> resultFactory)
        {
            return optional.Then(resultFactory);
        }

        internal static ValueTask<Optional<TResult>> Select<TFault, TValue, TResult>(
            Optional<TValue> optional,
            Func<TValue, ValueTask<Result<TFault, TResult>>> resultFactory)
        {
            return optional.Then(resultFactory);
        }
    }
}