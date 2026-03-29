using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class GetFaultOrThrowResultPlainFactoryInternal
    {
        internal static TFault GetFaultOrThrow<TFault>(
            IResult<TFault> result,
            Func<Exception> exceptionFactory)
        {
            return result.Match(fault => fault, () => throw exceptionFactory());
        }

        internal static async Task<TFault> GetFaultOrThrow<TFault>(
            IResult<TFault> result,
            Func<Task<Exception>> exceptionFactory)
        {
            return await result.Match(
                fault => Task.FromResult(fault),
                async () => throw await exceptionFactory().ConfigureAwait(false)).ConfigureAwait(false);
        }

        internal static async ValueTask<TFault> GetFaultOrThrow<TFault>(
            IResult<TFault> result,
            Func<ValueTask<Exception>> exceptionFactory)
        {
            return await result.Match<ValueTask<TFault>>(
                fault => new(fault),
                async () => throw await exceptionFactory().ConfigureAwait(false)).ConfigureAwait(false);
        }
    }
}
