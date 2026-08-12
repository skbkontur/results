using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class EnsureFailureResultValuePassInternal
    {
        internal static void EnsureFailure<TFault, TValue>(
            IResult<TFault, TValue> result,
            Func<TValue, Exception> exceptionFactory)
        {
            result.OnSuccess(new Action<TValue>(value => throw exceptionFactory(value)));
        }

        internal static async Task EnsureFailure<TFault, TValue>(
            IResult<TFault, TValue> result,
            Func<TValue, Task<Exception>> exceptionFactory)
        {
            await result.Match<Task>(
                _ => Task.CompletedTask,
                async value => throw await exceptionFactory(value).ConfigureAwait(false)).ConfigureAwait(false);
        }

        internal static async ValueTask EnsureFailure<TFault, TValue>(
            IResult<TFault, TValue> result,
            Func<TValue, ValueTask<Exception>> exceptionFactory)
        {
            await result.Match<ValueTask>(
                _ => default,
                async value => throw await exceptionFactory(value).ConfigureAwait(false)).ConfigureAwait(false);
        }
    }
}
