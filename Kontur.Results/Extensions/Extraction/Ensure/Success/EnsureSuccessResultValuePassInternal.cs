using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class EnsureSuccessResultValuePassInternal
    {
        internal static void EnsureSuccess<TFault, TValue>(
            IResult<TFault, TValue> result,
            Func<TFault, Exception> exceptionFactory)
        {
            result.OnFailure(new Action<TFault>(fault => throw exceptionFactory(fault)));
        }

        internal static async Task EnsureSuccess<TFault, TValue>(
            IResult<TFault, TValue> result,
            Func<TFault, Task<Exception>> exceptionFactory)
        {
            await result.Match<Task>(
                async fault => throw await exceptionFactory(fault).ConfigureAwait(false),
                _ => Task.CompletedTask).ConfigureAwait(false);
        }

        internal static async ValueTask EnsureSuccess<TFault, TValue>(
            IResult<TFault, TValue> result,
            Func<TFault, ValueTask<Exception>> exceptionFactory)
        {
            await result.Match<ValueTask>(
                async fault => throw await exceptionFactory(fault).ConfigureAwait(false),
                _ => default).ConfigureAwait(false);
        }
    }
}
