using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class EnsureSuccessResultPlainPassInternal
    {
        internal static void EnsureSuccess<TFault>(
            IResult<TFault> result,
            Func<TFault, Exception> exceptionFactory)
        {
            result.OnFailure(new Action<TFault>(fault => throw exceptionFactory(fault)));
        }

        internal static async Task EnsureSuccess<TFault>(
            IResult<TFault> result,
            Func<TFault, Task<Exception>> exceptionFactory)
        {
            await result.Match<Task>(
                async fault => throw await exceptionFactory(fault).ConfigureAwait(false),
                () => Task.CompletedTask).ConfigureAwait(false);
        }

        internal static async ValueTask EnsureSuccess<TFault>(
            IResult<TFault> result,
            Func<TFault, ValueTask<Exception>> exceptionFactory)
        {
            await result.Match<ValueTask>(
                async fault => throw await exceptionFactory(fault).ConfigureAwait(false),
                () => default).ConfigureAwait(false);
        }
    }
}
