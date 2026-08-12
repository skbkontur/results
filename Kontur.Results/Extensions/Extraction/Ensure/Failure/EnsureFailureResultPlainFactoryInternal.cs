using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class EnsureFailureResultPlainFactoryInternal
    {
        internal static void EnsureFailure<TFault>(
            IResult<TFault> result,
            Func<Exception> exceptionFactory)
        {
            result.OnSuccess(new Action(() => throw exceptionFactory()));
        }

        internal static async Task EnsureFailure<TFault>(
            IResult<TFault> result,
            Func<Task<Exception>> exceptionFactory)
        {
            await result.Match<Task>(
                _ => Task.CompletedTask,
                async () => throw await exceptionFactory().ConfigureAwait(false)).ConfigureAwait(false);
        }

        internal static async ValueTask EnsureFailure<TFault>(
            IResult<TFault> result,
            Func<ValueTask<Exception>> exceptionFactory)
        {
            await result.Match<ValueTask>(
                _ => default,
                async () => throw await exceptionFactory().ConfigureAwait(false)).ConfigureAwait(false);
        }
    }
}
