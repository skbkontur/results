using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class GetFaultOrThrowResultValuePassInternal
    {
        internal static TFault GetFaultOrThrow<TFault, TValue>(
            IResult<TFault, TValue> result,
            Func<TValue, Exception> exceptionFactory)
        {
            return result.Match(fault => fault, value => throw exceptionFactory(value));
        }

        internal static async Task<TFault> GetFaultOrThrow<TFault, TValue>(
            IResult<TFault, TValue> result,
            Func<TValue, Task<Exception>> exceptionFactory)
        {
            return await result.Match(
                fault => Task.FromResult(fault),
                async value => throw await exceptionFactory(value).ConfigureAwait(false)).ConfigureAwait(false);
        }

        internal static async ValueTask<TFault> GetFaultOrThrow<TFault, TValue>(
            IResult<TFault, TValue> result,
            Func<TValue, ValueTask<Exception>> exceptionFactory)
        {
            return await result.Match<ValueTask<TFault>>(
                fault => new(fault),
                async value => throw await exceptionFactory(value).ConfigureAwait(false)).ConfigureAwait(false);
        }
    }
}
